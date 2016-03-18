using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    public partial class AppWindow : Form
    {
        Matrix viewTransform = new Matrix();
        Matrix defaultView = new Matrix();
        Point dragPos;
        bool displayAxes = true;
        int maxAxes = 20;

        Body selectedBody = Body.NONE;
        LightRay selectedLightRay = LightRay.NONE;
        PointRotor lightRayRotor;
        bool movingLray = false;

        Scene scene;
        Timer physT;

        OpenFileDialog openSceneBinary;
        SaveFileDialog saveSceneBinary;

        LightRay clipboardLray;
        Body clipboardBody;

        string lastSave;
        bool sameToSave = true;
        public string windowText = "Optiks Csharp V.1.4.2 - ";

        public AppWindow()
        {
            InitializeComponent();
        }

        private void AppWindow_Load(object sender, EventArgs e)
        {

            this.canvas.MouseWheel += canvas_MouseWheel;
            this.canvas.MouseMove += canvas_MouseMove;
            this.canvas.MouseDown += canvas_MouseDown;
            this.canvas.MouseUp += canvas_MouseUp;
            this.canvas.PreviewKeyDown += canvas_PreviewKeyDown;

            defaultView = new Matrix(10, 0, 0, 10, canvas.Width/2, canvas.Height/2);

            lastSave = "New File";
            Text = windowText + lastSave;
            scene = new Scene(new List<Body>
            {
                new Body(
                    new List<Line>
                    {
                        new Segment(new Vector(0, 0), new Vector(10, 0)),
                        new Segment(new Vector(10, 0), new Vector(10, 2)),
                        new Segment(new Vector(10, 2), new Vector(0, 2)),
                        new ParabolicBezier(new Vector(0, 2), new Vector(0, 0), 1)
                    },
                    1.5,
                    BodyTypes.Refracting,
                    new Pen(Color.Black, 1),
                    new SolidBrush(Color.FromArgb(50, Color.Aqua)),
                    DrawTypes.Draw | DrawTypes.Fill
                )
            }, new List<LightRay>
            {
                new LightRay(
                    new Ray(new Vector(0.3, 2), Vector.fromAngle(-Math.PI/2)),
                    10,
                    Pens.Green
                )
            });

            setViewMode(ViewModes.Edit);
            physT = new Timer();
            physT.Interval = 1000;
            physT.Tick += updateScene;

            openSceneBinary = new OpenFileDialog();
            openSceneBinary.Filter = "Optiks Scene Files (*.opt)|*.opt|All Files (*.*)|*.*";
            openSceneBinary.FilterIndex = 0;
            openSceneBinary.DefaultExt = "opt";
            openSceneBinary.Multiselect = false;

            saveSceneBinary = new SaveFileDialog();
            saveSceneBinary.Filter = "Optiks Scene Files (*.opt)|*.opt|All Files (*.*)|*.*";
            saveSceneBinary.DefaultExt = "opt";
        }

        public void newScene()
        {
            DialogResult ok;
            if (sameToSave || (scene.bodies.Count == 0 && scene.lightRays.Count == 0))
            {
                ok = DialogResult.No;
            }
            else
            {
                ok = MessageBox.Show(
                    "Some changes have not been saved. Save now?",
                    "Not saved!",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );
            }

            if (ok == DialogResult.Yes) {
                askSaveSceneFile(true);
            }

            if (ok == DialogResult.Cancel)
            {
                return;
            }

            viewTransform = defaultView;
            selectedBody = Body.NONE;
            selectedLightRay = LightRay.NONE;
            lastSave = "New File";
            Text = windowText + lastSave;
            scene = new Scene(new List<Body>(), new List<LightRay>());
            sameToSave = true;
            canvas.Invalidate();
        }

        public void askOpenSceneFile()
        {
            DialogResult ok;
            if (sameToSave || (scene.bodies.Count == 0 && scene.lightRays.Count == 0))
            {
                ok = DialogResult.No;
            }
            else
            {
                ok = MessageBox.Show(
                    "Some changes have not been saved. Save now?",
                    "Not saved!",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );
            }

            if (ok == DialogResult.Yes)
            {
                askSaveSceneFile(true);
            }

            if (ok == DialogResult.Cancel)
            {
                return;
            }

            ok = openSceneBinary.ShowDialog();

            if (ok == DialogResult.OK)
            {
                sceneFromFile(openSceneBinary.FileName);
                lastSave = openSceneBinary.FileName;
                Text = windowText + lastSave;
                sameToSave = true;
            }
        }

        public void askSaveSceneFile(bool useLastSave)
        {
            if (useLastSave && lastSave != "New File")
            {
                fileFromScene(lastSave);
                return;
            }

            DialogResult ok = saveSceneBinary.ShowDialog();

            if (ok == DialogResult.OK)
            {
                fileFromScene(saveSceneBinary.FileName);
                lastSave = saveSceneBinary.FileName;
                Text = windowText + lastSave;
                sameToSave = true;
            }
        }

        public void sceneFromFile(string path)
        {
            try
            {
                scene = FileStruct.toScene(File.ReadAllBytes(path));
                viewTransform = defaultView;
                this.canvas.Invalidate();
            }
            catch (IOException e)
            {
                if (e.HResult == 1)
                {
                    MessageBox.Show(
                        "The file you specified is not a valid Optiks CSharp file.", 
                        "File error", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                if (e.HResult == 2)
                {
                    MessageBox.Show(
                        "The file you specified is either corrupted or somehow invalid", 
                        "File error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        public void fileFromScene(string path)
        {
            File.WriteAllBytes(path, FileStruct.toBytes(scene));
        }

        private void resetView()
        {
            viewTransform = defaultView.Clone();
            canvas.Invalidate();
        }

        private void setViewMode(ViewModes v)
        {
            if (v == UIConstants.viewMode)
            {
                return;
            }
            UIConstants.viewMode = v;
            selectedBody = Body.NONE;
            selectedLightRay = LightRay.NONE;

            if (UIConstants.viewMode == ViewModes.Edit) { scene.reset(); }
            if (UIConstants.viewMode == ViewModes.RunSim) { physT.Start(); } 

            canvas.Invalidate();
        }

        private void updateScene(object sender, EventArgs e)
        {
            var all = scene.lightRays.All(x => x.stopped);
            if (!all)
            {
                scene.physicsTick();
                canvas.Invalidate();
                return;
            }
            physT.Stop();
            setViewMode(ViewModes.GetInfo);
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            scene.renderBodies(e.Graphics, viewTransform);
            scene.renderLightRays(e.Graphics, viewTransform);

            Pen axisPen = new Pen(Color.FromArgb(128, 100, 100, 100), 1);

            //Graph section
            if (displayAxes)
            {
                e.Graphics.SmoothingMode = SmoothingMode.None;

                e.Graphics.DrawLine(axisPen, 0, viewTransform.OffsetY, canvas.Width, viewTransform.OffsetY);
                e.Graphics.DrawLine(axisPen, viewTransform.OffsetX, 0, viewTransform.OffsetX, canvas.Height);


                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }

            //Selected hitbox & body
            if (selectedBody)
            {
                var points = new PointF[] { selectedBody.bounds.Location,
                    selectedBody.bounds.Location + selectedBody.bounds.Size };

                viewTransform.TransformPoints(points);
                e.Graphics.DrawRectangle(
                    new Pen(Color.LightGreen, 2), 
                    points[0].X, 
                    points[0].Y, 
                    points[1].X - points[0].X, 
                    points[1].Y - points[0].Y
                );

                foreach (Line l in selectedBody.segments)
                {
                    var vp = viewTransform * l.vertex;
                    if (l.type != LineTypes.Straight)
                    {
                        Pen pen = (l.type == LineTypes.CircleArc) ? Pens.Orange : Pens.Green;
                        Brush brush = (l.type == LineTypes.CircleArc) ? Brushes.Orange : Brushes.Green;

                        var fp = viewTransform * l.focalPoint;
                        e.Graphics.DrawLine(pen, vp, fp);
                        fp.render(3, brush, e.Graphics);
                    }
                    vp.render(3, Brushes.Blue, e.Graphics);
                }
            }

            //Seected lightray
            if (selectedLightRay)
            {
                selectedLightRay.rays[0].render(selectedLightRay.pen, e.Graphics, viewTransform);
                lightRayRotor.display(selectedLightRay.rays[0].udir, e.Graphics, viewTransform);

                foreach (Body b in scene.bodies)
                {
                    foreach (Line l in b.segments)
                    {
                        var vp = viewTransform * l.vertex;
                        if (l.type != LineTypes.Straight)
                        {
                            Pen pen = (l.type == LineTypes.CircleArc) ? Pens.Orange : Pens.Green;
                            Brush brush = (l.type == LineTypes.CircleArc) ? Brushes.Orange : Brushes.Green;
                            
                            var fp = viewTransform * l.focalPoint;
                            e.Graphics.DrawLine(pen, vp, fp);
                            fp.render(3, brush, e.Graphics);
                        }
                        vp.render(3, Brushes.Blue, e.Graphics);
                    }
                }
            }

            //Sides change color when you are focused
            Color col = canvas.Focused ? Color.Black : Color.LightGray;
            ControlPaint.DrawBorder(e.Graphics, canvas.ClientRectangle, col, ButtonBorderStyle.Solid);
        }

        public void canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            viewTransform.Translate(-e.X, -e.Y, MatrixOrder.Append);
            viewTransform.Scale(1 + (float)e.Delta / 1000, 1 + (float)e.Delta / 1000, MatrixOrder.Append);
            viewTransform.Translate(e.X, e.Y, MatrixOrder.Append);

            this.canvas.Invalidate();
        }

        public void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Right))
            {
                if (dragPos.X == -1 && dragPos.Y == -1)
                {
                    dragPos = new Point(e.X, e.Y);
                    canvas.Cursor = Cursors.SizeAll;
                    return;
                }

                var delta = new Point(e.X - dragPos.X, e.Y - dragPos.Y);
                viewTransform.Translate(delta.X, delta.Y, MatrixOrder.Append);

                dragPos = new Point(e.X, e.Y);

                this.canvas.Invalidate();
            }
            else
            {
                dragPos = new Point(-1, -1);
                canvas.Cursor = Cursors.Default;
            }

            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (selectedLightRay)
                {
                    if ((viewTransform * selectedLightRay.rays[0].start - e.Location).lenSqr() > 25 && !movingLray)
                    {
                        sameToSave = false;
                        var locks = new List<Vector>();
                        foreach (Body b in scene.bodies)
                        {
                            foreach (Line l in b.segments)
                            {
                                if (l.type != LineTypes.Straight) { locks.Add(l.focalPoint); }
                            }
                        }

                        lightRayRotor.setUnitVector(ref selectedLightRay.rays[0].udir, new Vector(e.Location), viewTransform, locks.ToArray(), 5);
                    }
                    if (movingLray)
                    {
                        sameToSave = false;
                        selectedLightRay.rays[0].start = viewTransform.inverseTransform(e.Location);
                        lightRayRotor.rotationCenter = selectedLightRay.rays[0].start;
                    }
                    canvas.Invalidate();
                }
            }
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            var scaledMousePos = viewTransform.inverseTransform(e.Location);
            var s = viewTransform.Elements[0]; // Scale component

            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (selectedLightRay)
                {
                    foreach (Body b in scene.bodies)
                    {
                        foreach (Line l in b.segments)
                        {
                            if ((scaledMousePos - l.vertex).lenSqr() * s * s <= 25)
                            {
                                sameToSave = false;
                                selectedLightRay.rays[0].udir = l.normal * (Math.Sign(l.normal * (l.vertex - selectedLightRay.rays[0].start)) | 1);
                                canvas.Invalidate();
                                return;
                            }
                        }
                    }

                    if ((viewTransform * selectedLightRay.rays[0].start - e.Location).lenSqr() <= 25)
                    {
                        movingLray = true;
                    }
                }
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                movingLray = false;
            }
        }

        private void canvas_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var mousePos = canvas.PointToClient(Cursor.Position);

            // Use fake events, because lazyness is a quality
            if (e.KeyCode == Keys.Oemplus)
            {
                canvas_MouseWheel(false, new MouseEventArgs(MouseButtons.None, 0, mousePos.X, mousePos.Y, 100));
            }
            if (e.KeyCode == Keys.OemMinus)
            {
                canvas_MouseWheel(false, new MouseEventArgs(MouseButtons.None, 0, mousePos.X, mousePos.Y, -100));
            }
            if (e.KeyCode == Keys.R)
            {
                Vector start = viewTransform.inverseTransform(canvas.PointToClient(Cursor.Position));
                scene.lightRays.Add(new LightRay(new Ray(start, new Vector(0, -1)), 10, Pens.Green));
                canvas.Invalidate();
            }
        }

        private void canvas_MouseLeave(object sender, EventArgs e)
        {
            unfocusLabel.Focus();
            canvas.Invalidate();
        }

        private void canvas_MouseEnter(object sender, EventArgs e)
        {
            canvas.Focus();
            canvas.Invalidate();
        }

        private void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (UIConstants.viewMode != ViewModes.Edit)
            {
                return;
            }

            selectedLightRay = LightRay.NONE;
            selectedBody = Body.NONE;
            Vector scaledMousePos = viewTransform.inverseTransform(new Vector(e.Location));

            foreach (LightRay r in scene.lightRays)
            {

                if ((scaledMousePos - r.rays[0].start).len() * viewTransform.Elements[0] <= 7) {
                    selectedLightRay = r;
                    lightRayRotor = new PointRotor(selectedLightRay.rays[0].start);
                    canvas.Invalidate();
                    return;
                }
            }

            List<Body> lB = scene.bodies;
            lB.Sort((x1, x2) => (int)(x2.bounds.Height * x2.bounds.Width - x1.bounds.Height * x1.bounds.Width));

            foreach (Body b in lB)
            {
                GraphicsPath gPath = (GraphicsPath)b.gpath.Clone();
                gPath.Transform(viewTransform);

                if (gPath.IsVisible(e.X, e.Y))
                {
                    selectedBody = b;
                    canvas.Invalidate();
                    return;
                }
            }

            canvas.Invalidate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            askOpenSceneFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            askSaveSceneFile(true);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            askSaveSceneFile(false);
        }

        private void axesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            displayAxes = !displayAxes;
            canvas.Invalidate();
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newScene();
        }

        private void pointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UIConstants.pointDisplay = PointDisplayModes.Circle;
            pointToolStripMenuItem.Checked = true;
            crossToolStripMenuItem.Checked = false;
            canvas.Invalidate();
        }

        private void crossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UIConstants.pointDisplay = PointDisplayModes.Cross;
            pointToolStripMenuItem.Checked = false;
            crossToolStripMenuItem.Checked = true;
            canvas.Invalidate();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setViewMode(ViewModes.Edit);
        }

        private void runSimulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setViewMode(ViewModes.RunSim);
        }

        private void canvas_SizeChanged(object sender, EventArgs e)
        {
            defaultView = new Matrix(10, 0, 0, 10, canvas.Width / 2, canvas.Height / 2);
            canvas.Invalidate();
        }

        private void resetViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetView();
        }
    }
}
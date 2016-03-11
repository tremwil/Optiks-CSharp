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
    enum ViewModes
    {
        /// <summary>
        /// Mode in which the user can interact and modify the scene.
        /// </summary>
        Edit,
        /// <summary>
        /// Mode in which the user waits for the simulation to finish.
        /// </summary>
        RunSim,
        /// <summary>
        /// Mode in which the user can retrieve information from the simulation.
        /// </summary>
        GetInfo
    }

    public partial class AppWindow : Form
    {
        Matrix viewTransform = new Matrix();
        Matrix defaultView = new Matrix();
        Point dragPos;

        Body selectedBody = Body.NONE;
        LightRay selectedLightRay = LightRay.NONE;
        UI.PointRotor lightRayRotor;
        bool movingLray = false;

        ViewModes viewMode;

        Scene scene;
        Timer renderT;

        OpenFileDialog openSceneBinary;
        SaveFileDialog saveSceneBinary;

        string lastSave;

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

            viewMode = ViewModes.Edit;

            defaultView = new Matrix();
            defaultView.Scale(10, 10, MatrixOrder.Append);
            defaultView.Translate(canvas.Width / 2, canvas.Height / 2, MatrixOrder.Append);

            lastSave = "";
            scene = new Scene(new List<Body>
            {
                new Body(
                    new List<Line>
                    {
                        new CircleArc(new Vector(0, 0), new Vector(1, 1), -0.2),
                        new ParabolicBezier(new Vector(1, 1), new Vector(0, 1), -0.2),
                        new Segment(new Vector(0, 1), new Vector(0, 0))
                    },
                    2.3,
                    BodyTypes.Reflecting,
                    new Pen(Color.Black, 2),
                    (SolidBrush)Brushes.White,
                    DrawTypes.Draw | DrawTypes.Fill
                )
            }, new List<LightRay>
            {
                new LightRay(
                    new Ray(new Vector(1, 0.5), Vector.fromAngle(Math.PI)),
                    30,
                    new Pen(Color.Yellow, 2)
                )
            });

            debugText.Text = scene.bodies[0].segments[1].bezierHandle.ToString();

            openSceneBinary = new OpenFileDialog();
            openSceneBinary.Filter = "Optiks Scene Files (*.opt)|*.opt|All Files (*.*)|*.*";
            openSceneBinary.FilterIndex = 0;
            openSceneBinary.DefaultExt = "opt";
            openSceneBinary.Multiselect = false;

            saveSceneBinary = new SaveFileDialog();
            saveSceneBinary.Filter = "Optiks Scene Files (*.opt)|*.opt|All Files (*.*)|*.*";
            saveSceneBinary.DefaultExt = "opt";


            //askOpenSceneFile();
        }

        public void newScene()
        {
            if (scene.bodies.Count == 0 && scene.lightRays.Count == 0 && scene.airRefractionIndex == 1)
            {
                return;
            }

            DialogResult ok = MessageBox.Show(
                "Some changes have not been saved. Save now?",
                "Not saved!",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (ok == DialogResult.Yes) {
                askSaveSceneFile(true);
            }

            if (ok == DialogResult.None)
            {
                return;
            }

            viewTransform = new Matrix();
            lastSave = "";
            scene = new Scene(new List<Body>(), new List<LightRay>());
            this.canvas.Invalidate();
        }

        public void askOpenSceneFile()
        {
            DialogResult ok = openSceneBinary.ShowDialog();

            if (ok == DialogResult.OK)
            {
                sceneFromFile(openSceneBinary.FileName);
                lastSave = openSceneBinary.FileName;
            }
        }

        public void askSaveSceneFile(bool useLastSave)
        {
            if (useLastSave && lastSave != "")
            {
                fileFromScene(lastSave);
            }

            DialogResult ok = saveSceneBinary.ShowDialog();

            if (ok == DialogResult.OK)
            {
                fileFromScene(saveSceneBinary.FileName);
                lastSave = saveSceneBinary.FileName;
            }
        }

        public void sceneFromFile(string path)
        {
            try
            {
                scene = FileStruct.toScene(File.ReadAllBytes(path));
                viewTransform = new Matrix();
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

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            scene.renderBodies(e.Graphics, viewTransform, viewMode);
            scene.renderLightRays(e.Graphics, viewTransform, viewMode);

            Pen axisPen = new Pen(Color.FromArgb(128, 100, 100, 100), 1);

            //Graph section
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.DrawLine(axisPen, 0, viewTransform.OffsetY, canvas.Width, viewTransform.OffsetY);
            e.Graphics.DrawLine(axisPen, viewTransform.OffsetX, 0, viewTransform.OffsetX, canvas.Height);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

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
                    if (l.type == LineTypes.CircleArc)
                    {
                        Vector focus = viewTransform * (l.center + l.pointCW * l.radius / 2 * l.normal);
                        Vector curvePt = viewTransform * (l.center + l.pointCW * l.radius * l.normal);
                        e.Graphics.DrawLine(Pens.Orange, focus, curvePt);
                        focus.render(3, Brushes.Orange, e.Graphics);
                    }
                    if (l.type == LineTypes.Parabolic)
                    {
                        e.Graphics.DrawLine(Pens.Green, viewTransform * l.vertex, viewTransform * l.focalPoint);
                        (viewTransform * l.focalPoint).render(3, Brushes.Green, e.Graphics);
                    }
                }
            }

            //Seected lightray
            if (selectedLightRay)
            {
                selectedLightRay.rays[0].render(selectedLightRay.pen, e.Graphics, viewTransform);
                lightRayRotor.display(selectedLightRay.rays[0].udir, e.Graphics, viewTransform);
            }

            //Sides chaange color when you are focused
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
            }

            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (selectedLightRay)
                {
                    if ((viewTransform * selectedLightRay.rays[0].start - e.Location).lenSqr() > 25 && !movingLray)
                    {
                        var locks = scene.bodies[0].gpath.PathPoints.Select(x => new Vector(x)).ToArray();
                        lightRayRotor.setUnitVector(ref selectedLightRay.rays[0].udir, new Vector(e.Location), viewTransform, locks, 5);
                    }
                    if (movingLray)
                    {
                        selectedLightRay.rays[0].start = viewTransform.inverseTransform(e.Location);
                        lightRayRotor.rotationCenter = selectedLightRay.rays[0].start;
                    }
                    canvas.Invalidate();
                }
            }
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (selectedLightRay)
                {
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
            if (e.KeyCode == Keys.F1)
            {
                resetView();
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
            if (viewMode != ViewModes.Edit)
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
                    lightRayRotor = new UI.PointRotor(selectedLightRay.rays[0].start);
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
    }
}
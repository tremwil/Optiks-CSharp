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

        Body selectedBody = Body.NONE;

        enum ViewModes
        {
            Edit,
            RunSim,
            GetInfo
        }

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
                        new Segment(new Vector(000, 000), new Vector(100, 100)),
                        new Segment(new Vector(100, 100), new Vector(000, 100)),
                        new Segment(new Vector(000, 100), new Vector(000, 000))
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
                    new Ray(new Vector(100, 75), Vector.fromAngle(Math.PI * 1.125)),
                    30,
                    new Pen(Color.Yellow, 2)
                )
            });

            scene.physicsTick();

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

            scene.renderBodies(e.Graphics, viewTransform);
            scene.lightRays[0].rays[0].render(scene.lightRays[0].pen, e.Graphics, viewTransform);

            Pen axisPen = new Pen(Color.FromArgb(128, 100, 100, 100), 1);

            e.Graphics.DrawLine(axisPen, 0, viewTransform.OffsetY, canvas.Width, viewTransform.OffsetY);
            e.Graphics.DrawLine(axisPen, viewTransform.OffsetX, 0, viewTransform.OffsetX, canvas.Height);

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
            }

            Color col = canvas.Focused ? Color.Black : Color.DimGray;
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
        }

        private void canvas_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            debugText.Text = e.KeyData.ToString();

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
            Matrix viewMatInvert = viewTransform.Clone(); viewMatInvert.Invert();
            PointF[] lP = new PointF[] { e.Location }; viewMatInvert.TransformPoints(lP);
            PointF worldPoint = lP[0];

            selectedBody = Body.NONE;

            List<Body> lB = scene.bodies;
            lB.Sort((x1, x2) => (int)(x1.bounds.Height * x1.bounds.Width - x2.bounds.Height * x2.bounds.Width));

            foreach (Body b in lB)
            {
                if (b.gpath.IsVisible(worldPoint))
                {
                    selectedBody = b;
                }
            }

            canvas.Invalidate();
        }
    }
}
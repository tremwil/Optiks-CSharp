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
        Matrix defaultView;
        Point dragPos;

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
                    new Pen(Color.Black, 5),
                    (SolidBrush)Brushes.White,
                    DrawTypes.Draw | DrawTypes.Fill
                )
            }, new List<LightRay>
            {
                new LightRay(
                    new Ray(new Vector(150, 50), new Vector(-1, 0)),
                    30,
                    new Pen(Color.Yellow, 5)
                )
            });

            openSceneBinary = new OpenFileDialog();
            openSceneBinary.Filter = "Optiks Scene Files (*.opt)|*.opt|All Files (*.*)|*.*";
            openSceneBinary.FilterIndex = 0;
            openSceneBinary.DefaultExt = "opt";
            openSceneBinary.Multiselect = false;

            saveSceneBinary = new SaveFileDialog();
            saveSceneBinary.Filter = "Optiks Scene Files (*.opt)|*.opt|All Files (*.*)|*.*";
            saveSceneBinary.DefaultExt = "opt";


            askOpenSceneFile();
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
                        "The file you specified is either corrupted or invalid", 
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

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            scene.renderBodies(e.Graphics, viewTransform);
            scene.lightRays[0].rays[0].render(scene.lightRays[0].pen, e.Graphics, viewTransform);
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
    }
}
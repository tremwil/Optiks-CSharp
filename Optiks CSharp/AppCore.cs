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

        bool displayGrad = true;
        bool AAenabled = true;
        int maxGridSize = 150;
        int minGridSize = 15;
        int minTextGridSize = 30;

        Body selectedBody = Body.NONE;
        int selectedBodyIndex = 0;
        LightRay selectedLightRay = LightRay.NONE;
        int selectedLightRayIndex = 0;
        PointRotor lightRayRotor;
        bool movingLray = false;

        Scene scene;
        Timer physT;

        OpenFileDialog openSceneBinary;
        SaveFileDialog saveSceneBinary;

        LightRay ClipboardLray = LightRay.NONE;
        Body clipboardBody = Body.NONE;

        string lastSave;
        bool sameToSave = true;
        public string windowText = "Optiks Csharp V.1.4.2 - ";

        bool zoomEnd = false;

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

            defaultView = new Matrix(10, 0, 0, 10, canvas.Width / 2, canvas.Height / 2);

            lastSave = "New File";
            Text = windowText + lastSave;
            scene = new Scene(new List<Body>
            {
                new Body(
                    new List<Line>
                    {
                        new HyperbolicSurface(new Vector(0, 0), new Vector(10, 0), -2, 1.5),
                        new Segment(new Vector(10, 0), new Vector(10, 0)),
                        new Segment(new Vector(10, 0), new Vector(0, 0)),
                        new Segment(new Vector(0, 0), new Vector(0, 0))
                    },
                    1.5,
                    DispertionCoefs.Moderate,
                    BodyTypes.Refracting,
                    new Pen(Color.Black, 1),
                    new SolidBrush(Color.FromArgb(50, Color.LightSeaGreen)),
                    DrawTypes.Draw | DrawTypes.Fill
                ),
            }, new List<LightRay>
            {
                new LightRay(
                    new Ray(new Vector(1, 4), Vector.fromAngle(-Math.PI/2)),
                    10,
                    480,
                    2
                ),
                new LightRay(
                    new Ray(new Vector(2, 4), Vector.fromAngle(-Math.PI/2)),
                    10,
                    500,
                    2
                ),
                new LightRay(
                    new Ray(new Vector(3, 4), Vector.fromAngle(-Math.PI/2)),
                    10,
                    520,
                    2
                ),
                new LightRay(
                    new Ray(new Vector(4, 4), Vector.fromAngle(-Math.PI/2)),
                    10,
                    540,
                    2
                ),
                new LightRay(
                    new Ray(new Vector(5, 4), Vector.fromAngle(-Math.PI/2)),
                    10,
                    560,
                    2
                ),
                new LightRay(
                    new Ray(new Vector(6, 4), Vector.fromAngle(-Math.PI/2)),
                    10,
                    580,
                    2
                ),
                new LightRay(
                    new Ray(new Vector(7, 4), Vector.fromAngle(-Math.PI/2)),
                    10,
                    600,
                    2
                ),
                new LightRay(
                    new Ray(new Vector(8, 4), Vector.fromAngle(-Math.PI/2)),
                    10,
                    620,
                    2
                ),
                new LightRay(
                    new Ray(new Vector(9, 4), Vector.fromAngle(-Math.PI/2)),
                    10,
                    640,
                    2
                ),
            });

            setViewMode(ViewModes.Edit);
            physT = new Timer();
            physT.Interval = 1;
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

            if (ok == DialogResult.Yes)
            {
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
            if (v == StaticParameters.viewMode)
            {
                return;
            }
            StaticParameters.viewMode = v;
            selectedBody = Body.NONE;
            selectedLightRay = LightRay.NONE;

            if (StaticParameters.viewMode == ViewModes.Edit) { scene.reset(); physT.Stop(); }
            if (StaticParameters.viewMode == ViewModes.RunSim) { physT.Start(); }

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
    }
}
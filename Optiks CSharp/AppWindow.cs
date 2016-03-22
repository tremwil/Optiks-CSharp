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
        public void canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (viewTransform.Elements[0] > 500000 && e.Delta > 0)
            {
                zoomEnd = true;
                canvas.Invalidate();
                return;
            }
            zoomEnd = false;

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
            if (StaticParameters.viewMode != ViewModes.Edit)
            {
                return;
            }

            selectedLightRay = LightRay.NONE;
            selectedBody = Body.NONE;
            Vector scaledMousePos = viewTransform.inverseTransform(new Vector(e.Location));

            int i = 0;
            foreach (LightRay r in scene.lightRays)
            {

                if ((scaledMousePos - r.rays[0].start).len() * viewTransform.Elements[0] <= 7)
                {
                    selectedLightRay = r;
                    selectedLightRayIndex = i;
                    lightRayRotor = new PointRotor(selectedLightRay.rays[0].start);
                    canvas.Invalidate();
                    return;
                }
                i++;
            }

            List<Body> lB = scene.bodies;
            lB.Sort((x1, x2) => (int)(x2.bounds.Height * x2.bounds.Width - x1.bounds.Height * x1.bounds.Width));

            i = 0;
            foreach (Body b in lB)
            {
                GraphicsPath gPath = (GraphicsPath)b.gpath.Clone();
                gPath.Transform(viewTransform);

                if (gPath.IsVisible(e.X, e.Y))
                {
                    selectedBody = b;
                    selectedBodyIndex = i;
                    canvas.Invalidate();
                    return;
                }
                i++;
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
            StaticParameters.pointDisplay = PointDisplayModes.Circle;
            pointToolStripMenuItem.Checked = true;
            crossToolStripMenuItem.Checked = false;
            canvas.Invalidate();
        }

        private void crossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaticParameters.pointDisplay = PointDisplayModes.Cross;
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

        private void graduationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            displayGrad = !displayGrad;
            canvas.Invalidate();
        }

        private void diffractionNYEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaticParameters.useDiffraction = !StaticParameters.useDiffraction;
            MessageBox.Show(
                "Turned diffraction " + ((StaticParameters.useDiffraction) ? "on." : "off."),
                "Info",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void rayNormalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaticParameters.showRayNormals = !StaticParameters.showRayNormals;
            canvas.Invalidate();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedLightRay)
            {
                scene.lightRays.RemoveAt(selectedLightRayIndex);
                selectedLightRay = LightRay.NONE;
                canvas.Invalidate();
            }
            if (selectedBody)
            {
                scene.bodies.RemoveAt(selectedBodyIndex);
                selectedBody = Body.NONE;
                canvas.Invalidate();
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StaticParameters.viewMode != ViewModes.Edit)
            {
                return;
            }
            scene.lightRays = new List<LightRay>();
            scene.bodies = new List<Body>();
            selectedBody = Body.NONE;
            selectedLightRay = LightRay.NONE;
            canvas.Invalidate();
        }

        private void bodiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StaticParameters.viewMode != ViewModes.Edit)
            {
                return;
            }
            scene.bodies = new List<Body>();
            selectedBody = Body.NONE;
            canvas.Invalidate();
        }

        private void raysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StaticParameters.viewMode != ViewModes.Edit)
            {
                return;
            }
            scene.lightRays = new List<LightRay>();
            selectedLightRay = LightRay.NONE;
            canvas.Invalidate();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedBody)
            {
                clipboardBody = selectedBody;
            }
            if (selectedLightRay)
            {
                ClipboardUdir = new Vector(selectedLightRay.rays[0].udir);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedBody)
            {
                clipboardBody = selectedBody;
                deleteToolStripMenuItem_Click(this, new EventArgs());
            }
            if (selectedLightRay)
            {
                ClipboardUdir = new Vector(selectedLightRay.rays[0].udir);
                deleteToolStripMenuItem_Click(this, new EventArgs());
            }
            canvas.Invalidate();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clipboardBody)
            {
                MessageBox.Show("Feature not implemented", "Pasting bodies");
                return;
            }
            if (ClipboardUdir.x != 0 && ClipboardUdir.y != 0)
            {
                var r = new LightRay(
                    new Ray(viewTransform.inverseTransform(canvas.PointToClient(Cursor.Position)),
                    ClipboardUdir
                ),
                
                scene.lightRays.Add(ClipboardUdir);
                selectedLightRay = scene.lightRays.Last();
                lightRayRotor = new PointRotor(selectedLightRay.rays[0].start);
                movingLray = true;
            }
            canvas.Invalidate();
        }
    }
}

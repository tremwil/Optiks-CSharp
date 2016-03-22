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
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            if (zoomEnd)
            {
                e.Graphics.SmoothingMode = SmoothingMode.None;
                e.Graphics.DrawImage(Image.FromFile("SNP.jpg"), e.Graphics.ClipBounds);
                return;
            }
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //Graph section
            if (displayAxes)
            {
                e.Graphics.SmoothingMode = SmoothingMode.None;

                var zoom = viewTransform.Elements[0];
                gridSize = zoom;

                var c = 1d;
                while (gridSize > maxGridSize)
                {
                    gridSize /= 10;
                    c /= 10;
                }
                while (gridSize < minGridSize)
                {
                    gridSize *= 10;
                    c *= 10;
                }

                var dx = viewTransform.Elements[4];
                var dy = viewTransform.Elements[5];
                var w = e.Graphics.ClipBounds.Width;
                var h = e.Graphics.ClipBounds.Height;

                for (float i = dx % gridSize; i < w; i += gridSize)
                {
                    Pen p = (i - dx < 1 && i - dx > -1) ? Pens.Black : new Pen(Color.FromArgb(160, Color.LightGray));
                    e.Graphics.DrawLine(p, i, 0, i, h);

                    var s = Math.Round((i - dx) / gridSize);
                    if (gridSize >= minTextGridSize && displayGrad)
                    {
                        e.Graphics.DrawString((s * c).ToString(), SystemFonts.DefaultFont, Brushes.Black, new PointF(i, dy));
                        e.Graphics.DrawLine(Pens.Black, i, dy - 4, i, dy + 4);
                    }
                    else if (displayGrad && s % 2 == 0)
                    {
                        e.Graphics.DrawString((s * c).ToString(), SystemFonts.DefaultFont, Brushes.Black, new PointF(i, dy));
                        e.Graphics.DrawLine(Pens.Black, i, dy - 4, i, dy + 4);
                    }
                }
                for (float i = dy % gridSize; i < h; i += gridSize)
                {
                    Pen p = (i - dy < 1 && i - dy > -1) ? Pens.Black : new Pen(Color.FromArgb(160, Color.LightGray));
                    e.Graphics.DrawLine(p, 0, i, w, i);

                    var s = Math.Round((i - dy) / gridSize);
                    if (gridSize >= minTextGridSize && displayGrad)
                    {
                        e.Graphics.DrawString((s * c).ToString(), SystemFonts.DefaultFont, Brushes.Black, new PointF(dx, i));
                        e.Graphics.DrawLine(Pens.Black, dx - 4, i, dx + 4, i);
                    }
                    else if (displayGrad && s % 2 == 0)
                    {
                        e.Graphics.DrawString((s * c).ToString(), SystemFonts.DefaultFont, Brushes.Black, new PointF(dx, i));
                        e.Graphics.DrawLine(Pens.Black, dx - 4, i, dx + 4, i);
                    }
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }

            scene.renderBodies(e.Graphics, viewTransform);
            scene.renderLightRays(e.Graphics, viewTransform);

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
    }
}

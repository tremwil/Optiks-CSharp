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
            Vector scaledMousePos = viewTransform.inverseTransform(canvas.PointToClient(Cursor.Position));

            if (zoomEnd)
            {
                e.Graphics.SmoothingMode = SmoothingMode.None;
                e.Graphics.DrawImage(Image.FromFile("SNP.jpg"), e.Graphics.ClipBounds);
                return;
            }
            e.Graphics.SmoothingMode = SmoothingMode.None;

            var zoom = viewTransform.Elements[0];

            //Graph section
            if (StaticParameters.displayGrid)
            {
                StaticParameters.gridSize = zoom;

                StaticParameters.sizeMultiplier = 1;
                while (StaticParameters.gridSize > maxGridSize)
                {
                    StaticParameters.gridSize /= 10;
                    StaticParameters.sizeMultiplier /= 10;
                }
                while (StaticParameters.gridSize < minGridSize)
                {
                    StaticParameters.gridSize *= 10;
                    StaticParameters.sizeMultiplier *= 10;
                }

                var dx = viewTransform.Elements[4];
                var dy = viewTransform.Elements[5];
                var w = e.Graphics.ClipBounds.Width;
                var h = e.Graphics.ClipBounds.Height;
                var axisPen = new Pen(Color.FromArgb(160, Color.LightGray));

                for (float i = dx % StaticParameters.gridSize; i < w; i += StaticParameters.gridSize)
                {
                    Pen p = (i - dx < 1 && i - dx > -1) ? Pens.Black : axisPen;
                    e.Graphics.DrawLine(p, i, 0, i, h);

                    int s = (int)Math.Round((i - dx) / StaticParameters.gridSize);
                    if (StaticParameters.gridSize >= minTextGridSize && displayGrad)
                    {
                        e.Graphics.DrawString(
                            (s * StaticParameters.sizeMultiplier).ToString(), 
                            SystemFonts.DefaultFont, 
                            Brushes.Black,
                            new PointF(i, dy)
                        );
                        //e.Graphics.DrawLine(Pens.Black, i, dy - 4, i, dy + 4);
                    }
                    else if (displayGrad && s % 2 == 0)
                    {
                        e.Graphics.DrawString(
                            (s * StaticParameters.sizeMultiplier).ToString(), 
                            SystemFonts.DefaultFont, 
                            Brushes.Black, 
                            new PointF(i, dy)
                        );
                        //e.Graphics.DrawLine(Pens.Black, i, dy - 4, i, dy + 4);
                    }
                }
                for (float i = dy % StaticParameters.gridSize; i < h; i += StaticParameters.gridSize)
                {
                    Pen p = (i - dy < 1 && i - dy > -1) ? Pens.Black : axisPen;
                    e.Graphics.DrawLine(p, 0, i, w, i);

                    int s = (int)Math.Round((i - dy) / StaticParameters.gridSize);
                    if (StaticParameters.gridSize >= minTextGridSize && displayGrad)
                    {
                        e.Graphics.DrawString(
                            (s * StaticParameters.sizeMultiplier).ToString(),
                            SystemFonts.DefaultFont, 
                            Brushes.Black, 
                            new PointF(dx, i)
                        );
                        //e.Graphics.DrawLine(Pens.Black, dx - 4, i, dx + 4, i);
                    }
                    else if (displayGrad && s % 2 == 0)
                    {
                        e.Graphics.DrawString(
                            (s * StaticParameters.sizeMultiplier).ToString(), 
                            SystemFonts.DefaultFont, 
                            Brushes.Black,
                            new PointF(dx, i)
                        );
                        //e.Graphics.DrawLine(Pens.Black, dx - 4, i, dx + 4, i);
                    }
                }

                axisPen.Dispose();
            }

            if (AAenabled) { e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; }

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

            if (StaticParameters.viewMode == ViewModes.GetInfo && StaticParameters.showRayNormals)
            {
                foreach (LightRay r in scene.lightRays)
                {
                    foreach (Ray ray in r.rays)
                    {
                        if (ray.collision)
                        {
                            if ((ray.collision.contactPoint - scaledMousePos).lenSqr() * zoom * zoom < 400)
                            {
                                var n = ray.collision.normal;
                                var nOrient = ((n * ray.udir < 0) ? n : -n);
                                var cp = viewTransform * ray.collision.contactPoint;

                                if (StaticParameters.showRayNormals)
                                {
                                    e.Graphics.DrawLine(Pens.Red, cp, cp + nOrient * 50);

                                    if (ray.collision.newUdir.lenSqr() != 0)
                                    {
                                        var u = ray.collision.newUdir;
                                        u = ((n * u < 0) ? n : -n);

                                        e.Graphics.DrawLine(Pens.Red, cp, cp - u * 50);
                                    }
                                }

                                if (StaticParameters.showSurfaceAngles)
                                {
                                    var sweepA = Math.Acos(-nOrient * ray.udir) * MathExt.DEGREES;
                                    var startA = ((Vector.cross(-ray.udir, nOrient) > 0) ?
                                        Math.PI - Math.Atan2(-ray.udir.y, ray.udir.x) :
                                        Math.PI - Math.Atan2(nOrient.y, -nOrient.x)) * MathExt.DEGREES;

                                    var rect = new Rectangle(
                                        (Point)(cp - new Vector(30, 30)),
                                        new Size(60, 60)
                                    );

                                    e.Graphics.DrawPie(Pens.Red, rect, (float)startA, (float)sweepA);

                                    if (ray.collision.newUdir.lenSqr() != 0)
                                    {
                                        var u = ray.collision.newUdir;
                                        nOrient = ((n * u < 0) ? n : -n);

                                        sweepA = Math.Acos(-nOrient * u) * MathExt.DEGREES;
                                        startA = ((Vector.cross(-u, nOrient) > 0) ?
                                            Math.PI - Math.Atan2(-u.y, u.x) :
                                            Math.PI - Math.Atan2(nOrient.y, -nOrient.x)) * MathExt.DEGREES;

                                        e.Graphics.DrawPie(Pens.Red, rect, (float)startA + 180, (float)sweepA);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Sides change color when you are focused
            Color col = canvas.Focused ? Color.Black : Color.LightGray;
            ControlPaint.DrawBorder(e.Graphics, canvas.ClientRectangle, col, ButtonBorderStyle.Solid);
        }
    }
}

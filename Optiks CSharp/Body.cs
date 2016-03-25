using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    [Flags]
    enum DrawTypes
    {
        None = 0x00,
        Draw = 0x01,
        Fill = 0x02
    }

    enum BodyTypes
    {
        Absorbing,
        Reflecting,
        Refracting,
    }

    struct DispersionCoefs
    {
        public const double SuperSmall = 200;
        public const double VerySmall = 90;
        public const double Small = 55;
        public const double Moderate = 45;
        public const double High = 35;
        public const double VeryHigh = 20;
        public const double SuperHigh = 10;

        // Real glass, from refractiveIndex.info
        public const double FKHTi = 70.47;
        public const double BK7 = 64.17;
        public const double LaSF35 = 29.06;
        public const double Diamond = 55.3;
    }

    struct RefractionIndices
    {
        public const double SuperSmall = 1.05;
        public const double VerySmall = 1.2;
        public const double Small = 1.4;
        public const double Moderate = 1.6;
        public const double High = 2;
        public const double VeryHigh = 3;
        public const double SuperHigh = 5;

        public const double Water = 1.33;
        public const double Glass = 1.5;
        public const double Diamond = 2.4469;
        public const double Silicon = 3.48;

        // Real glass, from refractiveIndex.info
        public const double FKHTi = 1.4793;
        public const double BK7 = 1.5075;
        public const double LaSF35 = 1.9884;
    }

    class Body
    {
        public List<Line> segments;
        public double refractionIndex;
        public double abbeNumber;

        public Pen pen;
        public SolidBrush brush;
        public GraphicsPath gpath;
        public RectangleF bounds;
        public Vector centerOfRotation;
        public DrawTypes drawMode;

        public BodyTypes type;

        private bool empty;

        public string name = "";

        public Body(List<Line> segments, double n, double abbe, BodyTypes type, Pen pen, SolidBrush brush, DrawTypes mode)
        {
            empty = false;
            drawMode = mode;
            refractionIndex = n;
            abbeNumber = abbe;
            this.segments = segments;

            this.type = type;
            this.pen = pen;
            this.brush = brush;

            recomputeGpath();
            computeCOR();
        }

        public Body(List<Line> segments, double n, BodyTypes type, Pen pen, SolidBrush brush, DrawTypes mode)
        {
            empty = false;
            drawMode = mode;
            refractionIndex = n;
            abbeNumber = DispersionCoefs.Moderate;
            this.segments = segments;

            this.type = type;
            this.pen = pen;
            this.brush = brush;

            recomputeGpath();
            computeCOR();
        }

        public Body()
        {
            empty = true;
        }

        public void computeCOR()
        {
            centerOfRotation = new Vector(0, 0);

            foreach (Line l in segments)
            {
                centerOfRotation += l.start;
            }

            centerOfRotation /= segments.Count;
        }

        public void recomputeGpath()
        {
            gpath = new GraphicsPath();
            gpath.StartFigure();

            foreach (Line l in segments)
            {
                if (l.type == LineTypes.Straight)
                {
                    gpath.AddLine(l.start, l.end);
                }
                if (l.type == LineTypes.CircleArc)
                {
                    var v = l.center - new Vector(l.radius, l.radius);
                    var rect = new RectangleF(v, new SizeF((float)l.radius * 2, (float)l.radius * 2));
                    gpath.AddArc(rect, (float)l.startAngle, (float)l.sweepAngle);
                }
                if (l.type == LineTypes.Parabolic)
                {
                    // Convert quad Bezier to cubic because GDI+ doesn't support quad
                    Vector CP1 = l.start + 2d / 3 * (l.bezierHandle - l.start);
                    Vector CP2 = l.end + 2d / 3 * (l.bezierHandle - l.end);

                    gpath.AddBezier(l.start, CP1, CP2, l.end);
                }
                if (l.type == LineTypes.Hyperbolic)
                {
                    List<PointF> spline = new List<PointF>();
                    for (var i = 0; i <= 100; i++)
                    {
                        spline.Add(MathExt.evalWBezier(l, 0.01 * i));
                    }
                    gpath.AddLines(spline.ToArray());
                }
            }

            gpath.CloseFigure();
            bounds = gpath.GetBounds();
        }

        public void render(Graphics g, Matrix transform)
        {
            var newPath = (GraphicsPath)gpath.Clone();
            newPath.Transform(transform);

            if (drawMode.HasFlag(DrawTypes.Draw))
            {
                g.DrawPath(pen, newPath);
            }

            if (drawMode.HasFlag(DrawTypes.Fill))
            {
                g.FillPath(brush, newPath);
            }
            if (StaticParameters.viewMode == ViewModes.Edit)
            {
                Vector v = transform * centerOfRotation;
                v.render(4, Brushes.Black, g);
                v.render(3, Brushes.GreenYellow, g);
            }
        }

        public static implicit operator bool (Body b)
        {
            return !b.empty;
        }

        public static Body NONE = new Body();

        public Body Clone()
        {
            return new Body(
                segments.Select(x => x.ShallowCopy()).ToList(),
                refractionIndex,
                abbeNumber,
                type,
                new Pen(pen.Brush, pen.Width),
                new SolidBrush(brush.Color),
                drawMode
            );
        }
    }
}

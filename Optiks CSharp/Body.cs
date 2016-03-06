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
        Draw = 0x01,
        Fill = 0x02
    }

    enum BodyTypes
    {
        Absorbing,
        Reflecting,
        Refracting
    }

    class Body
    {
        public List<Line> segments;
        public double refractionIndex;

        public Pen pen;
        public SolidBrush brush;
        public GraphicsPath gpath;
        public DrawTypes drawMode;

        public BodyTypes type;

        private bool empty;

        public Body(List<Line> segments, double n, BodyTypes type, Pen pen, SolidBrush brush, DrawTypes mode)
        {
            empty = false;
            drawMode = mode;
            refractionIndex = n;
            this.segments = segments;

            this.type = type;
            this.pen = pen;
            this.brush = brush;

            gpath = new GraphicsPath();
            gpath.StartFigure();

            foreach(Line l in segments)
            {
                if (l.type == LineTypes.Straight)
                {
                    gpath.AddLine(l.start, l.end);
                }

                else
                {
                    var v = l.focalPoint - new Vector(l.radius, l.radius);
                    var rect = new RectangleF(v, new SizeF((float)l.radius * 2, (float)l.radius * 2));
                    gpath.AddArc(rect, (float)l.startAngle, (float)l.sweepAngle);
                }
            }

            gpath.CloseFigure();
        }

        public Body()
        {
            empty = true;
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
        }

        public static implicit operator bool(Body b)
        {
            return !b.empty;
        }

        public static Body NONE = new Body();
    }
}

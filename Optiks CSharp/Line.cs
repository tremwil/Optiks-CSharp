using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optiks_CSharp
{
    enum LineTypes
    {
        Straight,
        Curved
    }

    abstract class Line
    {
        public LineTypes type;

        public Vector start;
        public Vector end;
        public Vector tangent;
        public Vector normal;

        public double arcHeight;
        public double arcWidth;
        public double radius;
        public double pointCW;

        public double startAngle;
        public double sweepAngle;

        public Vector focalPoint;

        public abstract Vector norm(Vector contactPoint);
    }

    class Segment: Line
    {
        public Segment(Vector start, Vector end)
        {
            this.type = LineTypes.Straight;

            this.start = start;
            this.end = end;
            this.tangent = end - start;
            this.normal = tangent.normal().unit();
        }

        public override Vector norm(Vector contactPoint)
        {
            return this.normal;
        }
    }

    class Curve: Line
    {
        public Curve(Vector start, Vector end, double signedHeight)
        {
            this.type = LineTypes.Curved;

            this.start = start;
            this.end = end;
            this.tangent = end - start;
            this.normal = tangent.normal().unit();

            this.pointCW = Math.Sign(signedHeight);

            this.arcHeight = Math.Abs(signedHeight);
            this.arcWidth = tangent.len();
            this.radius = Math.Pow(arcWidth, 2) / (8 * arcHeight) + arcHeight / 2;
            this.focalPoint = this.start + tangent / 2 + (radius - arcHeight) * -pointCW * normal;

            var delta = this.focalPoint - this.start;
            this.startAngle = 180 - MathExt.DEGREES * Math.Atan2(-delta.y, delta.x);

            delta = this.focalPoint - this.end;
            var endAngle = 180 - MathExt.DEGREES * Math.Atan2(-delta.y, delta.x);

            this.sweepAngle = endAngle - startAngle;

            if ((radius < arcHeight && Math.Abs(sweepAngle) < 180) ||
                (radius > arcHeight && Math.Abs(sweepAngle) > 180) ||
                (MathExt.diff(sweepAngle, 180) < MathExt.EPSILON && pointCW > 0))
            {
                this.sweepAngle = -Math.Sign(sweepAngle) * (360 - Math.Abs(sweepAngle));
            }
        }

        public override Vector norm(Vector contactPoint)
        {
            var delta = contactPoint - focalPoint;
            return delta.unit() * pointCW;
        }
    }
}

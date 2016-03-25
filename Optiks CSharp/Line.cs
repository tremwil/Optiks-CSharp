using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace Optiks_CSharp
{
    enum LineTypes
    {
        Straight,
        CircleArc,
        Parabolic,
        Hyperbolic
    }

    abstract class Line
    {
        public LineTypes type;

        public Vector start;
        public Vector end;
        public Vector tangent;
        public Vector normal;

        public double height;
        public double width;
        public double radius;
        public double pointCW;
        public double startAngle;
        public double sweepAngle;
        public Vector center;

        public Vector focalPoint;
        public Vector vertex;
        public Vector bezierHandle;

        public double weight;
        public double a;
        public double b;
        public double c;
        public double e;

        public abstract Vector norm(Vector contactPoint);

        public Line ShallowCopy()
        {
            return (Line)MemberwiseClone();
        }
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

            // For UI
            this.vertex = start + tangent / 2;
        }

        public override Vector norm(Vector contactPoint)
        {
            return this.normal;
        }
    }

    class CircleArc: Line
    {
        public CircleArc(Vector start, Vector end, double signedHeight)
        {
            this.type = LineTypes.CircleArc;

            this.start = start;
            this.end = end;
            this.tangent = end - start;
            this.normal = tangent.normal().unit();

            this.pointCW = Math.Sign(signedHeight);

            this.height = Math.Abs(signedHeight);
            this.width = tangent.len();
            this.radius = Math.Pow(width, 2) / (8 * height) + height / 2;
            this.center = this.start + tangent / 2 + (radius - height) * -pointCW * normal;

            // For rendering & Approximate focal point
            this.focalPoint = center + pointCW * radius / 2 * normal;
            this.vertex = center + pointCW * radius * normal;

            /* Angle dipshit starts here - MIND = BLOWN ALERT */
            var delta = this.center - this.start;
            this.startAngle = 180 - MathExt.DEGREES * Math.Atan2(-delta.y, delta.x);

            delta = this.center - this.end;
            var endAngle = 180 - MathExt.DEGREES * Math.Atan2(-delta.y, delta.x);

            this.sweepAngle = endAngle - startAngle;
            var testVec = radius * ((sweepAngle > 0) ? normal : -normal);

            if (normal * pointCW * testVec > 0)
            {
                this.sweepAngle = -Math.Sign(sweepAngle) * (360 - Math.Abs(sweepAngle));
            }
        }

        public override Vector norm(Vector contactPoint)
        {
            var delta = contactPoint - center;
            return delta.unit() * pointCW;
        }
    }

    class ParabolicBezier : Line
    {
        public ParabolicBezier(Vector start, Vector end, double signedHeight)
        {
            type = LineTypes.Parabolic;

            this.start = start;
            this.end = end;
            tangent = end - start;
            normal = tangent.normal().unit();

            pointCW = Math.Sign(signedHeight);
            height = Math.Abs(signedHeight);
            width = tangent.len();

            vertex = start + tangent / 2 + normal * signedHeight;
            focalPoint = vertex + Math.Pow(width * 0.5, 2) / (-4 * signedHeight) * normal;
            bezierHandle = 2 * vertex - start * 0.5 - end * 0.5;

            weight = 1;
            e = 1;
        }

        public static ParabolicBezier fromFoci(Vector start, Vector end, double signedFoci)
        {
            return new ParabolicBezier(start, end, (start - end).lenSqr() / (signedFoci * 16));
        }

        public override Vector norm(Vector contactPoint)
        {
            Vector I = -pointCW * normal;
            Vector R = (focalPoint - contactPoint).unit();
            return ((R + I) * -0.5).unit() * pointCW;
        }
    }

    class HyperbolicSurface: Line
    {
        public HyperbolicSurface(Vector start, Vector end, double signedHeight, double eccentricity)
        {
            type = LineTypes.Hyperbolic;

            if (eccentricity < 1 + MathExt.EPSILON)
            {
                throw new ArgumentException("Eccentricity must be bigger than 1 + EPSILON");
            }

            this.start = start;
            this.end = end;
            tangent = end - start;
            normal = tangent.normal().unit();

            pointCW = Math.Sign(signedHeight);
            height = Math.Abs(signedHeight);
            width = tangent.len();

            e = eccentricity;

            var e2x4 = 4 * eccentricity * eccentricity;
            var t2 = width * width;
            var h2 = height * height;
            var w = ((e2x4 - 4) * h2 + t2) / ((4 - e2x4) * h2 + t2);
            Vector M = start + tangent / 2;

            weight = w;
            vertex = M + normal * signedHeight;
            bezierHandle = M + normal * signedHeight * (w + 1) / w;

            var wp1 = (w + 1); var wm1 = (w - 1);
            a = height / wm1;
            c = 0.5 * Math.Sqrt((4 * h2 * wp1 + t2 * wm1) / (wm1 * wm1 * wp1));
            b = Math.Sqrt(c * c - a * a);

            center = vertex + normal * pointCW * a;
            focalPoint = center + normal * pointCW * c;
        }

        public static HyperbolicSurface fromFoci(Vector start, Vector end, double signedFoci, double e)
        {
            var t = (start - end).len();
            var t2 = t * t;
            var f = Math.Abs(signedFoci);
            var e2 = e * e;
            var efx8 = (e - 1) * f * 8;

            var eqVal = 0.5 * t / Math.Sqrt(e2 - 1) - 0.0005 * t;
            if (f <= eqVal)
            {
                return new HyperbolicSurface(start, end, Math.Sign(signedFoci) * eqVal, e);
            }

            var sqrt = Math.Sqrt(4 * (4 * e2 - 8 * e + 4) * t2 + efx8 * efx8);
            var H = 0.125 * (sqrt - efx8) / (e2 - 2 * e + 1) * Math.Sign(signedFoci);

            return new HyperbolicSurface(start, end, H, e);
        }

        public override Vector norm(Vector contactPoint)
        {
            var sina = -normal.y;
            var cosa = normal.x;

            var tCP = new Vector(
                (contactPoint.y - center.y) * sina - (contactPoint.x - center.x) * cosa,
                (contactPoint.x - center.x) * sina + (contactPoint.y - center.y) * cosa
            );

            var norm = new Vector(tCP.x / (a * a), tCP.y / (b * b)).unit();

            return new Vector(
                norm.x * cosa + norm.y * sina,
                norm.y * cosa - norm.x * sina
            ) * pointCW;
        }
    }
}

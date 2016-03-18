using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    enum LineTypes
    {
        Straight,
        CircleArc,
        Parabolic,
        Conic
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
        }

        public override Vector norm(Vector contactPoint)
        {
            Vector I = -pointCW * normal;
            Vector R = (focalPoint - contactPoint).unit();
            return ((R + I) * -0.5).unit();
        }
    }

    /* WIP Hyperbolic lens */
    //class ConicSurface : Line
    //{
    //    public ConicSurface(Vector start, Vector end, double signedHeight, double e)
    //    {
    //        type = LineTypes.Conic;

    //        this.start = start;
    //        this.end = end;
    //        tangent = end - start;
    //        normal = tangent.normal().unit();

    //        pointCW = Math.Sign(signedHeight);
    //        height = Math.Abs(signedHeight);
    //        width = tangent.len();

    //        vertex = start + tangent / 2 + normal * signedHeight;
    //        bezierHandle = 2 * vertex - start * 0.5 - end * 0.5;

    //    }

    //    public override Vector norm(Vector contactPoint)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

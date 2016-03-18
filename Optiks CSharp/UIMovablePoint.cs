using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    static class MatrixExtensions
    {
        /// <summary>
        /// Inverse-tranform a vector without fully inverting the matrix.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector inverseTransform(this Matrix t, Vector p)
        {
            /* 
            Algebraic solving:
                P' = (a*x + b*x + e, c*y + d*y + f)
                x' = a*x + b*x + e
                x' = (a + b)*x + e
                x' - e = (a + b)*x
                (x' - e) / (a + b) = x
            */

            var a = t.Elements[0];
            var b = t.Elements[1];
            var c = t.Elements[2];
            var d = t.Elements[3];
            var e = t.Elements[4];
            var f = t.Elements[5];

            if (a + b == 0 || c + d == 0)
            {
                throw new DivideByZeroException();
            }

            return new Vector((p.x - e) / (a + b), (p.y - f) / (c + d));
        }
    }

    enum MovablePointTypes
    {
        Translator,
        Rotor
    }

    abstract class MovablePoint
    {
        public Vector rotationCenter;
        public Vector unitGuidanceVector;

        public MovablePointTypes type;

        public abstract void applyMovement(ref Vector point, Vector mousePos, Matrix t, Vector[] locks, double lockThreshold);
        public abstract void display(Vector point, Graphics g, Matrix t);
        public abstract void setUnitVector(ref Vector unitp, Vector mousePos, Matrix t, Vector[] locks, double lockThreshold);
    }

    class PointTranslator : MovablePoint
    {
        public PointTranslator(Vector unitGuidance)
        {
            unitGuidanceVector = unitGuidance;
            type = MovablePointTypes.Translator;
        }

        public override void applyMovement(ref Vector point, Vector mousePos, Matrix t, Vector[] locks, double lockThreshold)
        {
            Vector scaledMousePos = t.inverseTransform(mousePos);

            Vector closest = new Vector(0, 0);
            double smallestDist = double.PositiveInfinity;

            foreach (Vector clip in locks)
            {
                var dist = (scaledMousePos - clip).lenSqr();
                if (dist < smallestDist) { smallestDist = dist; closest = clip; }
            }

            if (smallestDist * t.Elements[0] <= 25)
            {
                scaledMousePos = closest;
            }

            // P' = P + (S - P) . U * U where . is the dot product
            point += (scaledMousePos - point) * unitGuidanceVector * unitGuidanceVector;
        }

        public override void display(Vector point, Graphics g, Matrix t)
        {
                
        }

        public override void setUnitVector(ref Vector unitp, Vector mousePos, Matrix t, Vector[] locks, double lockThreshold)
        {

        }
    }

    class PointRotor : MovablePoint
    {
        public PointRotor(Vector rc)
        {
            rotationCenter = rc;
            type = MovablePointTypes.Rotor;
        }

        public override void applyMovement(ref Vector point, Vector mousePos, Matrix t, Vector[] locks, double lockThreshold)
        {
            var scaledMousePos = t.inverseTransform(mousePos);

            Vector closest = new Vector(0, 0);
            double smallestDist = double.PositiveInfinity;

            foreach (Vector clip in locks)
            {
                var dist = (scaledMousePos - clip).lenSqr();
                if (dist < smallestDist) { smallestDist = dist; closest = clip; }
            }

            if (smallestDist * t.Elements[0] <= 25)
            {
                scaledMousePos = closest;
            }

            point = rotationCenter + point.lenSqr() * (scaledMousePos - rotationCenter).unit();
        }

        public override void setUnitVector(ref Vector unitp, Vector mousePos, Matrix t, Vector[] locks, double lockThreshold)
        {
            var scaledMousePos = t.inverseTransform(mousePos);

            Vector closest = new Vector(0, 0);
            double smallestDist = double.PositiveInfinity;

            foreach (Vector clip in locks)
            {
                var dist = (scaledMousePos - clip).lenSqr();
                if (dist < smallestDist) { smallestDist = dist; closest = clip; }
            }

            if (smallestDist * t.Elements[0] * t.Elements[0] <= 25)
            {
                scaledMousePos = closest;
            }

            unitp = (scaledMousePos - rotationCenter).unit();
        }

        public override void display(Vector upoint, Graphics g, Matrix t)
        {
            var tCenter = t * rotationCenter;
            var angle = (float)(180 - MathExt.DEGREES * Math.Atan2(upoint.y, -upoint.x));
            var radiusV = new Vector(80, 80);

            g.DrawArc(Pens.OrangeRed, new RectangleF(tCenter - radiusV, radiusV * 2), angle, 360 - angle);
            g.DrawPie(Pens.LawnGreen, new RectangleF(tCenter - radiusV, radiusV * 2), 0, angle);
            g.DrawString(Math.Round(angle, 2).ToString(), SystemFonts.DefaultFont, Brushes.Black, tCenter + new Vector(-5, 8));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    namespace UI
    {

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
                t = t.Clone();
                t.Invert();

                Vector scaledMousePos = t * mousePos;

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
                t = t.Clone();
                t.Invert();
            }

            public override void display(Vector point, Graphics g, Matrix t)
            {
                var tPoint = t * point;
                var tCenter = t * rotationCenter;
                var radius = (tPoint - tCenter).len();
                var radiusV = new Vector(radius, radius);

                g.DrawEllipse(Pens.OrangeRed, new RectangleF(tCenter - radiusV, radiusV * 2));
            }
        }
    }
}

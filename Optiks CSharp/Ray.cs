using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    class RayCollisionInfo
    {
        public double distance;
        public Vector contactPoint;
        public Vector normal;
        public Line segment;
        public Body body;
        public Body secondBody;

        public bool empty;

        public RayCollisionInfo(double distance, Vector contact, Vector norm, Line seg)
        {
            empty = false;
            this.distance = distance;
            this.contactPoint = contact;
            this.normal = norm;
            this.segment = seg;
        }

        public RayCollisionInfo()
        {
            empty = true;
            distance = double.PositiveInfinity;
        }

        public static implicit operator bool(RayCollisionInfo r)
        {
            return !r.empty;
        }

        public static RayCollisionInfo EMPTY = new RayCollisionInfo();
    }

    class Ray
    {
        public Vector start;
        public Vector udir;
        public Vector unorm;
        public RayCollisionInfo collision;

        public Ray(Vector start, Vector udir)
        {
            this.start = start;
            this.udir = udir;
            this.unorm = udir.normal();
            this.collision = RayCollisionInfo.EMPTY;
        }

        public RayCollisionInfo segmentIntersect(Line seg)
        {
            var v1 = seg.start - this.start;
            var v2 = seg.end - this.start;
            var t1 = v1 * this.udir;
            var t2 = v2 * this.udir;

            if (t1 <= 0 && t2 <= 0)
            {
                return RayCollisionInfo.EMPTY; //already start eliminating for moar speed
            }
            var n1 = v1 * this.unorm;
            var n2 = v2 * this.unorm;

            if (n1 * n2 > 0)
            {
                return RayCollisionInfo.EMPTY;
            }
            if (Math.Abs(n2 - n1) <= MathExt.EPSILON)
            {
                return RayCollisionInfo.EMPTY;
            }
            var a = (t2 - t1) / (n2 - n1);
            var b = t1 - n1 * a;

            if (b <= MathExt.EPSILON) 
            {
                return RayCollisionInfo.EMPTY;
            }
            return new RayCollisionInfo(b, this.start + (b * this.udir), seg.normal, seg);
        }

        public RayCollisionInfo curveIntersect(Line curve)
        {
            var delta = start - curve.focalPoint;
            var deltaLenSqr = delta.lenSqr();

            var rootSqr = Math.Pow(udir * delta, 2) - deltaLenSqr + Math.Pow(curve.radius, 2); 

            if (rootSqr <= MathExt.EPSILON)
            {
                return RayCollisionInfo.EMPTY;
            }

            var root = Math.Sqrt(rootSqr);
            var d1 = (-udir * delta + root);
            var d2 = (-udir * delta - root);

            double[] dists = new double[] {
                Math.Min(d1, d2),
                Math.Max(d1, d2)
            };

            foreach (double dist in dists)
            {
                var contactPoint = start + dist * udir;

                if ((contactPoint - curve.start) * curve.normal * curve.pointCW >= MathExt.EPSILON && dist >= MathExt.EPSILON)
                {
                    return new RayCollisionInfo(dist, contactPoint, curve.norm(contactPoint), curve);
                }
            }

            return RayCollisionInfo.EMPTY;
        }

        public RayCollisionInfo bodyListIntersect(List<Body> bodies)
        {
            collision = RayCollisionInfo.EMPTY; // Also store result in object
            foreach (Body body in bodies)
            {
                foreach (Line line in body.segments)
                {
                    RayCollisionInfo r = (line.type == LineTypes.Straight) ? 
                        this.segmentIntersect(line) : this.curveIntersect(line);
                    
                    if (!r) { continue; }

                    if (collision)
                    {
                        if (collision.body != body)
                        {
                            if (MathExt.diff(collision.distance, r.distance) < MathExt.EPSILON) {
                                var b1dir = udir * collision.normal;
                                var b2dir = udir * r.normal;

                                if (b1dir > 0 && b2dir < 0)
                                {
                                    Body body2 = collision.body;
                                    collision = r;
                                    collision.body = body;
                                    collision.secondBody = body2;
                                }

                                if (b2dir > 0 && b1dir < 0)
                                {
                                    collision.secondBody = body;
                                }
                            }
                        }
                    }

                    if (collision.distance - r.distance >= MathExt.EPSILON)
                    {
                        collision = r;
                        collision.body = body;
                    }
                }
            }

            return collision;
        }

        public void render(Pen p, Graphics g, Matrix transform)
        {
            Vector endPoint;

            if (!collision)
            {
                // Make ray endpoint dynamic so you can never reach it
                var sL = new PointF[] { start }; transform.TransformPoints(sL); var nstart = sL[0];
                Vector offset = new Vector(nstart) - start;
                double translate = udir * offset;

                endPoint = start + udir * ((g.ClipBounds.Height + g.ClipBounds.Width) / transform.Elements[0] - translate);

            }
            else
            {
                endPoint = collision.contactPoint;
            }
            
            var points = new PointF[] { start, endPoint };
            transform.TransformPoints(points);

            g.DrawLine(p, points[0], points[1]);
        }
    }
}
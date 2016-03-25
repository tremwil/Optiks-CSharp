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
        public Vector newUdir = new Vector(0, 0);
        public Line segment;
        public Body body = Body.NONE;
        public Body secondBody = Body.NONE;

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

        public static RayCollisionInfo NONE = new RayCollisionInfo();
    }

    class Ray
    {
        public Vector start;
        public Vector udir;
        public RayCollisionInfo collision;

        public Ray(Vector start, Vector udir)
        {
            this.start = start;
            this.udir = udir;
            this.collision = RayCollisionInfo.NONE;
        }

        public RayCollisionInfo segmentIntersect(Line seg)
        {
            var unorm = udir.normal();

            var v1 = seg.start - this.start;
            var v2 = seg.end - this.start;
            var t1 = v1 * this.udir;
            var t2 = v2 * this.udir;

            if (t1 <= 0 && t2 <= 0)
            {
                return RayCollisionInfo.NONE; //already start eliminating for moar speed
            }
            var n1 = v1 * unorm;
            var n2 = v2 * unorm;

            if (n1 * n2 > 0)
            {
                return RayCollisionInfo.NONE;
            }
            if (Math.Abs(n2 - n1) <= MathExt.EPSILON)
            {
                return RayCollisionInfo.NONE;
            }
            var a = (t2 - t1) / (n2 - n1);
            var b = t1 - n1 * a;

            if (b <= MathExt.EPSILON) 
            {
                return RayCollisionInfo.NONE;
            }
            return new RayCollisionInfo(b, this.start + (b * this.udir), seg.normal, seg);
        }

        public RayCollisionInfo circleArcIntersect(Line curve)
        {
            var delta = start - curve.center;
            var deltaLenSqr = delta.lenSqr();

            var rootSqr = Math.Pow(udir * delta, 2) - deltaLenSqr + Math.Pow(curve.radius, 2); 

            if (rootSqr <= MathExt.EPSILON)
            {
                return RayCollisionInfo.NONE;
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

            return RayCollisionInfo.NONE;
        }

        public double[] getBezierY(Vector[] points)
        {
            var tx = start.x;
            var ty = start.y;
            var sin = -udir.y; // Cross product of udir and (1, 0)
            var cos = udir.x; // Dot product of udir and (1, 0)

            return points.Select(v => (v.x - tx) * sin + (v.y - ty) * cos).ToArray();
        }

        public RayCollisionInfo parabolaIntersect(Line para)
        {
            var points = getBezierY(new Vector[]
                { para.start, para.bezierHandle, para.end }
            );

            var a = points[0];
            var b = points[1];
            var c = points[2];
            var d = a - 2 * b + c;
            double[] dists;

            if (Math.Abs(d) >= MathExt.EPSILON)
            { // Two intersection points - real or complex
                var sqr = b * b - a * c;
                if (sqr <= 0) { return RayCollisionInfo.NONE; }

                var sqrt = Math.Sqrt(sqr);
                var m = a - b;
                dists = new double[] { (m + sqrt) / d, (m - sqrt) / d };
            }
            else if (a != b)
            { // Ray perpendicular to para.normal (one real intersection point)
                dists = new double[] { a * 0.5 / (a - b) };
            }
            else
            { // Bezier curve is a single point - no intersect
                return RayCollisionInfo.NONE;
            }

            Vector best = new Vector(0, 0);
            double bestT = double.PositiveInfinity;

            foreach (double t in dists)
            {
                // point not on curve
                if (t < MathExt.EPSILON || t > 1 - MathExt.EPSILON) { continue; }

                // Compute actual intersection point & distance
                Vector cp = MathExt.evalBezier(para, t);
                var lineT = udir * (cp - start);

                // point on other side of the ray
                if (lineT < MathExt.EPSILON) { continue; }

                if (lineT < bestT)
                {
                    bestT = lineT;
                    best = cp;
                }
            }

            if (bestT != double.PositiveInfinity)
            {
                return new RayCollisionInfo(bestT, best, para.norm(best), para);
            }

            return RayCollisionInfo.NONE;
        }

        public RayCollisionInfo conicIntersect(Line conic)
        {
            if (conic.weight == 1)
            {
                return parabolaIntersect(conic);
            }

            var points = getBezierY(new Vector[]
                { conic.start, conic.bezierHandle, conic.end }
            );

            var w = conic.weight;
            var a = points[0];
            var b = points[1];
            var c = points[2];
            var d = a - 2 * b * w + c;
            double[] dists;

            if (Math.Abs(d) >= MathExt.EPSILON)
            { // Two intersection points - real or complex
                var sqr = b * b * w * w - a * c;
                if (sqr <= 0) { return RayCollisionInfo.NONE; }

                var sqrt = Math.Sqrt(sqr);
                var m = a - b * w;
                dists = new double[] { (m + sqrt) / d, (m - sqrt) / d };
            }
            else if (a != b)
            { // Ray is stewpid (1 intersect)
                dists = new double[] { a * 0.5 / (a - b * w) };
            }
            else
            { // Bezier curve is a single point - no intersect
                return RayCollisionInfo.NONE;
            }

            Vector best = new Vector(0, 0);
            double bestT = double.PositiveInfinity;

            foreach (double t in dists)
            {
                // point not on curve
                if (t < MathExt.EPSILON || t > 1 - MathExt.EPSILON) { continue; }

                // Compute actual intersection point & distance
                Vector cp = MathExt.evalWBezier(conic, t);
                var lineT = udir * (cp - start);

                // point on other side of the ray
                if (lineT < MathExt.EPSILON) { continue; }

                if (lineT < bestT)
                {
                    bestT = lineT;
                    best = cp;
                }
            }

            if (bestT != double.PositiveInfinity)
            {
                return new RayCollisionInfo(bestT, best, conic.norm(best), conic);
            }

            return RayCollisionInfo.NONE;
        }

        public RayCollisionInfo bodyListIntersect(List<Body> bodies)
        {
            collision = RayCollisionInfo.NONE; // Also store result in object
            foreach (Body body in bodies)
            {
                foreach (Line line in body.segments)
                {
                    var r = RayCollisionInfo.NONE;
                    switch (line.type)
                    {
                        case LineTypes.Straight:
                            r = segmentIntersect(line);
                            break;

                        case LineTypes.CircleArc:
                            r = circleArcIntersect(line);
                            break;

                        case LineTypes.Parabolic:
                            r = parabolaIntersect(line);
                            break;

                        case LineTypes.Hyperbolic:
                            r = conicIntersect(line);
                            break;
                    }  
                    
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
            var nstart = transform * start;

            if (!collision)
            {
                // Make ray endpoint dynamic so you can never reach it
                var r = new Ray(nstart, udir);

                var bounds = new List<Line> {
                    new Segment(g.ClipBounds.Location, new Vector(g.ClipBounds.Width, 0)),
                    new Segment(new Vector(g.ClipBounds.Width, 0), g.ClipBounds.Size),
                    new Segment(g.ClipBounds.Size, new Vector(0, g.ClipBounds.Width)),
                    new Segment(new Vector(0, g.ClipBounds.Width), g.ClipBounds.Location)
                };
                // Compute intersect with bounds
                var intersects = bounds.Select(x => r.segmentIntersect(x));
                var maxdist = intersects.Max(x => (x) ? (x.distance == double.PositiveInfinity) ? 0 : x.distance : 0);
                endPoint = nstart + udir * maxdist;
            }
            else
            {
                endPoint = transform * collision.contactPoint;
            }
            g.DrawLine(p, nstart, endPoint);
        }
    }
}
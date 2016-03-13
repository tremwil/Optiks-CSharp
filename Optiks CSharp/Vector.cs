using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    class Vector
    {
        private double X;
        private double Y;

        public double x { get { return this.X; } }
        public double y { get { return this.Y; } }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(PointF p)
        {
            X = p.X;
            Y = p.Y;
        }

        public Vector(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public static Vector fromAngle(double radians, double len)
        {
            return new Vector(Math.Cos(radians) * len, Math.Sin(radians) * len);
        }

        public double lenSqr()
        {
            return X * X + Y * Y;
        }

        public double len()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public Vector unit()
        {
            var l = len();
            return new Vector(X / l, Y / l);
        }

        public Vector normal()
        { // Clockwise normal
            return new Vector(-Y, X);
        }

        public static Vector fromAngle(double radians)
        {
            return new Vector(Math.Cos(radians), Math.Sin(radians));
        }

        public void render(float w, Brush b, Graphics g)
        {
            var tx = (float)x;
            var ty = (float)y;
            g.FillEllipse(b, tx - w, ty - w, 2 * w, 2 * w);
        }

        public static Vector operator -(Vector self)
        {
            return new Vector(-self.X, -self.Y);
        }

        public static Vector operator +(Vector A, Vector B)
        {
            return new Vector(A.X + B.X, A.Y + B.Y);
        }

        public static Vector operator -(Vector A, Vector B)
        {
            return new Vector(A.X - B.X, A.Y - B.Y);
        }

        public static double operator *(Vector A, Vector B)
        { // Dot product of A and B
            return A.X * B.X + A.Y * B.Y;
        }

        public static Vector operator *(double A, Vector B)
        {
            return new Vector(B.X * A, B.Y * A);
        }

        public static Vector operator *(Vector A, double B)
        {
            return new Vector(A.X * B, A.Y * B);
        }

        public static Vector operator *(Matrix A, Vector B)
        {
            return new Vector(
                B.X * A.Elements[0] + B.X * A.Elements[1] + A.Elements[4],
                B.Y * A.Elements[2] + B.Y * A.Elements[3] + A.Elements[5]
            );
        }

        public static Vector cross(Vector A, double B)
        {
            return new Vector(B * A.Y, -B * A.X);
        }

        public static Vector cross(double A, Vector B)
        {
            return new Vector(-A * B.Y, A * B.X);
        }

        public static double cross(Vector A, Vector B)
        {
            return A.X * B.Y - A.Y * B.X;
        }

        public static Vector operator /(Vector A, double B)
        {
            return new Vector(A.X / B, A.Y / B);
        }

        public static explicit operator Point(Vector A)
        {
            return new Point((int)A.X, (int)A.Y);
        }

        public static implicit operator PointF(Vector A)
        {
            return new PointF((float)A.X, (float)A.Y);
        }

        public static explicit operator Size(Vector A)
        {
            return new Size((int)A.X, (int)A.Y);
        }

        public static implicit operator SizeF(Vector A)
        {
            return new SizeF((float)A.X, (float)A.Y);
        }

        public static implicit operator Vector(Point A)
        {
            return new Vector(A);
        }

        public static implicit operator Vector(PointF A)
        {
            return new Vector(A);
        }

        public override string ToString()
        {
            return "{X: " + X.ToString() + ", Y: " + Y.ToString() + "}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    static class MathExt
    {
        public const double EPSILON = 10e-12;

        public const double TAU = 2 * Math.PI;
        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        public const double DEGREES = 180 / Math.PI;
        /// <summary>
        /// converts degrees to radians.
        /// </summary>
        public const double RADIANS = Math.PI / 180;

        public static double diff(double a, double b)
        {
            return Math.Abs(a - b);
        }

        /// <summary>
        /// Calculates the solutions of a quadratic equation.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns>The solution(s) of ax^2 + bx + c = 0</returns>
        public static double[] quad(double a, double b, double c)
        {
            var sqrt = Math.Sqrt(b * b - 4 * a * c);
            return new double[] { (-b + sqrt) / (2 * a), (-b - sqrt) / (2 * a) };
        }

        /// <summary>
        /// Calculates the angle between a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double vcrs(Vector a, Vector b)
        {
            return Math.Acos(a * b);
        }

        public static Vector evalBezier(Line para, double t)
        {
            var tm1 = 1 - t;
            return
                tm1 * tm1 * para.start +
                2 * tm1 * t * para.bezierHandle +
                t * t * para.end;
        }

        public static Vector evalWBezier(Line para, double t)
        {
            var tm1 = 1 - t;
            var a = tm1 * tm1;
            var b = 2 * tm1 * t * para.weight;
            var c = t * t;

            return (a * para.start + b * para.bezierHandle + c * para.end) / (a + b + c);
        }

        public static Vector cross(this Vector A, double B)
        {
            return new Vector(B * A.y, -B * A.x);
        }

        public static Vector cross(this double A, Vector B)
        {
            return new Vector(-A * B.y, A * B.x);
        }

        public static Vector cross(this float A, Vector B)
        {
            return new Vector(-A * B.y, A * B.x);
        }

        public static Vector cross(this int A, Vector B)
        {
            return new Vector(-A * B.y, A * B.x);
        }

        public static double cross(this Vector A, Vector B)
        {
            return A.x * B.y - A.y * B.x;
        }
    }

    enum ViewModes
    {
        /// <summary>
        /// Mode in which the user can interact and modify the scene.
        /// </summary>
        Edit,
        /// <summary>
        /// Mode in which the user waits for the simulation to finish.
        /// </summary>
        RunSim,
        /// <summary>
        /// Mode in which the user can retrieve information from the simulation.
        /// </summary>
        GetInfo
    }

    public enum PointDisplayModes
    {
        /// <summary>
        /// Show the point as a circle (less precision)
        /// </summary>
        Circle,
        /// <summary>
        /// Show the point as a cross (more precision)
        /// </summary>
        Cross
    }

    static class StaticParameters
    {
        public static PointDisplayModes pointDisplay = PointDisplayModes.Circle;
        public static ViewModes viewMode = ViewModes.Edit;
        public static bool showRayNormals = false;

        public static bool useDiffraction = false;
    }
}

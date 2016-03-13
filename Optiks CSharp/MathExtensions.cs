using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Optiks_CSharp
{
    struct MathExt
    {
        public static readonly double EPSILON = 10e-12;

        public static readonly double TAU = 2 * Math.PI;
        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        public static readonly double DEGREES = 180 / Math.PI;
        /// <summary>
        /// converts degrees to radians.
        /// </summary>
        public static readonly double RADIANS = Math.PI / 180;

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
        /// <returns></returns>
        public static double[] quad(double a, double b, double c)
        {
            var sqrt = Math.Sqrt(b * b - 4 * a * c);
            return new double[] { (-b + sqrt) / (2 * a), (-b - sqrt) / (2 * a) };
        }

        internal static Vector evalBezier(Line para, double t)
        {
            var tm1 = 1 - t;
            return
                tm1 * tm1 * para.start +
                2 * tm1 * t * para.bezierHandle +
                t * t * para.end;
        }
    }
}

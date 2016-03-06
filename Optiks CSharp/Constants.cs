using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optiks_CSharp
{
    struct MathExt
    {
        public static readonly double EPSILON = 10e-10;

        public static readonly double TAU = 2*Math.PI;
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
    }
}

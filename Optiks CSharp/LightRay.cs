using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    class LightRay
    {
        public List<Ray> rays;
        public int maxRays;
        public Pen pen;

        private bool stopped;

        public LightRay(Ray startRay, int maxRays, Pen p)
        {
            rays = new List<Ray>(maxRays) { startRay };
            this.maxRays = maxRays;
            pen = p;
            stopped = false;
        }

        public void render(Graphics g, Matrix t)
        {
            int e = (stopped) ? rays.Count : rays.Count - 1;
            for (int i = 0; i < e; i++)
            {
                rays[i].render(pen, g, t);
            }
        }

        public void reset()
        {
            stopped = false;
            Ray s = rays[0];
            s.collision = RayCollisionInfo.EMPTY;
            rays.Clear();
            rays.Add(s);
        }

        /// <summary>
        /// Updates the light ray by one tick.
        /// </summary>
        /// <param name="scene">Scene to get info from.</param>
        public void update(Scene scene)
        {
            if (stopped)
            {
                return;
            }

            Ray ray = rays.Last();
            RayCollisionInfo col = ray.bodyListIntersect(scene.bodies);

            if (!col)
            {
                stopped = true;
                return;
            }

            if (col.body.type == BodyTypes.Absorbing)
            {
                stopped = true;
                return;
            }

            // Extreme math starts here
            Ray ray2;
            Vector l = ray.udir;
            Vector n = col.normal;
            if (l * n > 0) { n = -n; }

            double cos1 = -n * l;
            Vector reflect = l + 2 * cos1 * n;

            if (col.body.type == BodyTypes.Reflecting)
            {
                if (rays.Count == rays.Capacity)
                {
                    stopped = true;
                    return;
                }
                ray2 = new Ray(col.contactPoint, reflect);
                rays.Add(ray2);

                return;
            }

            // Refracting, find refraction indices (n1, n2)
            double n1, n2;

            if (col.secondBody)
            { // Body - Body
                n1 = col.body.refractionIndex;
                n2 = col.secondBody.refractionIndex;
            }
            else if (col.normal * ray.udir > 0)
            { // Air - body
                n1 = scene.airRefractionIndex;
                n2 = col.body.refractionIndex;
            }
            else
            { // Body - Air
                n1 = col.body.refractionIndex;
                n2 = scene.airRefractionIndex;
            }

            double r = n1 / n2;
            double cos2Sqr = 1 - r * r * (1 - cos1 * cos1);

            if (cos2Sqr < 0)
            { // Total internal refraction
                ray2 = new Ray(col.contactPoint, reflect);
            }
            else
            {
                double cos2 = Math.Sqrt(cos2Sqr);
                Vector refract = r * l + (r * cos1 - cos2) * n;
                ray2 = new Ray(col.contactPoint, refract);
            }

            if (rays.Count == rays.Capacity)
            {
                stopped = true;
                return;
            }
            rays.Add(ray2);
        }
    }
}

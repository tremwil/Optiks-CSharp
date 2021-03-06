﻿using System;
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
        public float wavelength;

        public Pen pen;

        public bool stopped;

        private bool empty;
        public static LightRay NONE = new LightRay();

        public LightRay(Ray startRay, int maxRays, Pen p)
        {
            rays = new List<Ray>(maxRays) { startRay };
            this.maxRays = maxRays;
            pen = p;
            stopped = false;
            empty = false;

            wavelength = 650 - 35 * p.Color.GetHue() / 48;
        }

        public LightRay(Ray startRay, int maxRays, float wavelength, float width)
        {
            rays = new List<Ray>(maxRays) { startRay };
            this.maxRays = maxRays;
            this.wavelength = wavelength;
            stopped = false;
            empty = false;

            var hue = 48 * (650 - wavelength) / 35;
            pen = new Pen(ExtensionsAndMethods.toRGB(hue), width);
        }

        public LightRay()
        {
            empty = true;
        }

        public static implicit operator bool (LightRay r)
        {
            return !r.empty;
        }

        public void render(Graphics g, Matrix t)
        {
            if (StaticParameters.viewMode != ViewModes.Edit)
            {
                int e = (stopped) ? rays.Count : rays.Count - 1;
                for (int i = 0; i < e; i++)
                {
                    (t * rays[0].start).render(4, Brushes.Red, g);
                    rays[i].render(pen, g, t);
                }
            }

            else
            {
                var v = (t * rays[0].start);
                v.render(4, Brushes.Red, g);
                g.DrawLine(pen, v, (v + rays[0].udir * 60));
            }
        }

        public void reset()
        {
            stopped = false;
            Ray s = rays[0];
            s.collision = RayCollisionInfo.NONE;
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
                if (rays.Count == maxRays)
                {
                    stopped = true;
                    return;
                }
                rays.Last().collision.newUdir = reflect;
                ray2 = new Ray(col.contactPoint, reflect);
                rays.Add(ray2);

                return;
            }

            // Refracting, find refraction indices (n1, n2)
            double n1, n2;

            if (col.secondBody)
            { // Body - Body
                n1 = getDispersion(col.body);
                n2 = getDispersion(col.secondBody);

                if (col.secondBody.type == BodyTypes.Reflecting)
                {
                    rays.Last().collision.newUdir = reflect;
                    ray2 = new Ray(col.contactPoint, reflect);
                    rays.Add(ray2);
                    return;
                }
                if (col.secondBody.type == BodyTypes.Absorbing)
                {
                    stopped = true;
                    return;
                }
            }
            else if (col.normal * ray.udir > 0)
            { // Air - body
                n1 = scene.airRefractionIndex;
                n2 = getDispersion(col.body);
            }
            else
            { // Body - Air
                n1 = getDispersion(col.body);
                n2 = scene.airRefractionIndex;
            }

            double r = n1 / n2;
            double cos2Sqr = 1 - r * r * (1 - cos1 * cos1);

            if (cos2Sqr < 0)
            { // Total internal refraction
                rays.Last().collision.newUdir = reflect;
                ray2 = new Ray(col.contactPoint, reflect);
            }
            else
            {
                double cos2 = Math.Sqrt(cos2Sqr);
                Vector refract = r * l + (r * cos1 - cos2) * n;
                rays.Last().collision.newUdir = refract;
                ray2 = new Ray(col.contactPoint, refract);
            }

            if (rays.Count == maxRays)
            {
                stopped = true;
                return;
            }
            rays.Add(ray2);
        }

        public LightRay Clone()
        {
            return new LightRay(
                new Ray(
                    new Vector(rays[0].start),
                    new Vector(rays[0].udir)
                ),
                maxRays,
                new Pen(pen.Brush, pen.Width)
            );
        }

        public double getDispersion(Body b)
        {
            if (!StaticParameters.useDispersion)
            {
                return b.refractionIndex;
            }

            var w = wavelength;
            var n = b.refractionIndex;
            var a = b.abbeNumber;

            return n + (589.3 - w) * 5 * 10e+5 / (a * 589.3 * w * w);
        }
    }
}

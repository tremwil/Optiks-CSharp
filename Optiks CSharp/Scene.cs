using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Optiks_CSharp
{
    class Scene
    {
        public List<Body> bodies;
        public List<LightRay> lightRays;
        public double airRefractionIndex = 1;

        public Scene(List<Body> bodies, List<LightRay> lrays, double airRefraction)
        {
            this.bodies = bodies;
            this.lightRays = lrays;
            this.airRefractionIndex = airRefraction;
        }

        public Scene(List<Body> bodies, List<LightRay> lrays)
        {
            this.bodies = bodies;
            this.lightRays = lrays;
        }

        public void reset()
        {
            for (int i = 0; i < lightRays.Count; i++)
            {
                lightRays[i].reset();
            }
        }

        public void physicsTick()
        {
            foreach (LightRay r in lightRays)
            {
                r.update(this);
            }
        }

        public void renderBodies(Graphics g, Matrix t)
        {
            foreach (Body b in bodies)
            {
                b.render(g, t);
            }
        }

        public void renderLightRays(Graphics g, Matrix t)
        {
            foreach (LightRay r in lightRays)
            {
                r.render(g, t);
            }
        }

        public void renderTick(Graphics g, Matrix t)
        {
            renderBodies(g, t);
            renderLightRays(g, t);
        }
    }
}

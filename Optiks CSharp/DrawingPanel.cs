using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Optiks_CSharp
{
    /// <summary>
    /// Simple double-buffered panel.
    /// </summary>
    class DrawingPanel : Panel
    {
        public DrawingPanel()
        {
            DoubleBuffered = true;
        }
    }
}

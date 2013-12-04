using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace N_Tris
{
    public abstract class CanvasDrawer
    {
        protected Canvas canvas;
        protected Canvas Canvas
        {
            get { return this.canvas; }
            set { this.canvas = value; }
        }

        public CanvasDrawer(Canvas canvas)
        {
            this.Canvas = canvas;
        }

        public abstract void Draw();
    }
}

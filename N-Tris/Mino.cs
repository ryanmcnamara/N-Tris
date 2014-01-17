using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace N_Tris
{
    public class Mino
    {
        public Vector2 v;
        public readonly Polyomino p;

        public Mino(Vector2 v, Polyomino p)
        {
            this.v = v;
            this.p = p;
        }

        public override int GetHashCode()
        {
            return v.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj as Mino == null)
            {
                return base.Equals(obj);
            }
            Mino o = (Mino)obj;
            return v.Equals(o.v);
        }

        public void drawMino(Canvas canvas, double minoSize, bool ghost )
        {
            double x = v.X * minoSize;
            double y = canvas.ActualHeight - minoSize;
            y = y - v.Y * minoSize;

            drawMino(canvas, minoSize, x, y, ghost);
        }

        public void drawMino(Canvas canvas, double minoSize, double locx, double locy, bool ghost )
        {
            SolidColorBrush c = new SolidColorBrush( Color.FromRgb( p.colorR, p.colorG, p.colorB ) );
            if (ghost)
            {
                c.Opacity = c.Opacity / 2;
            }

            double x = locx;
            double y = locy;

            Rectangle rect = new Rectangle { Stroke = Brushes.Black, StrokeThickness = 2, Height = minoSize, Width = minoSize, Fill = c };

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);
        }

    }
}

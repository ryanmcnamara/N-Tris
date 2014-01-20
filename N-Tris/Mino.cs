using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ExtensionMethods;
using System.Windows;
using N_TrisNetworkInterface;

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

        public static void drawMino(Canvas canvas, double minoSize, bool ghost, Vector2 v, int color )
        {
            double x = v.X * minoSize;
            double y = canvas.Height - minoSize;
            y = y - v.Y * minoSize;

            Mino.drawMino(canvas, minoSize, x, y, ghost, color);
        }


        public void drawMino(Canvas canvas, double minoSize, bool ghost )
        {
            Mino.drawMino(canvas, minoSize, ghost, this.v, this.p.color);
        }

        public static void drawMino(Canvas canvas, double minoSize, double locx, double locy, bool ghost, int color)
        {
            Color col = (new Color()).FromArgb(color);
            SolidColorBrush c = new SolidColorBrush(col);
            if (ghost)
            {
                c.Opacity = c.Opacity / 2;
            }

            double x = locx;
            double y = locy;

            Rectangle rect = new Rectangle { Stroke = Brushes.Black, StrokeThickness = 1, Height = minoSize, Width = minoSize, Fill = c };

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);
        }

        public void drawMino(Canvas canvas, double minoSize, double locx, double locy, bool ghost )
        {
            Mino.drawMino(canvas, minoSize, locx, locy, ghost, this.p.color);
        }

        public static void drawPolyomino(Polyomino p, Canvas canvas, Vector2 location, double minoSize, bool ghost)
        {
            foreach (Vector2 v in p.Minos)
            {
                Mino.drawMino(canvas, minoSize, ghost, v + location, p.color );
            }

            return;
        }

        public static void drawPolyomino(Polyomino p, Canvas canvas, double minoSize, double locx, double locy, bool ghost)
        {
            foreach (Vector2 v in p.Minos)
            {
                double x = v.X * minoSize + locx;
                double y = -v.Y * minoSize + locy;
                
                drawMino(canvas, minoSize, x, y, ghost, p.color );
            }

        }

        public static void drawPolyomino(Polyomino p, Canvas canvas, Rect r, bool ghost)
        {
            int minx = int.MaxValue;
            int miny = int.MaxValue;
            int maxx = int.MinValue;
            int maxy = int.MinValue;

            foreach (Vector2 v in p.Minos)
            {
                minx = Math.Min(minx, v.X);
                maxx = Math.Max(maxx, v.X);
                miny = Math.Min(miny, v.Y);
                maxy = Math.Max(maxy, v.Y);
            }

            double minoWidth = (r.Width) / (maxx - minx + 1);
            double minoHeight = (r.Height) / (maxy - miny + 1);


            double minoSize = r.Width / p.Minos.Count;

            // assume wider than tall
            double realWidth = minoSize * (maxx - minx + 1);
            double realHeight = minoSize * (maxy - miny + 1);

            double midx = ((double)minx + maxx) / 2;
            double midy = ((double)miny + maxy) / 2;

            midx *= minoSize;
            midy *= minoSize;

            double x = r.Left + r.Width / 2 - midx - minoSize / 2;
            double y = r.Top + r.Height / 2 + midy - minoSize / 2;

            drawPolyomino(p, canvas, minoSize, x, y, ghost);

        }

    }
}

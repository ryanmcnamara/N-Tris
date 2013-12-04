using N_Tris.Utilities;
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
    class PolyominoDrawer : CanvasDrawer
    {
        private static double squareSize = 70;
        public PolyominoDrawer(Canvas canvas)
            : base(canvas)
        {
            
        }

        public void drawPolyomino( Polyomino p )
        {
            canvas.Children.Clear();
            double xOffset = squareSize;
            double yOffset = Canvas.Height - squareSize * 5;

            foreach (Vector2 x in p.Minos)
            {
                Rectangle rect = new Rectangle { Stroke = Brushes.White, StrokeThickness = 2, Height = squareSize, Width = squareSize, Fill = Brushes.Black };
                double left = xOffset + x.X * squareSize;
                double right = yOffset - x.Y * squareSize;
                Canvas.SetLeft(rect, left );
                Canvas.SetTop(rect,  right);
                canvas.Children.Add( rect  );
            }

        }

        public override void Draw()
        {

        }
    }
}

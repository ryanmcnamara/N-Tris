using N_TrisNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace N_Tris
{
    class AccessoryDrawer
    {
        internal void draw(Canvas accessoryCanvas, GameBoardData e)
        {
            accessoryCanvas.Children.Clear();

            double holdCellWidth = 2 * accessoryCanvas.ActualWidth / 3;

            Rectangle rect = new Rectangle { Stroke = Brushes.Black, StrokeThickness = 2, Height = holdCellWidth, Width = holdCellWidth, Fill = Brushes.Black };

            Canvas.SetLeft(rect, ( accessoryCanvas.ActualWidth - holdCellWidth ) / 2 );
            Canvas.SetTop(rect, 0);
            accessoryCanvas.Children.Add(rect);

            if (e.HeldPolyomino != null)
            {
                Polyomino p = e.HeldPolyomino.Clone();
                p.SRSNormalize();
                Rect r = new Rect((accessoryCanvas.ActualWidth - holdCellWidth) / 2, 0, holdCellWidth, holdCellWidth);
                Mino.drawPolyomino(p, accessoryCanvas, r, false);
            }

            //List<Polyomino> nexts = e.MyPieceDealer.peekNextPolyominos(5);

            //double startNextsY = holdCellWidth * 1.5;

            //for (int i = 0; i < 5; i++)
            //{
            //    double x = (accessoryCanvas.ActualWidth - holdCellWidth) / 2;
            //    double y = startNextsY + i * holdCellWidth;

            //    Rectangle fill = new Rectangle { Stroke = Brushes.Black, StrokeThickness = 2, Height = holdCellWidth, Width = holdCellWidth, Fill = Brushes.Black };

            //    Canvas.SetLeft(fill, x);
            //    Canvas.SetTop(fill, y);
            //    accessoryCanvas.Children.Add(fill);

            //    Polyomino p = nexts[i].Clone();
            //    p.SRSNormalize();
            //    Rect r = new Rect(x,y, holdCellWidth, holdCellWidth);
            //    p.drawPolyomino(accessoryCanvas, r, false);
            //}
        }

        internal void drawRight(Canvas accessoryCanvas, GameBoardData e)
        {
            accessoryCanvas.Children.Clear();

            double holdCellWidth = 2 * accessoryCanvas.ActualWidth / 3;

            var nextPoly = e.PolyominoQueue.First;

            double startNextsY = 0;

            for (int i = 0; i < 5; i++)
            {
                double x = (accessoryCanvas.ActualWidth - holdCellWidth) / 2;
                double y = startNextsY + i * holdCellWidth;

                Rectangle fill = new Rectangle { Stroke = Brushes.Black, StrokeThickness = 2, Height = holdCellWidth, Width = holdCellWidth, Fill = Brushes.Black };

                Canvas.SetLeft(fill, x);
                Canvas.SetTop(fill, y);
                accessoryCanvas.Children.Add(fill);

                Polyomino p = nextPoly.Value.Clone();
                nextPoly = nextPoly.Next;
                p.SRSNormalize();
                Rect r = new Rect(x, y, holdCellWidth, holdCellWidth);
                Mino.drawPolyomino(p, accessoryCanvas, r, false);
            }
        }
    }
}

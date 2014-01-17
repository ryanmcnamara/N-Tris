using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace N_Tris
{
    public class GameBoardDrawer
    {
        private double minoBorderSize;

        private double minoDrawSize;
        private double MinoDrawSize
        {
            get { return minoDrawSize; }
            set
            {
                minoBorderSize = value / 10;
                minoDrawSize = value;
            }
        }

        private Canvas canvas;

        public GameBoardDrawer(Canvas aCanvas)
        {
            canvas = aCanvas;
        }

        public void draw(GameBoardData data)
        {
            
            canvas.Children.Clear();
            MinoDrawSize = canvas.Width / data.Width;


            Rectangle rect = new Rectangle { Stroke = Brushes.Black, StrokeThickness = 2, Height = canvas.Height, Width = canvas.ActualWidth, Fill = Brushes.Black };

            Canvas.SetLeft(rect, 0);
            Canvas.SetTop(rect, 0);
            canvas.Children.Add(rect);

            data.FallingPolyomino.drawPolyomino(canvas, data.FallingPolyominoLocation, MinoDrawSize, false);
            data.FallingPolyomino.drawPolyomino(canvas, data.getGhostPieceLocation(), minoDrawSize, true);

            //todo remove
            //Ellipse e = new Ellipse { Fill = Brushes.Red, Height = 10, Width = 10 };
            //double x = data.FallingPolyominoLocation.X * minoDrawSize + minoDrawSize / 2 -5;
            //double y = canvas.Height - minoDrawSize / 2 - 5;
            //y -= minoDrawSize * data.FallingPolyominoLocation.Y;
            //Canvas.SetLeft(e, x);
            //Canvas.SetTop(e, y);
            //canvas.Children.Add(e);

            lock (data.SettledMinos)
            {
                foreach (Mino m in data.SettledMinos)
                {
                    Vector2 v = m.v;
                    m.drawMino(canvas, MinoDrawSize, false);
                }
                drawGridLines(canvas);
            }
        }

        private void drawGridLines(Canvas canvas)
        {
            for (double i = 0; i <= canvas.ActualWidth; i += minoDrawSize)
            {
                // vertical lines
                Line l = new Line { Stroke = Brushes.White, StrokeThickness = 1, X1 = i, Y1 = canvas.Height, X2 = i, Y2 = 0 };
                canvas.Children.Add(l);
            }
            for (double j = 0; j <= canvas.Height; j += minoDrawSize)
            {
                // vertical lines
                Line l = new Line { Stroke = Brushes.White, StrokeThickness = 1, X1 = 0, Y1 = canvas.Height - j, X2 = canvas.ActualWidth, Y2 = canvas.Height - j };
                canvas.Children.Add(l);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace N_Tris
{
    class GameBoardDrawer
    {
        private GameBoardManager manager;
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

        public void draw(GameBoardManager manager)
        {
            this.manager = manager;
            
            canvas.Children.Clear();
            MinoDrawSize = canvas.ActualWidth / manager.Width;

            drawPolyomino(manager.FallingPolyomino, manager.FallingPolyominoLocation, false);
            drawPolyomino(manager.FallingPolyomino, manager.getGhostPieceLocation(), true);

            Ellipse e = new Ellipse { Fill = Brushes.Red, Height = 10, Width = 10 };
            double x = manager.FallingPolyominoLocation.X * minoDrawSize + minoDrawSize / 2 -5;
            double y = canvas.ActualHeight - minoDrawSize / 2 - 5;
            y -= minoDrawSize * manager.FallingPolyominoLocation.Y;
            Canvas.SetLeft(e, x);
            Canvas.SetTop(e, y);
            canvas.Children.Add(e);

            foreach (Mino m in manager.SettledMinos)
            {
                Vector2 v = m.v;
                drawMino( canvas, new Vector2( v.X, v.Y ), m.p.Color ); 
            }
            drawGridLines(canvas);
        }

        private void drawPolyomino(Polyomino p, Vector2 location, bool ghost )
        {
            Brush color = p.Color;
            if (ghost)
            {
                color = color.Clone();
                color.Opacity = color.Opacity / 2;
            }
            foreach (Vector2 v in p.Minos)
            {
                
                drawMino(canvas, new Vector2(v.X + location.X, location.Y + v.Y), color);
            }
        }

        private void drawGridLines(Canvas canvas)
        {
            for (double i = 0; i <= canvas.ActualWidth; i += minoDrawSize)
            {
                // vertical lines
                Line l = new Line { Stroke = Brushes.Black, StrokeThickness = 1, X1 = i, Y1 = canvas.ActualHeight, X2 = i, Y2 = 0 };
                canvas.Children.Add(l);
            }
            for (double j = 0; j <= canvas.ActualHeight; j += minoDrawSize)
            {
                // vertical lines
                Line l = new Line { Stroke = Brushes.Black, StrokeThickness = 1, X1 = 0, Y1 = canvas.ActualHeight - j, X2 = canvas.ActualWidth, Y2 = canvas.ActualHeight - j };
                canvas.Children.Add(l);
            }
        }

        public void drawMino(Canvas canvas, Vector2 location, Brush color )
        {
            double x = location.X * MinoDrawSize;
            double y = canvas.ActualHeight - MinoDrawSize;
            y = y - location.Y * MinoDrawSize;

            Rectangle rect = new Rectangle { Stroke = Brushes.White, StrokeThickness = 2, Height = MinoDrawSize, Width = MinoDrawSize, Fill = color };

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);
        }
    }
}

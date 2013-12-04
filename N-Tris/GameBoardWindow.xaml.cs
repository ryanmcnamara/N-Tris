using N_Tris.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace N_Tris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameBoardWindow : Window
    {

        List<Polyomino> polys;
        GameBoardDrawer drawer;
        GameBoardManager manager;
        public GameBoardWindow()
        {
            int n = 4;
            InitializeComponent();

            drawer = new GameBoardDrawer(BoardCanvas);

            manager = new GameBoardManager(n, drawer);

            //index = 0;
            //n = 4;
            //PolyominoGenerator gen = new PolyominoGenerator( );
            //polys = gen.getPolyominos(n);

            //for (int i = 1; i <= n; i++)
            //{
            //    List<Polyomino> x = gen.getPolyominos(i);
            //    Console.WriteLine(x.Count);
            //}

            //drawer = new PolyominoDrawer( BoardCanvas );
            //drawer.drawPolyomino(polys[index]);

            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);


            //WallKickStrategy kicks = new WallKickStrategy();
            //for( int  i  = 0; i < 8; i++ )
            //{
            //List<Vector2> asdf = kicks.getStrategyCopy(8, i, true);
            //foreach ( Vector2 v in asdf )
            //{
            //    Console.Write("(" + v.X + ", " + v.Y + "),");
            //}
            //Console.WriteLine(asdf);
            //}
            
        }


        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            manager.Start();
            this.KeyDown -= new KeyEventHandler(OnButtonKeyDown);
            this.KeyDown += new KeyEventHandler(manager.keyDown);
            //index++;
            //index = index % polys.Count;
            //drawer.drawPolyomino(polys[index]);
        }



    }
}

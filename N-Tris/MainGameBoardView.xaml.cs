using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Interaction logic for MainGameBoardView.xaml
    /// </summary>
    public partial class MainGameBoardView : UserControl
    {
        GameBoardDrawer drawer;

        public MainGameBoardView( BoardChangedEvent boardChanger )
        {
            InitializeComponent();
            this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            drawer = new GameBoardDrawer(BoardCanvas);

            boardChanger.boardChanged += boardChangeFired;

        }

        private void boardChangeFired(object sender, GameBoardData e)
        {
            drawer.draw(e);
        }

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //     manager.endGame();

        //    if ( e != null && gameThread.IsAlive )
        //        gameThread.Join();
            
        //    //base.OnClosing(e); todo
        //}
    }
}

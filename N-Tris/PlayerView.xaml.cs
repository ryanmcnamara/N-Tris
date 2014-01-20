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
using System.IO;

namespace N_Tris
{
    


    /// <summary>
    /// Interaction logic for SinglePlayerView.xaml
    /// </summary>
    public abstract partial class PlayerView : UserControl, RunnableUserControl
    {
        public EventHandler<UserControl> WindowChangeEvent;

        protected BoardChangedEvent boardChanger;

        public bool Paused { get; set; }

        public PlayerView( int height = 800 )
        {
            Paused = false;
            InitializeComponent();

            boardChanger = new BoardChangedEvent();

            myPanel.Children.Add(new GameBoardLeftAccessoriesView(boardChanger, height));
            myPanel.Children.Add(new MainGameBoardView(boardChanger, height));
            myPanel.Children.Add(new GameBoardRightAccessoriesView(boardChanger, height));

        }



        public virtual void setUpSimulate()
        {
            throw new NotImplementedException();
        }

        public virtual void simulateFrame(int millis)
        {
            throw new NotImplementedException();
        }

        public virtual void addToWindowChangeEvent(EventHandler<UserControl> changeEvent)
        {
            throw new NotImplementedException();
        }


    }
}

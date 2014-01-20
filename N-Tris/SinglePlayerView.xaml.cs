using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for SinglePlayerView.xaml
    /// </summary>
    public partial class SinglePlayerView : UserControl, RunnableUserControl
    {
        EventHandler<UserControl> WindowChangeEvent;
        public SinglePlayerView() : this( 4 )
        {
        }

        public SinglePlayerView(int n)
        {
            InitializeComponent();
            this.polyCountBox.Text = "" + n;
        }

        private void Robot_Click(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt32(polyCountBox.Text);
            WindowChangeEvent(this, new MultiPlayerView(n, new LocalPlayerView( n, new RobotMinimaxPlayer(), null ), new List<PlayerView>()));
        }

        public void setUpSimulate()
        {
        }

        public void simulateFrame(int millis)
        {
        }

        public void addToWindowChangeEvent(EventHandler<UserControl> changeEvent)
        {
            WindowChangeEvent += changeEvent;
        }

        private void Human_Click(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt32(polyCountBox.Text);
            WindowChangeEvent(this, new MultiPlayerView(n, new LocalPlayerView( n, new HumanPlayer(), null ), new List<PlayerView>()));
        }
    }
}

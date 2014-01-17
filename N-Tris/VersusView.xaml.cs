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
    /// Interaction logic for VersusView.xaml
    /// </summary>
    public partial class VersusView : UserControl, RunnableUserControl
    {
        public EventHandler<UserControl> WindowChangeEvent;

        public VersusView() : this(4)
        {
        }

        public VersusView(int p)
        {
            InitializeComponent();
            polyCountBox.Text = "" + p;
        }

        private void RobotButton_Click(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt32( polyCountBox.Text );
            List<PlayerView> opponents = new List<PlayerView>();
            opponents.Add( new LocalPlayerView( n, new RobotMinimaxPlayer() ) );
            WindowChangeEvent(this, new MultiPlayerView(n, new LocalPlayerView(n, new HumanPlayer()), opponents)); 
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
    }
}

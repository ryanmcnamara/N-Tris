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
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : UserControl, RunnableUserControl
    {
        public EventHandler<UserControl> WindowChangeEvent;

        public SplashScreen()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt32(polyCountBox.Text);
            WindowChangeEvent(sender, new SinglePlayerView(n));
        }

        public void addToWindowChangeEvent(EventHandler<UserControl> changeEvent)
        {
            WindowChangeEvent += changeEvent;
        }


        public void setUpSimulate()
        {
        }

        public void simulateFrame( int millis )
        {
        }

    }
}

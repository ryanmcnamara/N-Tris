using N_TrisNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Interaction logic for VersusMatchMakingView.xaml
    /// </summary>
    public partial class VersusMatchMakingView : UserControl, RunnableUserControl
    {
        EventHandler<UserControl> WindowChangedEvent;

        //networking
        DuplexChannelFactory<IN_TrisServer> channelFactory;
        IN_TrisServer server;
        NTrisClientImpl client;


        public VersusMatchMakingView()
        {
            InitializeComponent();
            App.Current.MainWindow.Width = 1700;
        }


        public void setUpSimulate()
        {
            client = new NTrisClientImpl( getStart );
            channelFactory = new DuplexChannelFactory<IN_TrisServer>( client, "ChatServiceEndpoint");
            server = channelFactory.CreateChannel();
            server.LogIn(Environment.UserName);
        }

        private void displayData(object sender, string e)
        {
            App.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    DebugText.Text += "Got data : " + e + Environment.NewLine;
                }));
        }

        //private void displayData(string data)
        //{
        //    App.Current.Dispatcher.BeginInvoke((Action)(() =>
        //        {
        //            DebugText.Text += "Got data : " + data + Environment.NewLine;
        //        }));
        //}


        private void getStart( object sender, string[] e )
        {
            //App.Current.Dispatcher.BeginInvoke((Action)(() =>
            //    {
            //        DebugText.Text += "Got start message  " + Environment.NewLine;

            //        int n = Convert.ToInt32(e[0]);
            //        if (n > 12) n = 4;

            //        DebugText.Text += "Opp is " + e[1];
            //    }
            //));
            // create a multiplayer view
            // local view
            int n = Convert.ToInt32(e[0]);
            App.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                LocalPlayerView localPlayer = new LocalPlayerView(n, new HumanPlayer(), server /*e[0].Contains( "1")*/);

                List<PlayerView> opponentViews = new List<PlayerView>();

                int oppCount = e.Count() - 1;
                int width = 1000;
                int height = 800;
                int cols = -1;
                int cellHeight = 0;

                // todo, there must be a fastern time than linear....... not a problem though...
                for (int i = 1; i <= oppCount; i++)
                {
                    // what if this many cols
                    int thisWidth = (int)(((double)(width))/ i );
                    int thisHeight = (int)(((double)(height)) / ((oppCount)/i));

                    thisHeight = Math.Min(thisHeight,(int)( thisWidth * ( 800.0 / 700 )));
                    if (thisHeight > cellHeight)
                    {
                        cols = i;
                        cellHeight = thisHeight;
                    }
                }


                for (int i = 1; i < e.Count(); i++)
                {
                    String playerId = e[i];

                    NetworkPlayerView netPlayer = new NetworkPlayerView(playerId, server, client, cellHeight);
                    opponentViews.Add(netPlayer);
                }


                MultiPlayerView multiplayerView = new MultiPlayerView(n, localPlayer, opponentViews, cols, cellHeight );
                WindowChangedEvent(this, multiplayerView);
            }
            ));
        }

        public void simulateFrame(int millis)
        {
            // no op
        }

        public void addToWindowChangeEvent(EventHandler<UserControl> changeEvent)
        {
            WindowChangedEvent += changeEvent;
        }

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Content = "waiting...";
            server.SignUpForGame();
            DebugText.Text += "signed up " + Environment.UserName + " for game";
        }
    }
}

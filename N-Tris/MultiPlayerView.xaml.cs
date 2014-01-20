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
    /// Interaction logic for MultiPlayerView.xaml
    /// </summary>
    public partial class MultiPlayerView : UserControl, RunnableUserControl
    {
        public EventHandler<UserControl> WindowChangeEvent;

        List<PlayerView> players;
        List<PlayerView> opponents;
        PlayerView player;

        MediaPlayer mediaPlayer;

        public MultiPlayerView(int n, PlayerView thisPlayer, List<PlayerView> opponents, int cols = 1, int cellHeight = 800)
        {
            InitializeComponent();
            this.opponents = opponents;
            this.players = new List<PlayerView>(opponents);
            players.Add(thisPlayer);
            this.player = thisPlayer;
            MyPanel.Children.Add(player);

            // add stack panel containing opponents 
            StackPanel vStack = new StackPanel();
            vStack.Orientation = Orientation.Vertical;
            int k = 0;
            while (k < opponents.Count)
            {
                StackPanel hStack = new StackPanel();
                hStack.Orientation = Orientation.Horizontal;
                for (int i = 0; k < opponents.Count && i < cols; i++)
                {
                    hStack.Children.Add(opponents[k]);
                    k++;
                }
                vStack.Children.Add(hStack);
            }
            MyPanel.Children.Add(vStack);


            //foreach (PlayerView p in opponents)
            //{
            //    MyPanel.Children.Add(p);
            //}

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Pause_QuitButton.Click += Pause_QuitButton_Click;
        }

        public void setUpSimulate()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Stop();
            }
            mediaPlayer = null;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                String basedir = AppDomain.CurrentDomain.BaseDirectory;
                mediaPlayer = new MediaPlayer();
                mediaPlayer.Open(new Uri(basedir + @"..\..\Media\Music\heyhey.mp3"));
                if (!player.Paused)
                {
                    //mediaPlayer.Play(); todo
                }
                mediaPlayer = null; // todo
                onUnpause();
            }));

            foreach ( PlayerView view in players )
            {
                view.setUpSimulate();
            }
        }

        public void simulateFrame(int millis)
        {
            foreach ( PlayerView view in players )
            {
                view.simulateFrame( millis );
            }
        }

        private void Pause_QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.Stop();
                    mediaPlayer = null;
                }
            }));
            WindowChangeEvent(sender, new SplashScreen());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
            {
                if (player.Paused)
                {
                    onUnpause();
                }
                else
                {
                    onPause();
                }
            }


        }

        private void onUnpause()
        {
            PauseMenu.Visibility = Visibility.Collapsed;

            if (mediaPlayer != null)
            {
                mediaPlayer.Play();
            }

            player.Paused = false;
        }

        private void onPause()
        {
            player.Paused = true;
            PauseMenu.Visibility = Visibility.Visible; if (mediaPlayer != null)
            {
                mediaPlayer.Pause();
            }

        }

        private void Pause_ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            onUnpause();
        }

        private void Pause_RestartButton_Click(object sender, RoutedEventArgs e)
        {
            setUpSimulate();
        }

        public void addToWindowChangeEvent(EventHandler<UserControl> changeEvent)
        {
            WindowChangeEvent += changeEvent;
        }
    }
}

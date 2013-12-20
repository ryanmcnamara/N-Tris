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
    /// Interaction logic for SinglePlayerView.xaml
    /// </summary>
    public partial class SinglePlayerView : UserControl, RunnableUserControl
    {
        public EventHandler<UserControl> WindowChangeEvent;


        public bool Paused { get; set; }
        private Thread gameThread;
        private GameBoardManager manager;

        MediaPlayer mediaPlayer;
        private int n;
        private GamePlayer player;
    

        public SinglePlayerView() : this(4)
        {
            

        }

        public SinglePlayerView(int n)
        {
            this.n = n; 
            InitializeComponent();
            Pause_QuitButton.Click += Pause_QuitButton_Click;

            myPanel.Children.Add(new GameBoardLeftAccessoriesView());
            myPanel.Children.Add(new MainGameBoardView());
            myPanel.Children.Add(new GameBoardRightAccessoriesView());

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;

            player = new HumanPlayer();
            manager = null;
           
        }

        private void Pause_QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (mediaPlayer != null)
                    {
                        mediaPlayer.Stop();
                    }
                }));
            WindowChangeEvent(sender, new SplashScreen());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
            {
                if (Paused)
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

            mediaPlayer.Play();
            manager.Pause = false;
            Paused = false;
        }

        private void onPause()
        {
            manager.Pause = true;
            PauseMenu.Visibility = Visibility.Visible;
            mediaPlayer.Pause();
            Paused = true;
           
        }

        private void Pause_ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            onUnpause();
        }



        public void setUpSimulate()
        {
            manager = new GameBoardManager(n, player);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {

                mediaPlayer = new MediaPlayer();
                mediaPlayer.Open(new Uri(@"C:\Users\Ryan\Music\music\4 Non Blondes\heyhey.wav"));

                mediaPlayer.Play(); // can also use soundPlayer.PlaySync()
            }));


            //Application.Current.Dispatcher.BeginInvoke(new Action(() => { Application.Current.Shutdown(); }), null);
        }

        public void simulateFrame(int millis)
        {
            manager.GameLoop(millis);
        }


        public void addToWindowChangeEvent(EventHandler<UserControl> changeEvent)
        {
            WindowChangeEvent += changeEvent;
        }


    }
}

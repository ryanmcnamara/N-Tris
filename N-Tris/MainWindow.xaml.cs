using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Windows.Shapes;

namespace N_Tris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControl currentControl;
        private bool running = true;
        private int FrameRate;

        private Thread currentThread;
        private RunnableUserControl runner;

        public MainWindow()
        {
            InitializeComponent();
            FrameRate = 33;
            SplashScreen splash = new SplashScreen();
            onChangeView( null, splash );
            currentControl = splash;

            Application.Current.MainWindow.Closing += MainWindow_Closing;

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            running = false;
            if (currentThread != null)
            {
                currentThread.Join();
            }
        }


        private void loadGameLoop()
        {
            running = true;

            runner = currentControl as RunnableUserControl;
            currentThread = null;
            if ( runner != null )
            {
                // register event

                EventHandler<UserControl> asdf = new EventHandler<UserControl>( onChangeView );
                runner.addToWindowChangeEvent(asdf);

                currentThread = new Thread(() =>
                {
                    runner.setUpSimulate();
                    GameLoop();
                });

                currentThread.Start();

            }
        }

        private void GameLoop()
        {
            long lastFrame = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (running)
            {
                long frameStart = watch.ElapsedMilliseconds;
                int diff = (int)( 1000 / FrameRate );
                runner.simulateFrame(diff);
                lastFrame = watch.ElapsedMilliseconds;

                long elapsed = watch.ElapsedMilliseconds - frameStart;
                Thread.Sleep((int)(Math.Max( 1000 / FrameRate - elapsed, 0)));
                
            }

        }

        private void onChangeView(object sender, UserControl e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action( () =>
                {
                    running = false;
                    mainPanel.Children.Clear();
                    mainPanel.Children.Add(e);

                    currentControl = e;

                    if (currentThread != null)
                    {
                        currentThread.Join();
                    }
                    currentThread = null;
                    loadGameLoop();
                }
            ));

        }

    }
}

using N_TrisNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace N_Tris
{
    class LocalPlayerView : PlayerView 
    {
        private GamePlayer gamePlayer;
        private GameBoardManager manager;
        int n;
        IN_TrisServer server;

        public LocalPlayerView(int n, GamePlayer gamePlayer, IN_TrisServer server )
        {
            this.server = server;
            this.n = n;
            this.gamePlayer = gamePlayer;
            if (server != null)
            {
                this.boardChanger.boardChanged += postGame;
            }
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F)
            {
                postGame(null, manager.Manipulator.Data);
            }
        }

        int testdata = 0;
        private void postGame(object sender, GameBoardData e)
        {
            testdata++;
            server.postGame(e); // todo
        }

        public override void setUpSimulate()
        {
            manager = new GameBoardManager(n, gamePlayer, boardChanger);
        }

        public override void simulateFrame(int millis)
        {
            manager.GameLoop(millis);
        }

    }
}

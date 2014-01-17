using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace N_Tris
{
    class LocalPlayerView : PlayerView 
    {
        private GamePlayer gamePlayer;
        private GameBoardManager manager;
        

        public LocalPlayerView(int n, GamePlayer gamePlayer )
        {
            this.n = n;
            this.gamePlayer = gamePlayer;
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

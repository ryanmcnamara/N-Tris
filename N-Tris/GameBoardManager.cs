using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace N_Tris
{

    public class GameBoardManager
    {

        private GamePlayer player;


        public int FrameRate
        {
            get;
            set;
        }

        private int FrameNumber
        {
            get;
            set;
        }

        private bool pause;
        public bool Pause
        {
            get
            {
                return pause;
            }
            set
            {
                pause = value;
            }
        }

        private BoardChangedEvent BoardChanger { get; set; }


        public GameBoardData Data { get; set; }


        public GameBoardManager(int n, GamePlayer player, BoardChangedEvent boardChanger )
        {
            this.BoardChanger = boardChanger;
            this.player = player;
            
            // rule compliant board size
            int width = n * 2 + n / 2;
            int height = width * 2 + n / 2;

            WallKickStrategy wallKickStrategy = new WallKickStrategy();
            wallKickStrategy.getStrategyCopy(20, 0, false); // precompute

            PolyominoDealer pieceDealer = new PolyominoDealer( n );

            this.Data = new GameBoardData(new HashSet<Mino>(), null, new Vector2(0,0), n, width, height, wallKickStrategy, pieceDealer);
            FrameNumber = 0;
            FrameRate = 32;



        }

        public void GameLoop(int millis)
        {
            if (!Data.GameOver && !Pause)
            {
                Data.passTime(millis);

                processInput();

                // tell ui to draw
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() => BoardChanger.fire(this, Data)));

                //drawer.draw(this.Data);
                FrameNumber++;

            }
        }

        internal void endGame()
        {
            Data.GameOver = true;
        }
        
        public void processInput()
        {
            HashSet<int> moves = player.getMoves(this);

            foreach (int k in moves)
            {
                int x = Data.ProcessMove(k);

                if (k == (int)Constants.Moves.SOFT_DROP && x == 1)
                {
                    Data.FallingCooldown = Data.FallingRate;
                }
            }
        }
    }

}

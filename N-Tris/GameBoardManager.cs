using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

namespace N_Tris
{

    class GameBoardManager
    {
        public int Width
        {
            get;
            private set;
        }
        public int Height
        {
            get;
            private set;
        }

        public HashSet<Mino> SettledMinos
        {
            get;
            private set;
        }

        public Polyomino FallingPolyomino;

        private Polyomino HeldPolyomino { get; set; }
        public Vector2 FallingPolyominoLocation;
        public int n
        {
            get;
            private set;
        }

        public int FallingRate //milliseconds
        {
            get;
            set;
        }
        public int FallingCooldown
        {
            get;
            set;
        }

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

        private PolyominoDealer MyPieceDealer
        {
            get;
            set;
        }

        private Boolean GameRunning
        {
            get;
            set;
        }

        private GameBoardDrawer drawer;

        private HashSet<Key> depressedKeys;

        private WallKickStrategy wallKickStrategy;

        public GameBoardManager(int n, GameBoardDrawer drawer)
        {
            wallKickStrategy = new WallKickStrategy();
            wallKickStrategy.getStrategyCopy(20, 0, false); // precompute
            depressedKeys = new HashSet<Key>();
            GameRunning = true;
            this.drawer = drawer;
            FrameNumber = 0;
            FrameRate = 32;
            this.n = n;
            FallingRate = 700;
            FallingCooldown = FallingRate;
            SettledMinos = new HashSet<Mino>();
            FallingPolyomino = null;

            // rule compliant board size
            Width = n * 2 + n / 2;
            Height = Width * 2 + n / 2;

            MyPieceDealer = new PolyominoDealer(n);


        }

        public void Start()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(GameLoop);

            timer.Interval = new TimeSpan(10 * 1000 * 1000 / FrameRate);
            timer.Start();
        }

        long lastTime;
        private void GameLoop(object sender, EventArgs e)
        {
            Thread.Sleep((int)Math.Max(0, DateTime.UtcNow.Millisecond - lastTime));

            lastTime = DateTime.UtcNow.Millisecond;

            if (FallingPolyomino == null)
            {
                getNextPolyomino();
            }
            else
            {
                FallingCooldown -= 1000 / FrameRate;
            }

            processInput();


            if (FallingCooldown <= 0)
            {
                fallingPolyominoDescend( true );
                FallingCooldown = FallingRate;
            }
            drawer.draw(this);
            FrameNumber++;
        }

        private void processInput()
        {
            foreach (Key k in depressedKeys)
            {
                switch (k)
                {
                    case Key.Left:
                        tryFallingPosition(new Vector2(FallingPolyominoLocation.X - 1, FallingPolyominoLocation.Y));
                        break;
                    case Key.Up:
                        tryRotateClockwise();
                        break;
                    case Key.Down:
                        tryRotateCounterClockwise();
                        break;
                    case Key.Right:
                        tryFallingPosition(new Vector2(FallingPolyominoLocation.X + 1, FallingPolyominoLocation.Y));
                        break;
                    case Key.Space:
                        while (fallingPolyominoDescend( true )) ;
                        break;
                    case Key.Z:
                        if (fallingPolyominoDescend(false))
                        {
                            FallingCooldown = FallingRate;
                        }
                        break;
                    case Key.LeftShift:
                    case Key.RightShift:
                        holdPolyomino();
                        break;
                    case Key.N : 
                        FallingPolyomino.SRSNormalize();
                        break;
                }
            }

            depressedKeys.Clear();
        }

        private void holdPolyomino()
        {
            Polyomino toHold = FallingPolyomino;
            if (HeldPolyomino != null)
            {
                loadPolyomino(HeldPolyomino);
            }
            else
            {
                getNextPolyomino();
            }
            HeldPolyomino = toHold;
        }

        public void tryRotateClockwise()
        {
            FallingPolyomino.rotate90C();
            if (!wallKickAfterRotation(( FallingPolyomino.rotation + 3) % 4))
            {
                FallingPolyomino.rotate90CC();
            }
        }

        public void tryRotateCounterClockwise()
        {
            FallingPolyomino.rotate90CC();
            if (!wallKickAfterRotation(4 + ((7-FallingPolyomino.rotation) % 4)))
            {
                FallingPolyomino.rotate90C();
            }
        }

        private bool wallKickAfterRotation(int rot)
        {
            foreach (Vector2 p in wallKickStrategy.getStrategyCopy(FallingPolyomino.Minos.Count, rot, FallingPolyomino.isIPolyomino()))
            {
                if (tryFallingPosition(p + FallingPolyominoLocation))
                {
                    return true;
                }
            }
            return false;
        }

        private bool tryFallingPosition(Vector2 v)
        {
            foreach (Vector2 p in FallingPolyomino.Minos)
            {
                Vector2 test = new Vector2(v.X + p.X, v.Y + p.Y);
                if ((test.X < 0 || test.Y < 0) ||
                     (test.X >= Width || test.Y >= Height) ||
                     (SettledMinos.Contains(new Mino(test, FallingPolyomino))))
                {
                    return false;
                }
            }
            FallingPolyominoLocation = v;
            return true;
        }

        private bool fallingPolyominoDescend( bool replaceIfInvalid )
        {
            if (!tryFallingPosition(new Vector2(FallingPolyominoLocation.X, FallingPolyominoLocation.Y - 1)))
            {
                if (replaceIfInvalid)
                {
                    foreach (Vector2 v in FallingPolyomino.Minos)
                    {
                        Mino toAdd = new Mino(new Vector2(v.X + FallingPolyominoLocation.X, v.Y + FallingPolyominoLocation.Y), FallingPolyomino);
                        SettledMinos.Add( toAdd );
                    }
                    tryClearLines(0, Width - 1);

                    getNextPolyomino();
                }
                return false;
            }
            return true;
        }

        void loadPolyomino(Polyomino p)
        {
            FallingPolyomino = p;
            if (!tryFallingPosition(new Vector2(n + (n / 2) / 2 - 1, Height - n / 2 - 1)))
            {
                GameRunning = false;
            }
            FallingCooldown = FallingRate;
        }

        private void getNextPolyomino()
        {
            Polyomino p = MyPieceDealer.getNextPolyomino();

            loadPolyomino( p );
        }

        internal void keyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            depressedKeys.Add(e.Key);
        }

        public Vector2 getGhostPieceLocation()
        {
            Vector2 originalLocation = FallingPolyominoLocation;
            Vector2 position = FallingPolyominoLocation;
            while (fallingPolyominoDescend( false ))
            {
                position = FallingPolyominoLocation;
            }
            FallingPolyominoLocation = originalLocation;
            return position;
        }

        public void tryClearLines()
        {
            // use falling polyomino and its location
            tryClearLines(0, Width - 1);
        }

        public void tryClearLines(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (tryClearLine(i))
                {
                    i--;
                    end--;
                }
            }
        }

        public bool tryClearLine(int i)
        {
            if (isLineFull(i))
            {
                //remove
                for (int j = 0; j < Width; j++)
                {
                    SettledMinos.Remove( new Mino( new Vector2(j, i), FallingPolyomino ) );
                }
                HashSet<Mino> toDec = new HashSet<Mino>();
                foreach (Mino m in SettledMinos)
                {
                    if (m.v.Y > i)
                    {
                        toDec.Add(m);
                    }
                }
                foreach (Mino m in toDec)
                {
                    SettledMinos.Remove(m);
                }
                foreach (Mino m in toDec)
                {
                    SettledMinos.Add( new Mino( new Vector2(m.v.X, m.v.Y - 1), m.p ) );
                }
                return true;
            }
            return false;
        }

        public bool isLineFull(int i)
        {
            for (int j = 0; j < Width; j++)
            {
                if (!SettledMinos.Contains(new Mino(new Vector2(j, i), FallingPolyomino)))
                {
                    return false;
                }
            }

            return true;

        }

    }

}

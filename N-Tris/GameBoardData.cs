using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N_Tris
{
    public class GameBoardData
    {
        public HashSet<Mino> SettledMinos
        {
            get;
            set;
        }

        public Polyomino FallingPolyomino;

        public Polyomino HeldPolyomino { get; set; }
        public bool UsedHold { get; set; }
        public Vector2 FallingPolyominoLocation;
        public int n
        {
            get;
            set;
        }

        public WallKickStrategy WallKickStrategy { get; set; }

        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }

        public readonly int maxInfinity = 10;
        public int InfinityCount { get; private set; }
        


        public PolyominoDealer MyPieceDealer { get; set; }

        public bool GameOver { get; set; }

        public readonly int FallingRate;
        public int FallingCooldown
        {
            get;
            set;
        }

        public GameBoardData(HashSet<Mino> SettledMinos, Polyomino FallingPolyomino, Vector2 FallingPolyominoLocation, int n, int Width, 
                             int Height, WallKickStrategy WallKickStrategy, PolyominoDealer dealer, bool usedHold = false )
        {
            this.GameOver = false;
            this.SettledMinos = SettledMinos;
            this.FallingPolyomino = FallingPolyomino;
            this.FallingPolyominoLocation = FallingPolyominoLocation;
            this.n = n;
            this.Height = Height;
            this.Width = Width;
            this.WallKickStrategy = WallKickStrategy;
            this.MyPieceDealer = dealer;
            this.UsedHold = usedHold;

            FallingRate = 700;
            FallingCooldown = FallingRate;
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


        public Vector2 getGhostPieceLocation()
        {
            Vector2 originalLocation = FallingPolyominoLocation;
            Vector2 position = FallingPolyominoLocation;
            while (fallingPolyominoDescend(false))
            {
                position = FallingPolyominoLocation;
            }
            FallingPolyominoLocation = originalLocation;
            return position;
        }


        public void holdPolyomino()
        {
            if (!UsedHold)
            {
                UsedHold = true;
                Polyomino toHold = FallingPolyomino;
                toHold.SRSNormalize();
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
        }

        public bool tryRotateClockwise()
        {
            bool result = true;
            Vector2 tempLocation = FallingPolyominoLocation;
            FallingPolyomino.rotate90C();
            if (!wallKickAfterRotation((FallingPolyomino.rotation + 3) % 4))
            {
                FallingPolyomino.rotate90CC();
                result = false;
            }
            else
            {
                FallingPolyomino.rotate90CC();
                inifinityUpdate(tempLocation);
                FallingPolyomino.rotate90C();
            }
            return result;
        }


        public bool tryRotateCounterClockwise()
        {
            bool result = true;
            Vector2 tempLocation = FallingPolyominoLocation;
            FallingPolyomino.rotate90CC();
            if (!wallKickAfterRotation(4 + ((7 - FallingPolyomino.rotation) % 4)))
            {
                FallingPolyomino.rotate90C();
                result = false;
            }
            else
            {
                FallingPolyomino.rotate90C();
                inifinityUpdate( tempLocation );
                FallingPolyomino.rotate90CC();
            }
            return result;
        }

        private void inifinityUpdate( Vector2 locationToTry )
        {
            Vector2 temp = FallingPolyominoLocation;
            if (!tryFallingPosition(locationToTry + new Vector2(0, -1)))
            {
                if (InfinityCount > 0)
                {
                    InfinityCount--;
                    Console.WriteLine(InfinityCount);
                    FallingCooldown = FallingRate;                    
                }
            }
            FallingPolyominoLocation = temp;

        }

        public bool wallKickAfterRotation(int rot)
        {
            foreach (Vector2 p in WallKickStrategy.getStrategyCopy(FallingPolyomino.Minos.Count, rot, FallingPolyomino.isIPolyomino()))
            {
                if (tryFallingPosition(p + FallingPolyominoLocation))
                {
                    return true;
                }
            }
            return false;
        }

        public bool tryFallingPosition(Vector2 v)
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

        public bool fallingPolyominoDescend(bool replaceIfInvalid)
        {
            if (!tryFallingPosition(new Vector2(FallingPolyominoLocation.X, FallingPolyominoLocation.Y - 1)))
            {
                if (replaceIfInvalid)
                {
                    lock (SettledMinos)
                    {
                        foreach (Vector2 v in FallingPolyomino.Minos)
                        {
                            Mino toAdd = new Mino(new Vector2(v.X + FallingPolyominoLocation.X, v.Y + FallingPolyominoLocation.Y), FallingPolyomino);
                            SettledMinos.Add(toAdd);
                        }
                    }
                    tryClearLines();

                    getNextPolyomino();
                }
                return false;
            }
            return true;
        }      

        public void tryClearLines()
        {
            // use falling polyomino and its location
            tryClearLines(0, Height - 1);
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
                lock (SettledMinos)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        SettledMinos.Remove(new Mino(new Vector2(j, i), FallingPolyomino));
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
                        SettledMinos.Add(new Mino(new Vector2(m.v.X, m.v.Y - 1), m.p));
                    }
                }
                return true;
            }
            return false;
        }

        private bool loadPolyomino(Polyomino p)
        {
            FallingPolyomino = p;
            if (!tryFallingPosition(new Vector2(n + (n / 2) / 2 - 1, Height - n / 2 - 1)))
            {
                GameOver = true;
                return false;
            }
            p.SRSNormalize();

            InfinityCount = maxInfinity;
            tryClearLines();
            return true;
        }

        public bool getNextPolyomino()
        {
            bool result = false;
            Polyomino p = MyPieceDealer.getNextPolyomino();
            if (loadPolyomino(p))
            {
                result = true;
                UsedHold = false;
            }

            return result ;
        }



        public int ProcessMove(int k)
        {

            switch (k)
            {
                case (int)Constants.Moves.LEFT:
                    tryFallingPosition(new Vector2(FallingPolyominoLocation.X - 1, FallingPolyominoLocation.Y));
                    break;
                case (int)Constants.Moves.ROTC:
                    tryRotateClockwise();
                    break;
                case (int)Constants.Moves.ROTCC:
                    tryRotateCounterClockwise();
                    break;
                case (int)Constants.Moves.RIGHT:
                    tryFallingPosition(new Vector2(FallingPolyominoLocation.X + 1, FallingPolyominoLocation.Y));
                    break;
                case (int)Constants.Moves.HARD_DROP:
                    while (fallingPolyominoDescend(true)) ;
                    break;
                case (int)Constants.Moves.SOFT_DROP:
                    if (fallingPolyominoDescend(false))
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                case (int)Constants.Moves.HOLD:
                    holdPolyomino();
                    break;
            }

            return 0;
        }


        internal void passTime(int p)
        {
            if (FallingPolyomino == null)
            {
                if (!getNextPolyomino())
                {
                    GameOver = false;
                }
                FallingCooldown = FallingRate;
            }
            else
            {
                FallingCooldown -= p;
            }
            if (FallingCooldown <= 0)
            {
                fallingPolyominoDescend(true);
                FallingCooldown = FallingRate;
            }
        }

        public GameBoardData clone()
        {
            HashSet<Mino> minos = new HashSet<Mino>();
            foreach ( Mino m in this.SettledMinos )
            {
                //todo really slow
                minos.Add( new Mino( m.v, null ) );
            }

            return new GameBoardData(minos, FallingPolyomino.Clone(), FallingPolyominoLocation, n, Width, Height, WallKickStrategy, MyPieceDealer.Clone(), UsedHold );
        }
    }

    
}

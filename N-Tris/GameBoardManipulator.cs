using N_TrisNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N_Tris
{
    public class GameBoardManipulator
    {
        public static WallKickStrategy wallKickStrategy = new WallKickStrategy();

        public GameBoardData Data;
        public PolyominoDealer MyPieceDealer { get; set; }

        public GameBoardManipulator( int n ) : this( GameBoardData.GetNewGameData(n), new PolyominoDealer(n) )
        {
        }

        public GameBoardManipulator(GameBoardData newData, PolyominoDealer polyominoDealer)
        {
            this.Data = newData;
            this.MyPieceDealer = polyominoDealer;

            for (int i = 0; i < 5; i++)
            {
                this.Data.PolyominoQueue.AddLast(MyPieceDealer.getNextPolyomino());
            }
        }


        public bool isLineFull(int i)
        {
            for (int j = 0; j < Data.Width; j++)
            {
                if (!Data.SettledMinoLocations.Contains(new Vector2(j, i)))
                {
                    return false;
                }
            }
            return true;
        }


        public Vector2 getGhostPieceLocation()
        {
            Vector2 originalLocation = Data.FallingPolyominoLocation;
            Vector2 position = Data.FallingPolyominoLocation;
            while (FallingPolyominoDescend(false))
            {
                position = Data.FallingPolyominoLocation;
            }
            Data.FallingPolyominoLocation = originalLocation;
            return position;
        }


        public void holdPolyomino()
        {
            if (!Data.UsedHold)
            {
                Data.UsedHold = true;
                Polyomino toHold = Data.FallingPolyomino;
                toHold.SRSNormalize();
                if (Data.HeldPolyomino != null)
                {
                    loadPolyomino(Data.HeldPolyomino);
                }
                else
                {
                    getNextPolyomino();
                }
                Data.HeldPolyomino = toHold;
            }
        }

        public bool tryRotateClockwise()
        {
            bool result = true;
            Vector2 tempLocation = Data.FallingPolyominoLocation;
            Data.FallingPolyomino.rotate90C();
            if (!wallKickAfterRotation((Data.FallingPolyomino.rotation + 3) % 4))
            {
                Data.FallingPolyomino.rotate90CC();
                result = false;
            }
            else
            {
                Data.FallingPolyomino.rotate90CC();
                inifinityUpdate(tempLocation);
                Data.FallingPolyomino.rotate90C();
            }
            return result;
        }


        public bool tryRotateCounterClockwise()
        {
            bool result = true;
            Vector2 tempLocation = Data.FallingPolyominoLocation;
            Data.FallingPolyomino.rotate90CC();
            if (!wallKickAfterRotation(4 + ((7 - Data.FallingPolyomino.rotation) % 4)))
            {
                Data.FallingPolyomino.rotate90C();
                result = false;
            }
            else
            {
                Data.FallingPolyomino.rotate90C();
                inifinityUpdate( tempLocation );
                Data.FallingPolyomino.rotate90CC();
            }
            return result;
        }

        private void inifinityUpdate( Vector2 locationToTry )
        {
            Vector2 temp = Data.FallingPolyominoLocation;
            if (!tryFallingPosition(locationToTry + new Vector2(0, -1)))
            {
                if (Data.InfinityCount > 0)
                {
                    Data.InfinityCount--;
                    Console.WriteLine(Data.InfinityCount);
                    Data.FallingCooldown = GameBoardData.FallingRate;                    
                }
            }
            Data.FallingPolyominoLocation = temp;

        }

        public bool wallKickAfterRotation(int rot)
        {
            foreach (Vector2 p in wallKickStrategy.getStrategyCopy(Data.FallingPolyomino.Minos.Count, rot, Data.FallingPolyomino.isIPolyomino()))
            {
                if (tryFallingPosition(p + Data.FallingPolyominoLocation))
                {
                    return true;
                }
            }
            return false;
        }

        public bool tryFallingPosition(Vector2 v)
        {
            foreach (Vector2 p in Data.FallingPolyomino.Minos)
            {
                Vector2 test = new Vector2(v.X + p.X, v.Y + p.Y);
                if ((test.X < 0 || test.Y < 0) ||
                     (test.X >= Data.Width || test.Y >= Data.Height) ||
                     (Data.SettledMinoLocations.Contains( test )))
                {
                    return false;
                }
            }
            Data.FallingPolyominoLocation = v;
            return true;
        }

        public bool FallingPolyominoDescend(bool replaceIfInvalid)
        {
            if (!tryFallingPosition(new Vector2(Data.FallingPolyominoLocation.X, Data.FallingPolyominoLocation.Y - 1)))
            {
                if (replaceIfInvalid)
                {
                    lock (Data.SettledMinoLocations)
                    {
                        foreach (Vector2 v in Data.FallingPolyomino.Minos)
                        {
                            Vector2 toAdd = new Vector2(v.X + Data.FallingPolyominoLocation.X, v.Y + Data.FallingPolyominoLocation.Y);
                            Data.SettledMinoLocations.Add(toAdd);
                            Data.SettledMinoColors[toAdd] = Data.FallingPolyomino.color;
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
            tryClearLines(0, Data.Height - 1);
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
                lock (Data.SettledMinoLocations)
                {
                    for (int j = 0; j < Data.Width; j++)
                    {
                        Data.SettledMinoLocations.Remove( new Vector2(j, i) );
                        Data.SettledMinoColors.Remove( new Vector2(j, i) );
                    }
                    HashSet<Vector2> toDec = new HashSet<Vector2>();
                    Dictionary<Vector2, int> toDecColor = new Dictionary<Vector2, int>();
                    foreach (Vector2 m in Data.SettledMinoLocations)
                    {
                        if (m.Y > i)
                        {
                            toDec.Add(m);
                            toDecColor[m] = Data.SettledMinoColors[m];
                        }
                    }
                    foreach (Vector2 m in toDec)
                    {
                        Data.SettledMinoLocations.Remove(m);
                        Data.SettledMinoColors.Remove(m);
                    }
                    foreach (Vector2 m in toDec)
                    {
                        Data.SettledMinoLocations.Add( new Vector2(m.X, m.Y - 1));
                        Data.SettledMinoColors[new Vector2(m.X, m.Y - 1)] = toDecColor[m];
                    }
                }
                return true;
            }
            return false;
        }

        private bool loadPolyomino(Polyomino p)
        {
            Data.FallingPolyomino = p;
            if (!tryFallingPosition(new Vector2(Data.n + (Data.n / 2) / 2 - 1, Data.Height - Data.n / 2 - 1)))
            {
                Data.GameOver = true;
                return false;
            }
            p.SRSNormalize();

            Data.InfinityCount = GameBoardData.maxInfinity;
            tryClearLines();
            return true;
        }

        public bool getNextPolyomino()
        {
            bool result = false;
            Polyomino p = Data.PolyominoQueue.First();
            Data.PolyominoQueue.RemoveFirst();
            Data.PolyominoQueue.AddLast( new LinkedListNode<Polyomino>( MyPieceDealer.getNextPolyomino() ));
            if (loadPolyomino(p))
            {
                result = true;
                Data.UsedHold = false;
            }

            return result ;
        }



        public int ProcessMove(int k)
        {

            switch (k)
            {
                case (int)Constants.Moves.LEFT:
                    tryFallingPosition(new Vector2(Data.FallingPolyominoLocation.X - 1, Data.FallingPolyominoLocation.Y));
                    break;
                case (int)Constants.Moves.ROTC:
                    tryRotateClockwise();
                    break;
                case (int)Constants.Moves.ROTCC:
                    tryRotateCounterClockwise();
                    break;
                case (int)Constants.Moves.RIGHT:
                    tryFallingPosition(new Vector2(Data.FallingPolyominoLocation.X + 1, Data.FallingPolyominoLocation.Y));
                    break;
                case (int)Constants.Moves.HARD_DROP:
                    while (FallingPolyominoDescend(true)) ;
                    break;
                case (int)Constants.Moves.SOFT_DROP:
                    if (FallingPolyominoDescend(false))
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

            Data.GhostPieceLocation = getGhostPieceLocation();
            return 0;
        }


        internal void passTime(int p)
        {
            if (Data.FallingPolyomino == null)
            {
                if (!getNextPolyomino())
                {
                    Data.GameOver = false;
                }
                Data.FallingCooldown = GameBoardData.FallingRate;
            }
            else
            {
                Data.FallingCooldown -= p;
            }
            if (Data.FallingCooldown <= 0)
            {
                FallingPolyominoDescend(true);
                Data.FallingCooldown = GameBoardData.FallingRate;
            }

            Data.GhostPieceLocation = getGhostPieceLocation();
        }

        public GameBoardManipulator Clone()
        {
            GameBoardData newData = this.Data.Clone();
            GameBoardManipulator result = new GameBoardManipulator(newData, this.MyPieceDealer.Clone());
            return result;
        }
    }

    
}

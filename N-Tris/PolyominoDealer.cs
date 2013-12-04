/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N_Tris
{
    class PolyominoDealer
    {
        private List<Polyomino> polyList;

        private int polyListIndex;
        public PolyominoDealer(int n)
        {
            PolyominoGenerator gen = new PolyominoGenerator();
            polyList = gen.getPolyominos(n);
            polyListIndex = 0;
        }

        public Polyomino getNextPolyomino( )
        {
            polyListIndex = polyListIndex % polyList.Count;
            return polyList[polyListIndex++];
        }

    }
}

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N_Tris
{
    class PolyominoDealer
    {
        private List<Polyomino> dealingList;
        private List<Polyomino> backingList;

        public PolyominoDealer(int n)
        {
            PolyominoGenerator gen = new PolyominoGenerator();
            dealingList = gen.getPolyominos(n);
            backingList = new List<Polyomino>();
            foreach (Polyomino p in dealingList)
            {
                backingList.Add(p);
            }
        }

        public Polyomino getNextPolyomino( )
        {
            if (dealingList.Count == 0)
            {
                foreach (Polyomino p in backingList)
                {
                    dealingList.Add(p);
                }
            }
            Random r = new Random();

            int x = r.Next(dealingList.Count);
            Polyomino temp = dealingList[x];
            dealingList[x] = dealingList[dealingList.Count - 1];

            dealingList.RemoveAt(dealingList.Count - 1);


            temp.SRSNormalize();


            //foreach (Vector2 p in temp.Minos)
            //{
            //    int xx = p.X;
            //    int yy = p.Y;
            //    Console.Write("(" + p.X + ", " + p.Y + "), ");
            //}
            //Console.WriteLine(temp.Minos.GetHashCode());

            return temp;
        }

    }
}

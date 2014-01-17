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
    public class PolyominoDealer
    {
        Random r = new Random(Guid.NewGuid().GetHashCode());

        private LinkedList<Polyomino> dealingList;
        private List<Polyomino> backingList;

        public PolyominoDealer(int n)
        {
            PolyominoGenerator gen = new PolyominoGenerator();
            dealingList = new LinkedList<Polyomino>( );

            backingList = gen.getPolyominos( n );
        }

    public PolyominoDealer(LinkedList<Polyomino> dealingList,List<Polyomino> backingList,Random r)
    {
        this.dealingList = new LinkedList<Polyomino>( dealingList );
        this.backingList = new List<Polyomino>( backingList );
        this.r = r;
    }
        public Polyomino getNextPolyomino()
        {
            Polyomino p = peekNextPolyominos(1)[0];
            dealingList.RemoveFirst();
            return p;
        }



        private void addBag( )
        {
            List<Polyomino> toShuffle = new List<Polyomino>();
            foreach( Polyomino p in backingList ) toShuffle.Add( p );

            while ( toShuffle.Count > 0 )
            {
                dealingList.AddLast( removeAtRandom( toShuffle  ) );
            }

        }



        public Polyomino removeAtRandom( List<Polyomino> theList )
        {
            int x = r.Next(theList.Count);
            Polyomino temp = theList[x];
            theList[x] = theList[theList.Count - 1];

            theList.RemoveAt(theList.Count - 1);


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


        public List<Polyomino> peekNextPolyominos(int n)
        {
            List<Polyomino> ret = new List<Polyomino>();

            while (dealingList.Count < n)
            {
                addBag();
            }

            var enumd = dealingList.GetEnumerator();
            for (int i = 0; i < n; i++)
            {
                enumd.MoveNext();
                // clone so don't have access to internal state
                ret.Add(enumd.Current.Clone());
            }
            return ret;
        }

        internal PolyominoDealer Clone()
        {
            return new PolyominoDealer( dealingList, backingList, r );
        }
    }
}

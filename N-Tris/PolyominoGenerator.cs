using N_Tris.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ExtensionMethods;

namespace N_Tris
{
    class PolyominoGenerator
    {
        private Dictionary<int, HashSet<HashBitArray>> polyominoList;

        public PolyominoGenerator()
        {
            polyominoList = new Dictionary<int, HashSet<HashBitArray>>();
        }

        public void generatePolyominos(int n)
        {
            if (!polyominoList.ContainsKey(n))
            {
                HashSet<HashBitArray> toAdd = new HashSet<HashBitArray>();
                if (n == 1)
                {
                    HashBitArray b = new HashBitArray(1);
                    b[0] = true;
                    toAdd.Add(b);
                }
                else
                {
                    generatePolyominos(n - 1);
                    foreach (HashBitArray prev in polyominoList[n - 1])
                    {
                        Polyomino prevPoly = new Polyomino(prev, n - 1);
                        foreach (Vector2 adj in prevPoly.getAdj())
                        {
                            Polyomino next = new Polyomino(prevPoly, adj);
                            HashBitArray nextBits = next.toBitArray();

                            Boolean isNew = !toAdd.Contains(nextBits);
                            for (int i = 0; i < 3 && isNew; i++)
                            {
                                next.rotate90CC();
                                nextBits = next.toBitArray();
                                isNew = !toAdd.Contains(nextBits);
                            }
                            if (isNew)
                            {
                                toAdd.Add(nextBits);
                            }
                        }
                    }
                }
                polyominoList[n] = toAdd;
            }

        }


        internal List<Polyomino> getPolyominos(int n)
        {
            generatePolyominos(n);
            List<Polyomino> ret = new List<Polyomino>();
            foreach (HashBitArray h in polyominoList[n])
            {
                Polyomino p = new Polyomino(h, n);
                p.SRSNormalize();
                ret.Add(p);
            }

            PolyominoColorChooser.assignColors(ret);

            return ret;
        }
    }
}
/*



using N_Tris.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ExtensionMethods;

namespace N_Tris
{
   class PolyominoGenerator
   {
       private List<HashSet<HashBitArray>> minoList;
       private List<List<Polyomino>> polyominoList;

       public PolyominoGenerator( int n )
       {
           // generate all the polyominos less than n
           minoList = new List<HashSet<HashBitArray>>();
           polyominoList = new List<List<Polyomino>>();

           // generate for 0
           HashBitArray monominos = new HashBitArray( 1 * 1 );
           monominos[0]= true;
           HashSet<HashBitArray> minominoSet = new HashSet<HashBitArray>();
           minominoSet.Add( monominos );
           minoList.Add( minominoSet );
           List<Polyomino> monoList = new List<Polyomino>();
           monoList.Add(new Polyomino(monominos, 1));
           polyominoList.Add(monoList);

            

           Console.WriteLine(minominoSet.Count);

            

           for (int i = 2; i <= n; i++ )
           {
               // generate all polyominos of this size
               HashSet<HashBitArray> iominoSet = new HashSet<HashBitArray>();
                
               foreach (HashBitArray prevMino in minoList.Last() )
               {
                   addOneToMino( prevMino, iominoSet, i );
               }
               List<Polyomino> polys = new List<Polyomino>();
               foreach ( HashBitArray p in iominoSet )
               {
                   //convert to polyomino
                   polys.Add( new Polyomino( p, i ) );
               }
               polyominoList.Add(polys);

               minoList.Add(iominoSet);

               Console.WriteLine(iominoSet.Count);
           }

       }

       private void addOneToMino(HashBitArray prev, HashSet<HashBitArray> next, int iter)
       {
           int[,] dirs = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
           for (int i = 0; i < iter; i++)
           {
               for (int j = 0; j < iter; j++)
               {
                   // add one here and see what happens. 
                   //validate
                   if ( ( i >= ( iter -1 ) || j >= ( iter -1 ) ) || !( prev[ i * (iter-1) + j ] ) )
                   {
                       bool adjFull = false;
                       for (int dirn = 0; dirn < 4; dirn++ )
                       {
                           int x = i + dirs[dirn,0];
                           int y = j + dirs[dirn,1];
                           if (( x >= 0 ) && ( y >= 0 ) && ( x < (iter - 1) ) && ( y < (iter - 1) ) && prev[ x * (iter-1) + y])
                           {
                               adjFull = true;
                           }
                       }
                       if (adjFull)
                       {
                           // valid spot to try and add a block.
                           // copy into more usable list //TODO slow (not algorithmically)
                           List<Vector> afterAdd = new List<Vector>();
                           for (int k = 0; k < iter; k++)
                           {
                               for (int l = 0; l < iter; l++)
                               {
                                   if ( (k < (iter-1) && l < (iter-1)) && prev[k * (iter - 1) + l] )
                                   {
                                       afterAdd.Add(new Vector(l, k));
                                   }
                               }
                           }
                           // mark our new block
                           afterAdd.Add(new Vector(j, i));

                           addPolyominoToSet(afterAdd, iter, next);

                       }
                   }
               }
           }
       }

       private void addPolyominoToSet(List<Vector> curr, int n, HashSet<HashBitArray> polys)
       {
           // make sure all four rotations aren't in the set
           if ( polys.Contains( convertPointsToBitArray(curr, n) ) )
           {
               return;
           }
           // con
           // rotate 3 times 90 degrees counter clockwise
           for ( int c = 0; c < 3; c++ )
           {
               List<Vector> nextRot = new List<Vector>();

               foreach (Vector p in curr)
               {
                   nextRot.Add( new Vector( -p.Y, p.X ) );
               }


               int minx = int.MaxValue;
               int miny = int.MaxValue;
               // calculate min values
               foreach (Vector p in nextRot)
               {
                   minx = Math.Min(minx, p.X);
                   miny = Math.Min(miny, p.Y);
               }

               // shift back to first coordinate
               for ( int i = 0; i < nextRot.Count; i++ )
               {
                   Vector p = nextRot[i];
                   p.X += -minx;
                   p.Y += -miny;
               }

               // now check if it exists in set
               curr = nextRot;
               // get bit array, if already exists terminate
               if (polys.Contains(convertPointsToBitArray(curr, n)))
               {
                   return;
               }

           }
           polys.Add(convertPointsToBitArray(curr, n));            

           return;
       }

       private HashBitArray convertPointsToBitArray(List<Vector> a, int n)
       {
           HashBitArray ret = new HashBitArray(n * n);
           foreach (Vector p in a)
           {
               ret[p.Y * n + p.X] = true;
           }
           return ret;
       }

       public List<Polyomino> getPolyominos(int n)
       {
           return polyominoList[n - 1];
       }
   }
}
*/
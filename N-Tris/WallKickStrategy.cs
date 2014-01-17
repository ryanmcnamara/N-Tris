using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris
{

    /**
        * Attempts to generalize the SRS wall kick strategy
        * @author ryanmcnamara
        *
        */
    public class WallKickStrategy
    {

        private Dictionary<int, List<List<Vector2>>> defaultMinoTables;
        private Dictionary<int, List<List<Vector2>>> iMinoTables;

        public WallKickStrategy()
        {
            defaultMinoTables = new Dictionary<int, List<List<Vector2>>>();
            iMinoTables = new Dictionary<int, List<List<Vector2>>>();
        }

        private void generateWallKickTable(int n)
        {
            if (defaultMinoTables.ContainsKey(n))
            {
                // already generated
                return;
            }
            if (n == 1)
            {
                // all 0,0's
                List<List<Vector2>> table = new List<List<Vector2>>();
                List<List<Vector2>> table2 = new List<List<Vector2>>();
                for (int i = 0; i < 8; i++)
                {
                    List<Vector2> list = new List<Vector2>();
                    List<Vector2> list2 = new List<Vector2>();
                    list.Add(new Vector2(0, 0));
                    list2.Add(new Vector2(0, 0));
                    table.Add(list);
                    table2.Add(list);
                }
                defaultMinoTables[n] = table;
                iMinoTables[n] = table2;
                return;
            }

            generateWallKickTable(n - 1);

            // 0-3 inclusive are the clockwise rots, 4-7 inclusive are the cclockwise rots
            // copy previous
            List<List<Vector2>> defaultTable = new List<List<Vector2>>();
            List<List<Vector2>> iTable = new List<List<Vector2>>();
            defaultMinoTables[n] = defaultTable;
            iMinoTables[n] = iTable;

            for (int i = 0; i < 8; i++)
            {
                List<Vector2> defaultList = new List<Vector2>();
                List<Vector2> iList = new List<Vector2>();
                defaultTable.Add(defaultList);
                iTable.Add(iList);

                foreach (Vector2 p in defaultMinoTables[n - 1][i])
                {
                    defaultList.Add(new Vector2(p.X, p.Y));
                }
                foreach (Vector2 p in iMinoTables[(n - 1)][(i)])
                {
                    iList.Add(new Vector2(p.X, p.Y));
                }
            }
            // done copy

            // do default table
            for (int rotType = 0; rotType < 4; rotType++)
            {
                List<Vector2> list1 = defaultTable[rotType];
                List<Vector2> list2 = defaultTable[7 - rotType];

                List<Vector2> Vector2sToAdd = new List<Vector2>();

                bool rot180 = (rotType % 2 == 1);
                bool reflectOverY = (rotType >= 2);
                // add n / 2 Vector2s to the n / 2 level
                bool ccRot = (n % 4) < 2;

                // top left when ccRot is false
                if (n % 2 == 0)
                {
                    // left side
                    int x = -n / 2;
                    for (int y = 0; y < n / 2; y++)
                    {
                        int xAdd = x;
                        int yAdd = y;
                        if (ccRot)
                        {
                            xAdd ^= yAdd;
                            yAdd ^= xAdd;
                            xAdd ^= yAdd;

                            xAdd = -xAdd;
                        }
                        if (rot180)
                        {
                            xAdd = -xAdd;
                            yAdd = -yAdd;
                        }
                        if (reflectOverY)
                        {
                            xAdd = -xAdd;
                        }
                        Vector2sToAdd.Add(new Vector2(xAdd, yAdd));
                    }
                }
                else
                {
                    // top sideint x = -n/2;
                    int y = n / 2;
                    for (int x = -n / 2; x < 0; x++)
                    {
                        int xAdd = x;
                        int yAdd = y;

                        if (ccRot)
                        {
                            xAdd ^= yAdd;
                            yAdd ^= xAdd;
                            xAdd ^= yAdd;

                            xAdd = -xAdd;
                        }
                        if (rot180)
                        {
                            xAdd = -xAdd;
                            yAdd = -yAdd;
                        }
                        if (reflectOverY)
                        {
                            xAdd = -xAdd;
                        }
                        Vector2sToAdd.Add(new Vector2(xAdd, yAdd));
                    }
                }

                foreach (Vector2 p in Vector2sToAdd)
                {
                    list1.Add(new Vector2(p.X, p.Y));
                    // rot 180 for the other list
                    list2.Add(new Vector2(-p.X, -p.Y));
                }


            }

            // do i table
            // generate first kick list
            List<Vector2> firstList = iTable[0];
            HashSet<Vector2> used = new HashSet<Vector2>();
            foreach ( Vector2 v in firstList )
            {
                used.Add( v );
            }
            if (n == 3)
            {
                firstList.Add(new Vector2(1, 0));
            }
            else if (n == 2)
            {
                firstList.Add(new Vector2(-1, 0));
            }
            else
            {
                if (n == 4)
                {
                    // fix earlier "mistake"
                    used.Remove(firstList[1]);
                    used.Add(new Vector2(-2, 0));
                    firstList[1] = new Vector2(-2,0);
                }
                // add n/2 points for this list
                int init = (n % 2 == 0) ? 0 : n / 2;
                for (int i = init; i < init + n / 2; i++)
                {
                    int x;
                    int y;
                    if (i % 4 == 0)
                    {
                        // add low and down on left side as close to x=0 as possible.
                        x = -n / 2;
                        y = 0;
                        while (used.Contains(new Vector2(x, y)))
                        {
                            y--;
                        }
                    }
                    else if (i % 4 == 1)
                    {
                        // add low and down on left side as close to x=0 as possible.
                        x = n / 2 - 1;
                        y = n / 2;
                        while (used.Contains(new Vector2(x, y)))
                        {
                            x--;
                        }
                    }
                    else if (i % 4 == 2)
                    {
                        // add up on left side as close to x=0 as possible.
                        x = -n / 2;
                        y = 0;
                        while (used.Contains(new Vector2(x, y)))
                        {
                            y++;
                        }
                    }
                    else  // (i % 4 == 3)
                    {
                        // add to the right side as close to the center as possible.
                        x = n / 2;
                        y = 0;
                        while (used.Contains(new Vector2(x, y)))
                        {
                            y = -y;
                            if (used.Contains(new Vector2(x, y)))
                            {
                                break;
                            }
                            y = -y;
                            y++;
                        }
                    }
                    used.Add(new Vector2(x, y));
                    firstList.Add(new Vector2(x, y));
                }
            }            
            // add range to the other lists
            for (int i = 1; i < 8; i++)
            {
                if (i == 5) continue;
                iTable[i].AddRange(new Vector2[n / 2]);
            }

            // fifth is the same
            iTable[5] = firstList;

            // 7 and 2 is rot 180
            for (int i = 0; i < firstList.Count; i++)
            {
                iTable[7][i] = new Vector2(-firstList[i].X, -firstList[i].Y);
                iTable[2][i] = new Vector2(-firstList[i].X, -firstList[i].Y);
            }

            List<Vector2> secondList = iTable[1];
            //swap every two from the first list
            for (int i = 1; i < secondList.Count - 1; i += 2)
            {
                secondList[i] = firstList[i + 1];
                secondList[i + 1] = firstList[i];
            }

            // reflect over y axis
            for (int i = 0; i < secondList.Count; i++)
            {
                secondList[i] = new Vector2( -secondList[i].X, secondList[i].Y );
            }

            // list 4 is the same as the second list
            iTable[4] = secondList;

            // lists 3 and 6 are the second list rot 180
            for (int i = 0; i < secondList.Count; i++)
            {
                iTable[3][i] = new Vector2(-secondList[i].X, -secondList[i].Y);
                iTable[6][i] = new Vector2(-secondList[i].X, -secondList[i].Y);
            }

        }

        public List<Vector2> getStrategyCopy(int n, int rot, bool isI)
        {
            List<Vector2> ret;
            generateWallKickTable(n);
            if (!isI)
            {
                ret = defaultMinoTables[n][rot];
            }
            else
            {
                ret = iMinoTables[n][rot];
            }

            List<Vector2> retCopy = new List<Vector2>();
            foreach (Vector2 p in ret)
            {
                retCopy.Add(new Vector2(p.X, p.Y));
            }
            return retCopy;
        }
    }
}

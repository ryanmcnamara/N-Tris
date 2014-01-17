using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris.AI
{
    class HoleFitness : FitnessEvaluator
    {
        public int evaluate(GameBoardData d)
        {
            int maxHeight = 0;
            int numHoles = 0;

            // pieceseen for each column
            bool[] pieceSeen = new bool[d.Width];
            int[] maxHeights = new int[d.Width];

            for (int h = d.Height - 1; h >= 0; h--)
            {
                for (int x = 0; x < d.Width; x++)
                {
                    if ( d.SettledMinos.Contains(new Mino(new Vector2(x, h), null)))
                    {
                        maxHeight = Math.Max(maxHeight, h);
                        if (!pieceSeen[x])
                        {
                            maxHeights[x] = h + 1;
                        }
                        pieceSeen[x] = true;
                    }
                    else if ( pieceSeen[x] )
                    {
                        numHoles++;
                    }
                }
            }
            int diffs = 0;
            int bigDiff = 0;
            for ( int i = 1 ; i < maxHeights.Count(); i++ )
            {
                int thisDiff = Math.Abs( maxHeights[i] - maxHeights[i-1] );
                bigDiff = Math.Max(bigDiff, thisDiff);
                diffs += thisDiff;
            }
            //diffs -= bigDiff;

            int heightScore = -maxHeight * maxHeight * maxHeight + (d.Height / 2 * d.Height / 2 * d.Height / 2 );
            if ( maxHeight < d.Height / 2 )
            {
                heightScore = 0;
            }

            int holeScore = 30 * -numHoles;

            int holdScore = (d.UsedHold) ? -5 : 0;

            return holdScore + heightScore + holeScore - diffs;


        }
    }
}

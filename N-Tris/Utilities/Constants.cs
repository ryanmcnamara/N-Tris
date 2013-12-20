using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris
{
    class Constants
    {
        public enum Moves { SOFT_DROP, HARD_DROP, LEFT, RIGHT, ROTC, ROTCC, HOLD };
        public static readonly int[,] orthogonalDirs = { { 1, 0 }, { -1, 0 }, { 0, -1 }, { 0, 1 } };
    }
}

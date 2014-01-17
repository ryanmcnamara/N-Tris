using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris.AI
{
    interface FitnessEvaluator
    {
       int evaluate(GameBoardData d);
    }
}

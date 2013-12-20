using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris
{
    static class BoardChangedEvent
    {
        public static event EventHandler<GameBoardData> boardChanged;

        public static void fire(Object s, GameBoardData payload)
        {
            boardChanged(s, payload);
        }
    }
}

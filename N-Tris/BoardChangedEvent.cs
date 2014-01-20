using N_TrisNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris
{
    public class BoardChangedEvent
    {
        public event EventHandler<GameBoardData> boardChanged;

        public void fire(Object s, GameBoardData payload)
        {
            boardChanged(s, payload);
        }
    }
}

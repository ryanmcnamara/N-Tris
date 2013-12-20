using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace N_Tris
{
    class HumanPlayer : GamePlayer
    {
        Dictionary<int, Key> movesToKeys = new Dictionary<int, Key>()
        { 
            { (int)Constants.Moves.HARD_DROP, Key.Space }, 
            { (int)Constants.Moves.LEFT, Key.Left }, 
            { (int)Constants.Moves.RIGHT, Key.Right }, 
            { (int)Constants.Moves.SOFT_DROP, Key.Z }, 
            { (int)Constants.Moves.ROTC, Key.Up }, 
            { (int)Constants.Moves.ROTCC, Key.Down }, 
            { (int)Constants.Moves.HOLD, Key.LeftShift }
        };
        Dictionary<Key, int > keysToMoves = new Dictionary<Key, int>()
        { 
            { Key.Space, (int)Constants.Moves.HARD_DROP }, 
            { Key.Left, (int)Constants.Moves.LEFT }, 
            { Key.Right,(int)Constants.Moves.RIGHT }, 
            { Key.Z, (int)Constants.Moves.SOFT_DROP }, 
            { Key.Up, (int)Constants.Moves.ROTC }, 
            { Key.Down, (int)Constants.Moves.ROTCC }, 
            { Key.LeftShift, (int)Constants.Moves.HOLD }
        };

        ConcurrentBag<int> moves = new ConcurrentBag<int>();


        public HumanPlayer(  )
        {
            this.moves = new ConcurrentBag<int>();

            Application.Current.MainWindow.KeyDown += new System.Windows.Input.KeyEventHandler(getKeyDown); 
        }


        private void getKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int x;
            if ( keysToMoves.TryGetValue( e.Key, out x ) )
            {
                moves.Add( x );
            }
        }

        public override HashSet<int> getMoves(GameBoardManager manager)
        {
            HashSet<int> ret = new HashSet<int>(moves);
            moves = new ConcurrentBag<int>();
            return ret;
        }
    }
}

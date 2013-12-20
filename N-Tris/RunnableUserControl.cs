using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace N_Tris
{
    public interface RunnableUserControl 
    {
        void setUpSimulate();

        void simulateFrame( int millis );


        void addToWindowChangeEvent( EventHandler<UserControl> changeEvent );

        /*
         * 
        
        public EventHandler<UserControl> WindowChangeEvent;
        public EventHandler<UserControl> getWindowChangeEvent()
        {
            return WindowChangeEvent;
        }

        public void setUpSimulate()
        {
        }

        public void simulateFrame( int )
        {
        }
         * 
         * */
    }
}

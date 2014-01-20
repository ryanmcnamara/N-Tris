using N_TrisNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace N_Tris
{
    /// <summary>
    /// Interaction logic for GameBoardRightAccessoriesView.xaml
    /// </summary>
    public partial class GameBoardRightAccessoriesView : UserControl
    {
        private AccessoryDrawer accessoryDrawer;
        public GameBoardRightAccessoriesView( BoardChangedEvent boardChanger, int height = 800 )
        {
            InitializeComponent();
            accessoryDrawer = new AccessoryDrawer();
            AccessoryCanvas.Height = height;
            AccessoryCanvas.Width = (int)( height * 150.0 / 800 );

            AccessoryCanvas.ClipToBounds = true;
            AccessoryCanvas.SnapsToDevicePixels = true;

            boardChanger.boardChanged += BoardChangedEvent_boardChanged;
        }

        private void BoardChangedEvent_boardChanged(object sender, GameBoardData e)
        {
            accessoryDrawer.drawRight(AccessoryCanvas, e );
        }
    }
}

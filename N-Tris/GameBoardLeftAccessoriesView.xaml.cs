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
    /// Interaction logic for GameBoardAccessoriesView.xaml
    /// </summary>
    public partial class GameBoardLeftAccessoriesView : UserControl
    {
        private AccessoryDrawer accessoryDrawer;
        public GameBoardLeftAccessoriesView()
        {
            InitializeComponent();
            accessoryDrawer = new AccessoryDrawer();

            BoardChangedEvent.boardChanged += BoardChangedEvent_boardChanged;
        }

        private void BoardChangedEvent_boardChanged(object sender, GameBoardData e)
        {


            accessoryDrawer.draw(AccessoryCanvas, e );
        }
    }
}

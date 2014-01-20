using N_TrisNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris
{
    class NetworkPlayerView : PlayerView
    {
        private string playerId;
        private IN_TrisServer server;
        private NTrisClientImpl client;

        public NetworkPlayerView(string playerId, IN_TrisServer server, NTrisClientImpl client, int height = 800) : base( height )
        {
            this.playerId = playerId;
            this.server = server;
            this.client = client;

            client.addToReceiveGame(playerId, recData);
        }

        private void recData(object sender, GameBoardData e)
        {
            App.Current.Dispatcher.BeginInvoke( (Action)(()=>
                this.boardChanger.fire( this, e )
            ));
        }


        public override void setUpSimulate()
        {
            // no op
        }

        public override void simulateFrame(int millis)
        {
            // no op
        }

    }
}

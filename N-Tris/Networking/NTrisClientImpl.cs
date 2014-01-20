using N_TrisNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris
{
    public class NTrisClientImpl : IN_TrisClient
    {
        EventHandler<String[]> startCallback;
        EventHandler<String> dataCallback;
        String[] opponents;
        Dictionary<String, EventHandler<GameBoardData>> gameCallbacks = new Dictionary<string, EventHandler<GameBoardData>>();

        public NTrisClientImpl(EventHandler<String[]> startCallback /*DisplayMessageDelegate callback/*EventHandler<String[]> startToCall, EventHandler<String> dataToCall /* todo */)
        {
            this.startCallback = startCallback;
        }


        public void ReceiveGame(string player, GameBoardData data)
        {
            if (gameCallbacks[player] != null)
            {
                gameCallbacks[player](this, data);
            }
        }

        public void StartGame(string[] oppIds)
        {
            opponents = oppIds;
            if (startCallback != null)
            {
                startCallback(this, oppIds);
            }
        }

        public void addToReceiveGame(String opp, EventHandler<GameBoardData> gameDataCallback)
        {
            gameCallbacks[opp] = gameDataCallback;
        }

    }
}


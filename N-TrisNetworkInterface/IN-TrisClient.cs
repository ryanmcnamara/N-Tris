using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace N_TrisNetworkInterface
{
    public interface IN_TrisClient
    {
        [OperationContract(IsOneWay=true)]
        void ReceiveGame( String player, /*todo*/ );

    }
}

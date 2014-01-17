using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace N_TrisNetworkInterface
{
    [ServiceContract(CallbackContract = typeof(IN_TrisClient))]
    public interface IN_TrisServer
    {
        [OperationContract]
        public void LogIn(String id);

        [OperationContract]
        public void SignUpForGame(String id);

        [OperationContract]
        public void postGame(/*todo*/);
    }
}

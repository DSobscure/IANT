using IANTProtocol;
using Photon.SocketServer;
using System.Collections.Generic;
using IANTServer.OperationHandlers;

namespace IANTServer
{
    public class OperationManager
    {
        protected readonly Dictionary<OperationCode, OperationHandler> operationTable;
        protected readonly Peer peer;

        public OperationManager(Peer peer)
        {
            this.peer = peer;
            operationTable = new Dictionary<OperationCode, OperationHandler>()
            {
                { OperationCode.GetConfigurations, new GetConfigurationsHandler(peer) },
                { OperationCode.Login, new LoginHandler(peer) }
            };
        }

        public void Operate(OperationRequest operationRequest)
        {
            OperationCode operationCode = (OperationCode)operationRequest.OperationCode;
            if (operationTable.ContainsKey(operationCode))
            {
                if(!operationTable[operationCode].Handle(operationRequest))
                {
                    Application.Log.ErrorFormat("Operation Error: {0} from {1}", operationCode, peer.Guid);
                }
            }
            else
            {
                Application.Log.ErrorFormat("Unknow Operation: {0} from {1}", operationCode, peer.Guid);
            }
        }
    }
}

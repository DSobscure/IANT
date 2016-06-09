using IANTProtocol;
using Photon.SocketServer;
using System.Collections.Generic;

namespace IANTServer
{
    public abstract class OperationHandler
    {
        protected Peer peer;
        protected OperationHandler(Peer peer)
        {
            this.peer = peer;
        }
        public virtual bool Handle(OperationRequest operationRequest)
        {
            if(!CheckParameter(operationRequest))
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)StatusCode.InvalidParameter,
                    DebugMessage = "Parameter Error"
                };
                Application.Log.ErrorFormat("{0} {1} Parameter Error", peer.Guid, (OperationCode)operationRequest.OperationCode);
                peer.SendOperationResponse(response, new SendParameters());
                return false;
            }
            else
            {
                Application.Log.InfoFormat("{0} do {1}", peer.Guid, (OperationCode)operationRequest.OperationCode);
                return true;
            }
        }
        public abstract bool CheckParameter(OperationRequest operationRequest);
        public void SendError(byte operationCode, StatusCode statusCode, string debugMessage)
        {
            OperationResponse response = new OperationResponse(operationCode)
            {
                ReturnCode = (short)statusCode,
                DebugMessage = debugMessage
            };
            Application.Log.ErrorFormat("On Operation: {0} , Debug Message: {1}", (OperationCode)operationCode, debugMessage);
            peer.SendOperationResponse(response, new SendParameters());
        }
        public void SendResponse(byte operationCode, Dictionary<byte, object> parameter)
        {
            OperationResponse response = new OperationResponse(operationCode, parameter)
            {
                ReturnCode = (short)StatusCode.Correct
            };
            peer.SendOperationResponse(response, new SendParameters());
        }
    }
}

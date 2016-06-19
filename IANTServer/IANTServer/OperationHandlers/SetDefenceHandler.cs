using System.Collections.Generic;
using IANTProtocol;
using Photon.SocketServer;

namespace IANTServer.OperationHandlers
{
    class SetDefenceHandler : OperationHandler
    {
        public SetDefenceHandler(Peer peer) : base(peer)
        { }
        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 2;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                string defenceDataString = (string)operationRequest.Parameters[(byte)SetDefenceParameterCode.DefenceDataString];
                int usedBudget = (int)operationRequest.Parameters[(byte)SetDefenceParameterCode.UsedBudget];
                peer.Player.DefenceDataString = defenceDataString;
                peer.Player.UsedDefenceBudget = usedBudget;
                Application.ServerInstance.SaveDefence(peer.Player.UniqueID, defenceDataString, usedBudget);
                SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>());
                return true;
            }
            else
            {
                SendError(operationRequest.OperationCode, StatusCode.InvalidParameter, "參數錯誤");
                return false;
            }
        }
    }
}

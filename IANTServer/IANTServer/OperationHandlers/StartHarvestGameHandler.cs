using IANTProtocol;
using Photon.SocketServer;
using IANTLibrary;
using System.Collections.Generic;

namespace IANTServer.OperationHandlers
{
    class StartHarvestGameHandler : OperationHandler
    {
        public StartHarvestGameHandler(Peer peer) : base(peer)
        { }
        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 3;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                long harvestFacebookID = (long)operationRequest.Parameters[(byte)HarvestGameParameterCode.HarvestFacebookID];
                int usedCakeNumber = (int)operationRequest.Parameters[(byte)HarvestGameParameterCode.UsedCakeNumber];
                int harvestableCakeNumber = (int)operationRequest.Parameters[(byte)HarvestGameParameterCode.HarvestableCakeNumber];

                if (peer.Player.UseCake(usedCakeNumber))
                {
                    SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>()
                    {
                        { (byte)StartHarvestGameResponseParameterCode.UsedCakeNumber, usedCakeNumber },
                        { (byte)StartHarvestGameResponseParameterCode.HarvestTargetDefenceDataString, Application.ServerInstance.GetDefenceDataString(harvestFacebookID) },
                        { (byte)StartHarvestGameResponseParameterCode.HarvestableCakeNumber, harvestableCakeNumber },
                    });
                    Application.ServerInstance.SavePlayerCakeCount(peer.Player.UniqueID, peer.Player.CakeCount);
                    return true;
                }
                else
                {
                    SendError(operationRequest.OperationCode, StatusCode.PermissionDeny, "蛋糕不夠");
                    return false;
                }
            }
            else
            {
                SendError(operationRequest.OperationCode, StatusCode.InvalidParameter, "參數錯誤");
                return false;
            }
        }
    }
}

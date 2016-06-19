using System.Collections.Generic;
using IANTProtocol;
using Photon.SocketServer;

namespace IANTServer.OperationHandlers
{
    class HarvestGameOverHandler : OperationHandler
    {
        public HarvestGameOverHandler(Peer peer) : base(peer)
        { }
        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 3;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                int finalWave = (int)operationRequest.Parameters[(byte)HarvestGameOverParameterCode.FinalWaveNumber];
                long defenderFacebookID = (long)operationRequest.Parameters[(byte)HarvestGameOverParameterCode.DefenderFacebookID];
                int harvestCount = (int)operationRequest.Parameters[(byte)HarvestGameOverParameterCode.HarvestCount];

                peer.Player.EXP += finalWave;
                peer.Player.CakeCount += harvestCount;
                SendResponse((byte)OperationCode.GameOver, new Dictionary<byte, object>()
                {
                    { (byte)GameOverResponseParameterCode.Level, peer.Player.Level },
                    { (byte)GameOverResponseParameterCode.EXP, peer.Player.EXP }
                });
                Application.ServerInstance.SavePlayerCakeCount(peer.Player.UniqueID, peer.Player.CakeCount);
                Application.ServerInstance.SaveHarvestResult(defenderFacebookID, harvestCount);
                
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

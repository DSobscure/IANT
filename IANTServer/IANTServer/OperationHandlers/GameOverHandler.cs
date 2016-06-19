using System.Collections.Generic;
using IANTProtocol;
using Photon.SocketServer;

namespace IANTServer.OperationHandlers
{
    class GameOverHandler : OperationHandler
    {
        public GameOverHandler(Peer peer) : base(peer)
        { }
        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 4;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                int finalWave = (int)operationRequest.Parameters[(byte)GameOverParameterCode.FinalWaveNumber];
                string distributionMap1 = (string)operationRequest.Parameters[(byte)GameOverParameterCode.NestDistributionMap1];
                string distributionMap2 = (string)operationRequest.Parameters[(byte)GameOverParameterCode.NestDistributionMap2];
                string distributionMap3 = (string)operationRequest.Parameters[(byte)GameOverParameterCode.NestDistributionMap3];
                peer.Player.EXP += finalWave;
                peer.Player.Nests[0].Load3DistributionMap(distributionMap1, distributionMap2, distributionMap3);
                SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>()
                {
                    { (byte)GameOverResponseParameterCode.Level, peer.Player.Level },
                    { (byte)GameOverResponseParameterCode.EXP, peer.Player.EXP }
                });
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

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
            return operationRequest.Parameters.Count == 1;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                int finalWave = (int)operationRequest.Parameters[(byte)GameOverParameterCode.FinalWaveNumber];
                peer.Player.EXP += finalWave;
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

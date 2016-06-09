using Photon.SocketServer;
using IANTLibrary;
using IANTProtocol;
using System.Collections.Generic;

namespace IANTServer.OperationHandlers
{
    class UpgradeNestHandler : OperationHandler
    {
        public UpgradeNestHandler(Peer peer) : base(peer)
        { }

        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 1;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                AntGrowthDirection direction = (AntGrowthDirection)operationRequest.Parameters[(byte)UpgradeNestParameterCode.Direction];
                Nest nest = peer.Player.Nests[0];
                if (nest.UpgradeNest(direction, peer.Player))
                {
                    SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>()
                    {
                        { (byte)UpgradeNestResponseParameterCode.CakeCount, peer.Player.CakeCount },
                        { (byte)UpgradeNestResponseParameterCode.Duration, nest.GrowthProperties.duration },
                        { (byte)UpgradeNestResponseParameterCode.Speed, nest.GrowthProperties.speed },
                        { (byte)UpgradeNestResponseParameterCode.Resistant, nest.GrowthProperties.resistant },
                        { (byte)UpgradeNestResponseParameterCode.Population, nest.GrowthProperties.population },
                        { (byte)UpgradeNestResponseParameterCode.Sensitivity, nest.GrowthProperties.sensitivity }
                    });
                    return true;
                }
                else
                {
                    SendError(operationRequest.OperationCode, StatusCode.InvalidOperation, "無法升級");
                    return false;
                }
            }
            else
            {
                SendError(operationRequest.OperationCode, StatusCode.InvalidParameter, "參數數量錯誤");
                return false;
            }
        }
    }
}

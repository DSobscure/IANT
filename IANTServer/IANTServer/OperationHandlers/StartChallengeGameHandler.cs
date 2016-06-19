using IANTProtocol;
using Photon.SocketServer;
using IANTLibrary;
using System.Collections.Generic;

namespace IANTServer.OperationHandlers
{
    class StartChallengeGameHandler : OperationHandler
    {
        public StartChallengeGameHandler(Peer peer) : base(peer)
        { }
        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 2;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                long challengeFacebookID = (long)operationRequest.Parameters[(byte)ChallengeGameParameterCode.ChallengeFacebookID];
                int usedCakeNumber = (int)operationRequest.Parameters[(byte)ChallengeGameParameterCode.UsedCakeNumber];
                if (peer.Player.UseCake(usedCakeNumber))
                {
                    Nest nest = Application.ServerInstance.GetNest(challengeFacebookID);
                    string map1, map2, map3;
                    nest.Serialize3DistributionMap(out map1, out map2, out map3);
                    SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>()
                    {
                        { (byte)StartChallengeGameResponseParameterCode.UsedCakeNumber, usedCakeNumber },
                        { (byte)StartChallengeGameResponseParameterCode.NestDuration, nest.GrowthProperties.duration },
                        { (byte)StartChallengeGameResponseParameterCode.NestSpeed, nest.GrowthProperties.speed },
                        { (byte)StartChallengeGameResponseParameterCode.NestResistant, nest.GrowthProperties.resistant },
                        { (byte)StartChallengeGameResponseParameterCode.NestPopulation, nest.GrowthProperties.population },
                        { (byte)StartChallengeGameResponseParameterCode.NestSensitivity, nest.GrowthProperties.sensitivity },
                        { (byte)StartChallengeGameResponseParameterCode.NestDistributionMap1, map1 },
                        { (byte)StartChallengeGameResponseParameterCode.NestDistributionMap2, map2 },
                        { (byte)StartChallengeGameResponseParameterCode.NestDistributionMap3, map3 }
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

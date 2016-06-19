using System.Collections.Generic;
using IANTProtocol;
using Photon.SocketServer;

namespace IANTServer.OperationHandlers
{
    public struct ChallengePlayerInfo
    {
        public long facebookID;
        public int level;
        public int nestLevel;
    }
    class GetChallengePlayerListHandler : OperationHandler
    {
        public GetChallengePlayerListHandler(Peer peer) : base(peer)
        { }
        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 1;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                long[] friendFBIDs = (long[])operationRequest.Parameters[(byte)GetChallengePlayerListParameterCode.FriendsFacebookIDArray];
                ChallengePlayerInfo[] infos = Application.ServerInstance.FetchNChallengePlayerInfoWithKnownFriends(20, friendFBIDs, peer.Player.FacebookID);
                long[] facebookIDs = new long[infos.Length];
                int[] levels = new int[infos.Length];
                int[] nestLevels = new int[infos.Length];
                for(int i = 0; i < infos.Length; i++)
                {
                    facebookIDs[i] = infos[i].facebookID;
                    levels[i] = infos[i].level;
                    nestLevels[i] = infos[i].nestLevel;
                }
                SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>()
                {
                    { (byte)GetChallengePlayerListResponseParameterCode.FacebookIDArray, facebookIDs },
                    { (byte)GetChallengePlayerListResponseParameterCode.LevelArray, levels },
                    { (byte)GetChallengePlayerListResponseParameterCode.NestLevelArray, nestLevels }
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

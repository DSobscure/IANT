using System.Collections.Generic;
using IANTProtocol;
using Photon.SocketServer;

namespace IANTServer.OperationHandlers
{
    public struct HarvestPlayerInfo
    {
        public long facebookID;
        public int level;
        public int harvestableCakeNumber;
    }
    class GetHarvestPlayerListHandler : OperationHandler
    {
        public GetHarvestPlayerListHandler(Peer peer) : base(peer)
        { }
        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 1;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                long[] friendFBIDs = (long[])operationRequest.Parameters[(byte)GetHarvestPlayerListParameterCode.FriendsFacebookIDArray];
                HarvestPlayerInfo[] infos = Application.ServerInstance.FetchNHarvestPlayerInfoWithKnownFriends(20, friendFBIDs, peer.Player.FacebookID);
                long[] facebookIDs = new long[infos.Length];
                int[] levels = new int[infos.Length];
                int[] harvestableCakeNumbers = new int[infos.Length];
                for (int i = 0; i < infos.Length; i++)
                {
                    facebookIDs[i] = infos[i].facebookID;
                    levels[i] = infos[i].level;
                    harvestableCakeNumbers[i] = infos[i].harvestableCakeNumber;
                }
                SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>()
                {
                    { (byte)GetHarvestPlayerListResponseParameterCode.FacebookIDArray, facebookIDs },
                    { (byte)GetHarvestPlayerListResponseParameterCode.LevelArray, levels },
                    { (byte)GetHarvestPlayerListResponseParameterCode.HarvestableCakeNumberArray, harvestableCakeNumbers }
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

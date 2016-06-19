using IANTProtocol;
using System.Collections.Generic;

public class FetchDataActionManager
{
    public void GetConfigurations()
    {
        var parameter = new Dictionary<byte, object>();
        IANTGame.Service.SendOperation(OperationCode.GetConfigurations, parameter);
    }
    public void GetChallengePlayerList(long[] facebookIDArray)
    {
        var parameter = new Dictionary<byte, object>()
        {
            { (byte)GetChallengePlayerListParameterCode.FriendsFacebookIDArray, facebookIDArray }
        };
        IANTGame.Service.SendOperation(OperationCode.GetChallengePlayerList, parameter);
    }
    public void GetHarvestPlayerList(long[] facebookIDArray)
    {
        var parameter = new Dictionary<byte, object>()
        {
            { (byte)GetHarvestPlayerListParameterCode.FriendsFacebookIDArray, facebookIDArray }
        };
        IANTGame.Service.SendOperation(OperationCode.GetHarvestPlayerList, parameter);
    }
}

using ExitGames.Client.Photon;
using IANTProtocol;

public class GetChallengePlayerListResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if (base.Handle(operationResponse))
        {
            long[] facebookIDs = (long[])operationResponse.Parameters[(byte)GetChallengePlayerListResponseParameterCode.FacebookIDArray];
            int[] levels = (int[])operationResponse.Parameters[(byte)GetChallengePlayerListResponseParameterCode.LevelArray];
            int[] nestLevels = (int[])operationResponse.Parameters[(byte)GetChallengePlayerListResponseParameterCode.NestLevelArray];
            ChallengePlayerInfo[] infos = new ChallengePlayerInfo[facebookIDs.Length];
            for(int i = 0; i < facebookIDs.Length; i++)
            {
                infos[i].facebookID = facebookIDs[i];
                infos[i].level = levels[i];
                infos[i].nestLevel = nestLevels[i];
            }
            IANTGame.ResponseManager.FetchDataResponseManager.CallGetChallengePlayerListResponse(infos);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool CheckError(OperationResponse operationResponse)
    {
        OperationCode operationCode = (OperationCode)operationResponse.OperationCode;
        if (operationResponse.ReturnCode != (short)IANTProtocol.StatusCode.Correct)
        {
            return false;
        }
        if (operationResponse.Parameters.Count != 3)
        {
            return false;
        }
        return true;
    }
}

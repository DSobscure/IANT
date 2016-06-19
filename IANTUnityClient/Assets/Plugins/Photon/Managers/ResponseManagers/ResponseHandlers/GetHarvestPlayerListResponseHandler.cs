using ExitGames.Client.Photon;
using IANTProtocol;

public class GetHarvestPlayerListResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if (base.Handle(operationResponse))
        {
            long[] facebookIDs = (long[])operationResponse.Parameters[(byte)GetHarvestPlayerListResponseParameterCode.FacebookIDArray];
            int[] levels = (int[])operationResponse.Parameters[(byte)GetHarvestPlayerListResponseParameterCode.LevelArray];
            int[] harvestableCakeNumbers = (int[])operationResponse.Parameters[(byte)GetHarvestPlayerListResponseParameterCode.HarvestableCakeNumberArray];
            HarvestPlayerInfo[] infos = new HarvestPlayerInfo[facebookIDs.Length];
            for(int i = 0; i < facebookIDs.Length; i++)
            {
                infos[i].facebookID = facebookIDs[i];
                infos[i].level = levels[i];
                infos[i].harvestableCakeNumber = harvestableCakeNumbers[i];
            }
            IANTGame.ResponseManager.FetchDataResponseManager.CallGetHarvestPlayerListResponse(infos);
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

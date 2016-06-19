using ExitGames.Client.Photon;
using IANTProtocol;
using IANTLibrary;

public class StartHarvestGameResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if (base.Handle(operationResponse))
        {
            int usedCakeNumber = (int)operationResponse.Parameters[(byte)StartHarvestGameResponseParameterCode.UsedCakeNumber];
            string harvestDefenceDataString = (string)operationResponse.Parameters[(byte)StartHarvestGameResponseParameterCode.HarvestTargetDefenceDataString];
            int harvestableCakeNumber = (int)operationResponse.Parameters[(byte)StartHarvestGameResponseParameterCode.HarvestableCakeNumber];

            IANTGame.ResponseManager.OperationResponseManager.CallStartHarvestGameResponse(usedCakeNumber, harvestDefenceDataString, harvestableCakeNumber);
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

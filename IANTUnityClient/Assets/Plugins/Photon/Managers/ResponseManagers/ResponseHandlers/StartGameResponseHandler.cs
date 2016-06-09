using ExitGames.Client.Photon;
using IANTProtocol;
using IANTLibrary;

public class StartGameResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if (base.Handle(operationResponse))
        {
            int usedCakeNumber = (int)operationResponse.Parameters[(byte)StartGameResponseParameterCode.UsedCakeNumber];
            IANTGame.ResponseManager.OperationResponseManager.CallStartGameResponse(usedCakeNumber);
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
        if (operationResponse.Parameters.Count != 1)
        {
            return false;
        }
        return true;
    }
}

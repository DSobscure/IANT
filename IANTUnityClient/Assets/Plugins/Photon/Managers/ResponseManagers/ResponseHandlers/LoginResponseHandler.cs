using ExitGames.Client.Photon;
using IANTProtocol;

public class LoginResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if(base.Handle(operationResponse))
        {
            IANTGame.InformManager.SystemInformManager.CallDebugReturn("login successiful");
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
        if(operationResponse.ReturnCode != (short)IANTProtocol.StatusCode.Correct)
        {
            return false;
        }
        if (operationResponse.Parameters.Count != 0)
        {
            return false;
        }
        return true;
    }
}

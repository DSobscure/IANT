using ExitGames.Client.Photon;
using IANTProtocol;

public abstract class ResponseHandler
{
    public virtual bool Handle(OperationResponse operationResponse)
    {
        if (!CheckError(operationResponse))
        {
            IANTGame.InformManager.SystemInformManager.CallDebugReturn(string.Format("Response Error On {0}", (OperationCode)operationResponse.OperationCode));
            return false;
        }
        else
        {
            return true;
        }
    }
    public abstract bool CheckError(OperationResponse operationResponse);
}

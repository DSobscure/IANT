using ExitGames.Client.Photon;
using IANTProtocol;

public class GameOverResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if (base.Handle(operationResponse))
        {
            int level = (int)operationResponse.Parameters[(byte)GameOverResponseParameterCode.Level];
            int exp = (int)operationResponse.Parameters[(byte)GameOverResponseParameterCode.EXP];
            IANTGame.Player.Level = level;
            IANTGame.Player.EXP = exp;
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
        if (operationResponse.Parameters.Count != 2)
        {
            return false;
        }
        return true;
    }
}

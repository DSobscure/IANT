using ExitGames.Client.Photon;
using IANTProtocol;
using System;

public class TakeCakeResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if (base.Handle(operationResponse))
        {
            int cakeCount = (int)operationResponse.Parameters[(byte)TakeCakeResponseParameterCode.CakeCount];
            DateTime lastTime = DateTime.FromBinary((long)operationResponse.Parameters[(byte)TakeCakeResponseParameterCode.LastTakeCakeTime]);
            IANTGame.ResponseManager.OperationResponseManager.CallTakeCakeResponse(cakeCount, lastTime);
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

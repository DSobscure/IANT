using ExitGames.Client.Photon;
using IANTProtocol;
using System.Collections.Generic;
using IANTLibrary;
using System;

public class LoginResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if(base.Handle(operationResponse))
        {
            int uniqueID = (int)operationResponse.Parameters[(byte)LoginResponseParameterCode.UniqueID];
            int level = (int)operationResponse.Parameters[(byte)LoginResponseParameterCode.Level];
            int exp = (int)operationResponse.Parameters[(byte)LoginResponseParameterCode.EXP];
            IANTGame.Player = new IANTLibrary.Player(uniqueID, new PlayerProperties
            {
                facebookID = Convert.ToInt64(IANTGame.FacebookID),
                level = level,
                exp = exp,
                foodInfos = new List<FoodInfo>(),
                nests = new List<Nest>()
            });
            IANTGame.ResponseManager.AuthenticationResponseManager.CallLoginResponse();
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
        if (operationResponse.Parameters.Count != 3)
        {
            return false;
        }
        return true;
    }
}

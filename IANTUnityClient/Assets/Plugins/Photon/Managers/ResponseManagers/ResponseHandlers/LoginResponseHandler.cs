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
            DateTime lastTakeCakeTime = DateTime.FromBinary((long)operationResponse.Parameters[(byte)LoginResponseParameterCode.LastTakeCakeTime]);
            int cakeCount = (int)operationResponse.Parameters[(byte)LoginResponseParameterCode.CakeCount];

            int duration = (int)operationResponse.Parameters[(byte)LoginResponseParameterCode.FirstNestDuration];
            int speed = (int)operationResponse.Parameters[(byte)LoginResponseParameterCode.FirstNestSpeed];
            int resistant = (int)operationResponse.Parameters[(byte)LoginResponseParameterCode.FirstNestResistant];
            int population = (int)operationResponse.Parameters[(byte)LoginResponseParameterCode.FirstNestPopulation];
            int sensitivity = (int)operationResponse.Parameters[(byte)LoginResponseParameterCode.FirstNestSensitivity];

            IANTGame.Player = new IANTLibrary.Player(uniqueID, new PlayerProperties
            {
                facebookID = Convert.ToInt64(IANTGame.FacebookID),
                level = level,
                exp = exp,
                foodInfos = new List<FoodInfo>()
                {
                    new FoodInfo
                    {
                        food = new Cake(),
                        count = cakeCount
                    }
                },
                nests = new List<Nest>()
                {
                    new Nest(new AntGrowthProperties
                    {
                        duration = duration,
                        speed =speed,
                        resistant = resistant,
                        population = population,
                        sensitivity = sensitivity
                    })
                },
                lastTakeCakeTime = lastTakeCakeTime
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
        if (operationResponse.Parameters.Count != 10)
        {
            return false;
        }
        return true;
    }
}

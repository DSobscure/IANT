using ExitGames.Client.Photon;
using IANTProtocol;
using IANTLibrary;

public class StartChallengeGameResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if (base.Handle(operationResponse))
        {
            int usedCakeNumber = (int)operationResponse.Parameters[(byte)StartChallengeGameResponseParameterCode.UsedCakeNumber];
            int duration = (int)operationResponse.Parameters[(byte)StartChallengeGameResponseParameterCode.NestDuration];
            int speed = (int)operationResponse.Parameters[(byte)StartChallengeGameResponseParameterCode.NestSpeed];
            int resistant = (int)operationResponse.Parameters[(byte)StartChallengeGameResponseParameterCode.NestResistant];
            int population = (int)operationResponse.Parameters[(byte)StartChallengeGameResponseParameterCode.NestPopulation];
            int sensitivity = (int)operationResponse.Parameters[(byte)StartChallengeGameResponseParameterCode.NestSensitivity];
            string map1 = (string)operationResponse.Parameters[(byte)StartChallengeGameResponseParameterCode.NestDistributionMap1];
            string map2 = (string)operationResponse.Parameters[(byte)StartChallengeGameResponseParameterCode.NestDistributionMap2];
            string map3 = (string)operationResponse.Parameters[(byte)StartChallengeGameResponseParameterCode.NestDistributionMap3];
            IANTGame.BattleNest = new Nest(new AntGrowthProperties
            {
                duration = duration,
                speed = speed,
                resistant = resistant,
                population = population,
                sensitivity = sensitivity
            });
            IANTGame.BattleNest.Load3DistributionMap(map1, map2, map3);
            IANTGame.ResponseManager.OperationResponseManager.CallStartChallengeGameResponse(usedCakeNumber);
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
        if (operationResponse.Parameters.Count != 9)
        {
            return false;
        }
        return true;
    }
}

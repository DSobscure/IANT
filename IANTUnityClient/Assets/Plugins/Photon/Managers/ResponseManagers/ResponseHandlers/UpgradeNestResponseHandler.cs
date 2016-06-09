using ExitGames.Client.Photon;
using IANTProtocol;
using IANTLibrary;

public class UpgradeNestResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if (base.Handle(operationResponse))
        {
            int cakeCount = (int)operationResponse.Parameters[(byte)UpgradeNestResponseParameterCode.CakeCount];
            int duration = (int)operationResponse.Parameters[(byte)UpgradeNestResponseParameterCode.Duration];
            int speed = (int)operationResponse.Parameters[(byte)UpgradeNestResponseParameterCode.Speed];
            int resistant = (int)operationResponse.Parameters[(byte)UpgradeNestResponseParameterCode.Resistant];
            int population = (int)operationResponse.Parameters[(byte)UpgradeNestResponseParameterCode.Population];
            int sensitivity = (int)operationResponse.Parameters[(byte)UpgradeNestResponseParameterCode.Sensitivity];
            IANTGame.Player.CakeCount = cakeCount;
            IANTGame.Player.Nests[0].GrowthProperties = new AntGrowthProperties
            {
                duration = duration,
                speed = speed,
                resistant = resistant,
                population = population,
                sensitivity = sensitivity
            };
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
        if (operationResponse.Parameters.Count != 6)
        {
            return false;
        }
        return true;
    }
}

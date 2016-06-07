using IANTProtocol;
using System.Collections.Generic;

public class FetchDataActionManager
{
    public void GetConfigurations()
    {
        var parameter = new Dictionary<byte, object>();
        IANTGame.Service.SendOperation(OperationCode.GetConfigurations, parameter);
    }
}

using IANTProtocol;
using System.Collections.Generic;

public class AuthenticateActionManager
{
    public void Login(string facebookUserID, string accessToken)
    {
        var parameter = new Dictionary<byte, object>
        {
            { (byte)LoginParameterCode.FacebookUserID, facebookUserID },
            { (byte)LoginParameterCode.AccessToken, accessToken }
        };
        IANTGame.Service.SendOperation(OperationCode.Login, parameter);
    }
}

using System.Collections.Generic;
using IANTProtocol;
using ExitGames.Client.Photon;

namespace Managers
{
    public class ResponseManager
    {
        protected readonly Dictionary<OperationCode, ResponseHandler> responseTable;
        public readonly SystemResponseManager SystemResponseManager = new SystemResponseManager();
        public readonly AuthenticationResponseManager AuthenticationResponseManager = new AuthenticationResponseManager();
        public readonly FetchDataResponseManager FetchDataResponseManager = new FetchDataResponseManager();

        public ResponseManager()
        {
            responseTable = new Dictionary<OperationCode, ResponseHandler>()
            {
                { OperationCode.Login, new LoginResponseHandler() },
                { OperationCode.GetConfigurations, new GetConfigurationsResponseHandler() }
            };
        }

        public void Operate(OperationResponse operationResponse)
        {
            OperationCode operationCode = (OperationCode)operationResponse.OperationCode;
            if (responseTable.ContainsKey(operationCode))
            {
                responseTable[operationCode].Handle(operationResponse);
            }
            else
            {
                IANTGame.InformManager.SystemInformManager.CallDebugReturn(string.Format("Unknow Response: {0}", operationCode));
            }
        }
    }
}

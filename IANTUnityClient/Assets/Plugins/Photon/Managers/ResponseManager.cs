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
        public readonly OperationResponseManager OperationResponseManager = new OperationResponseManager();

        public ResponseManager()
        {
            responseTable = new Dictionary<OperationCode, ResponseHandler>()
            {
                { OperationCode.Login, new LoginResponseHandler() },
                { OperationCode.GetConfigurations, new GetConfigurationsResponseHandler() },
                { OperationCode.TakeCake, new TakeCakeResponseHandler() },
                { OperationCode.UpgradeNest, new UpgradeNestResponseHandler() },
                { OperationCode.StartGame, new StartGameResponseHandler() },
                { OperationCode.GameOver, new GameOverResponseHandler() },
                { OperationCode.GetChallengePlayerList, new GetChallengePlayerListResponseHandler() },
                { OperationCode.ChallengeGame, new StartChallengeGameResponseHandler() },
                { OperationCode.SetDefence, new SetDefenceResponseHandler() },
                { OperationCode.GetHarvestPlayerList, new GetHarvestPlayerListResponseHandler() },
                { OperationCode.HarvestGame, new StartHarvestGameResponseHandler() }
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

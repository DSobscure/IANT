using IANTProtocol;
using Photon.SocketServer;
using System.Collections.Generic;
using IANTServer.OperationHandlers;

namespace IANTServer
{
    public class OperationManager
    {
        protected readonly Dictionary<OperationCode, OperationHandler> operationTable;
        protected readonly Peer peer;

        public OperationManager(Peer peer)
        {
            this.peer = peer;
            operationTable = new Dictionary<OperationCode, OperationHandler>()
            {
                { OperationCode.GetConfigurations, new GetConfigurationsHandler(peer) },
                { OperationCode.Login, new LoginHandler(peer) },
                { OperationCode.TakeCake, new TakeCakeHandler(peer) },
                { OperationCode.UpgradeNest, new UpgradeNestHandler(peer) },
                { OperationCode.StartGame, new StartGameHandler(peer) },
                { OperationCode.GameOver, new GameOverHandler(peer) },
                { OperationCode.GetChallengePlayerList, new GetChallengePlayerListHandler(peer) },
                { OperationCode.ChallengeGame, new StartChallengeGameHandler(peer) },
                { OperationCode.SetDefence, new SetDefenceHandler(peer) },
                { OperationCode.GetHarvestPlayerList, new GetHarvestPlayerListHandler(peer) },
                { OperationCode.HarvestGame, new StartHarvestGameHandler(peer) },
                { OperationCode.HarvestGameOver, new HarvestGameOverHandler(peer) }
            };
        }

        public void Operate(OperationRequest operationRequest)
        {
            OperationCode operationCode = (OperationCode)operationRequest.OperationCode;
            if (operationTable.ContainsKey(operationCode))
            {
                if(!operationTable[operationCode].Handle(operationRequest))
                {
                    Application.Log.ErrorFormat("Operation Error: {0} from {1}", operationCode, peer.Guid);
                }
            }
            else
            {
                Application.Log.ErrorFormat("Unknow Operation: {0} from {1}", operationCode, peer.Guid);
            }
        }
    }
}

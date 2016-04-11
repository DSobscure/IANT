using IANTLibrary;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;

namespace IANTServer
{
    public class Peer : ClientPeer
    {
        public Guid Guid { get; }
        protected Player player;
        public int PlayerUniqueID
        {
            get
            {
                if (player == null)
                    return -1;
                else
                    return player.UniqueID;
            }
        }
        protected OperationManager operationManager;

        public Peer(InitRequest initRequest) : base(initRequest)
        {
            Guid = Guid.NewGuid();
            operationManager = new OperationManager(this);
            if (Application.ServerInstance.EstablishConnection(this))
            {
                Application.Log.Info(string.Format("establish new connection guid:{0}", Guid));
            }
            else
            {
                Application.Log.Info(string.Format("establish connection fail guid:{0}", Guid));
            }
            
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            if(player != null)
            {
                if (Application.ServerInstance.PlayerOffline(player))
                {
                    Application.Log.Info(string.Format("player offline uniqueID:{0}", player.UniqueID));
                }
                else
                {
                    Application.Log.Info(string.Format("player offline fail uniqueID:{0}", player.UniqueID));
                }
            }
            if(Application.ServerInstance.TerminateConnection(this))
            {
                Application.Log.Info(string.Format("terminate connection guid:{0}", Guid));
            }
            else
            {
                Application.Log.Info(string.Format("terminate connection fail guid:{0}", Guid));
            }
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            operationManager.Operate(operationRequest);
        }
    }
}

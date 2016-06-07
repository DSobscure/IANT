using IANTLibrary;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;

namespace IANTServer
{
    public class Peer : ClientPeer
    {
        public Guid Guid { get; }
        public Player Player { get; protected set; }
        public int PlayerUniqueID
        {
            get
            {
                if (Player == null)
                    return -1;
                else
                    return Player.UniqueID;
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
            if(Player != null)
            {
                if (Application.ServerInstance.PlayerOffline(Player))
                {
                    Application.Log.Info(string.Format("player offline uniqueID:{0}", Player.UniqueID));
                }
                else
                {
                    Application.Log.Info(string.Format("player offline fail uniqueID:{0}", Player.UniqueID));
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
        public void BindPlayer(Player player)
        {
            Player = player;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using ExitGames.Logging;
using IANTLibrary;

namespace IANTServer
{
    public class Peer : PeerBase
    {
        public Guid guid { get; }
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

        public Peer(IRpcProtocol rpcprotocol, IPhotonPeer nativePeer) : base(rpcprotocol,nativePeer)
        {
            guid = Guid.NewGuid();
            if(Application.Instance.EstablishConnection(this))
            {
                Application.Log.Info(string.Format("establish new connection guid:{0}", guid));
            }
            else
            {
                Application.Log.Info(string.Format("establish connection fail guid:{0}", guid));
            }
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            if(player != null)
            {
                if (Application.Instance.PlayerOffline(player))
                {
                    Application.Log.Info(string.Format("player offline uniqueID:{0}", player.UniqueID));
                }
                else
                {
                    Application.Log.Info(string.Format("player offline fail uniqueID:{0}", player.UniqueID));
                }
            }
            if(Application.Instance.TerminateConnection(this))
            {
                Application.Log.Info(string.Format("terminate connection guid:{0}", guid));
            }
            else
            {
                Application.Log.Info(string.Format("terminate connection fail guid:{0}", guid));
            }
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            throw new NotImplementedException();
        }
    }
}

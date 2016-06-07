using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IANTLibrary;

namespace IANTServer
{
    public class ServerPlayer : Player
    {
        protected Peer peer;
        public ServerPlayer(int uniqueID, PlayerProperties properties, Peer peer) : base(uniqueID, properties)
        {
            this.peer = peer;
        }
    }
}

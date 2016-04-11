using System;
using System.Collections.Generic;
using System.Linq;
using IANTLibrary;

namespace IANTServer
{
    public class PlayerManager
    {
        protected Dictionary<Guid, Peer> connections;
        protected Dictionary<int, Player> players;
        protected Dictionary<int, Peer> playerConnectionTable;

        public PlayerManager()
        {
            connections = new Dictionary<Guid, Peer>();
            players = new Dictionary<int, Player>();
            playerConnectionTable = new Dictionary<int, Peer>();
        }

        public bool RegisterConnection(Peer peer)
        {
            if (connections.ContainsKey(peer.Guid))
            {
                return false;
            }
            else
            {
                connections.Add(peer.Guid, peer);
                return true;
            }
        }
        public bool EraseConnection(Peer peer)
        {
            if (connections.ContainsKey(peer.Guid))
            {
                return connections.Remove(peer.Guid);
            }
            else
            {
                return true;
            }
        }

        public bool RegisterPlayer(Player player)
        {
            if (players.ContainsKey(player.UniqueID))
            {
                return false;
            }
            else
            {
                players.Add(player.UniqueID, player);
                Peer peer = connections.Values.First(x => x.PlayerUniqueID == player.UniqueID);
                if (peer == null)
                {
                    return false;
                }
                else
                {
                    playerConnectionTable.Add(player.UniqueID, peer);
                    return true;
                }
            }
        }
        public bool ErasePlayer(Player player)
        {
            if (players.ContainsKey(player.UniqueID))
            {
                return players.Remove(player.UniqueID) && playerConnectionTable.Remove(player.UniqueID);
            }
            else
            {
                return true;
            }
        }
    }
}

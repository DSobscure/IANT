using System;
using System.Collections.Generic;
using System.Linq;
using IANTLibrary;
using Photon.SocketServer;
using IANTProtocol;

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
        public bool Harvested(int uniqueID, int harvestNumber)
        {
            if(players.ContainsKey(uniqueID))
            {
                players[uniqueID].UseCake(Math.Min(harvestNumber, players[uniqueID].CakeCount));
                Application.ServerInstance.SavePlayerCakeCount(uniqueID, players[uniqueID].CakeCount);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void InformCakeNumberChange(int uniqueID, int cakeCount)
        {
            if (playerConnectionTable.ContainsKey(uniqueID))
            {
                EventData eventData = new EventData((byte)BroadcastCode.CakeNumberChange, new Dictionary<byte, object>()
                {
                    {(byte)CakeNumberChangeBroadcastParameterCode.CakeNumber, cakeCount}
                });
                playerConnectionTable[uniqueID].SendEvent(eventData, new SendParameters());
            }
        }
    }
}

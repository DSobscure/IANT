using System.Collections.Generic;
using IANTProtocol;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Managers
{
    public class BroadcastManager
    {
        protected readonly Dictionary<BroadcastCode, BroadcastHandler> broadcastTable;

        public BroadcastManager()
        {
            broadcastTable = new Dictionary<BroadcastCode, BroadcastHandler>()
            {
                { BroadcastCode.CakeNumberChange, new CakeNumberChangeBroadcastHandler() }
            };
        }

        public void Operate(EventData eventData)
        {
            BroadcastCode broadcastCode = (BroadcastCode)eventData.Code;
            Debug.Log(broadcastCode);
            if (broadcastTable.ContainsKey(broadcastCode))
            {
                broadcastTable[broadcastCode].Handle(eventData);
            }
            else
            {
                IANTGame.InformManager.SystemInformManager.CallDebugReturn(string.Format("Unknow Broadcast: {0}", broadcastCode));
            }
        }
    }
}

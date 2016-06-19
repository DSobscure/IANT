using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;
using IANTProtocol;

public class CakeNumberChangeBroadcastHandler : BroadcastHandler
{
    public override bool Handle(EventData eventData)
    {
        if (base.Handle(eventData))
        {
            int cakeCount = (int)eventData.Parameters[(byte)CakeNumberChangeBroadcastParameterCode.CakeNumber];
            IANTGame.Player.CakeCount = cakeCount;
            IANTGame.InformManager.GameInformManager.CallCakeNumberChange(cakeCount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool CheckError(EventData eventData)
    {
        if (eventData.Parameters.Count != 1)
        {
            return false;
        }
        return true;
    }
}

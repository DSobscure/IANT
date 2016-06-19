using UnityEngine;
using System.Collections;
using Managers;
using System;

public class KitchenControllerK : MonoBehaviour, IResponseHandler
{
    [SerializeField]
    private KitchenUI kitchenUI;
    [SerializeField]
    private PlayerInfoController playerInfoController;

    public void EraseEvents(ResponseManager responseManager)
    {
        responseManager.OperationResponseManager.EraseTakeCakeResponseFunction(OnTakeCakeResponse);
    }

    public void RegisterEvents(ResponseManager responseManager)
    {
        responseManager.OperationResponseManager.RegistrTakeCakeResponseFunction(OnTakeCakeResponse);
    }

    void Start()
    {
        RegisterEvents(IANTGame.ResponseManager);
    }
    void OnDestroy()
    {
        EraseEvents(IANTGame.ResponseManager);
    }

    void Update ()
    {
        kitchenUI.UpdateCakeInfo(IANTGame.Player.LastTakeCakeTime);
    }

    private void OnTakeCakeResponse(int cakeCount, DateTime lastTime)
    {
        IANTGame.Player.GainCake(cakeCount, lastTime);
        playerInfoController.UpdatePlayerInfo(IANTGame.Player);
    }

    public void TakeCake()
    {
        if(kitchenUI.CakeCount() > 0)
            IANTGame.ActionManager.OperationManager.TakeCake();
    }
}

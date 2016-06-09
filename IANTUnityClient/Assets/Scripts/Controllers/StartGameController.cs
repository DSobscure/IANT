using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using IANTLibrary;

public class StartGameController : MonoBehaviour, IResponseHandler
{
    [SerializeField]
    private TranningModeSelectUI tranningModeSelectUI;

    public void EraseEvents(ResponseManager responseManager)
    {
        responseManager.OperationResponseManager.EraseStartGameResponseFunction(OnStartGameResponseAction);
    }

    public void RegisterEvents(ResponseManager responseManager)
    {
        responseManager.OperationResponseManager.RegistrStartGameResponseFunction(OnStartGameResponseAction);
    }

    void Start()
    {
        RegisterEvents(IANTGame.ResponseManager);
    }
    void OnDestroy()
    {
        EraseEvents(IANTGame.ResponseManager);
    }

    public void StartGame()
    {
        IANTGame.ActionManager.OperationManager.StartGame(tranningModeSelectUI.GetUsedCakeNumber());
    }

    private void OnStartGameResponseAction(int usedCakeNumber)
    {
        IANTGame.Player.UseCake(usedCakeNumber);
        IANTGame.UsedCakeNumber = usedCakeNumber;
        IANTGame.Game = new ClientGame();
        SceneManager.LoadScene("Game");
    }
}

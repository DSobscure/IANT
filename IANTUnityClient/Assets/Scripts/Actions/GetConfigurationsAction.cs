using UnityEngine;
using IANTLibrary;
using Managers;
using System;

public class GetConfigurationsAction : MonoBehaviour, IResponseHandler
{
    [SerializeField]
    private StartMenuUI startMenuUI;

    void Start()
    {
        RegisterEvents(IANTGame.ResponseManager);
    }
    void OnDestroy()
    {
        EraseEvents(IANTGame.ResponseManager);
    }

    public void EraseEvents(ResponseManager responseManager)
    {
        IANTGame.ResponseManager.FetchDataResponseManager.EraseGetConfigurationsResponseFunction(OnGetConfigurations);
    }

    public void RegisterEvents(ResponseManager responseManager)
    {
        IANTGame.ResponseManager.FetchDataResponseManager.RegistrGetConfigurationsResponseFunction(OnGetConfigurations);
    }

    public void GetConfigurations()
    {
        IANTGame.ActionManager.FetchDataActionManager.GetConfigurations();
    }
    private void OnGetConfigurations(TowerUpgradeConfigurationContent content)
    {
        TowerUpgradeConfiguration.Load(content);
        IANTGame.IsLoadedConfigurations = true;
        startMenuUI.LoadedConfigurations();
    }
}

using System;
using IANTLibrary;

public class FetchDataResponseManager
{
    private event Action<TowerUpgradeConfigurationContent> onGetConfigurationsResponse;
    public void RegistrGetConfigurationsResponseFunction(Action<TowerUpgradeConfigurationContent> responseFunction)
    {
        onGetConfigurationsResponse += responseFunction;
    }
    public void EraseGetConfigurationsResponseFunction(Action<TowerUpgradeConfigurationContent> responseFunction)
    {
        onGetConfigurationsResponse -= responseFunction;
    }
    public void CallGetConfigurationsResponse(TowerUpgradeConfigurationContent content)
    {
        if (onGetConfigurationsResponse != null)
        {
            onGetConfigurationsResponse(content);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private event Action<ChallengePlayerInfo[]> onGetChallengePlayerListResponse;
    public void RegistrGetChallengePlayerListResponseFunction(Action<ChallengePlayerInfo[]> responseFunction)
    {
        onGetChallengePlayerListResponse += responseFunction;
    }
    public void EraseGetChallengePlayerListResponseFunction(Action<ChallengePlayerInfo[]> responseFunction)
    {
        onGetChallengePlayerListResponse -= responseFunction;
    }
    public void CallGetChallengePlayerListResponse(ChallengePlayerInfo[] content)
    {
        if (onGetChallengePlayerListResponse != null)
        {
            onGetChallengePlayerListResponse(content);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private event Action<HarvestPlayerInfo[]> onGetHarvestPlayerListResponse;
    public void RegistrGetHarvestPlayerListResponseFunction(Action<HarvestPlayerInfo[]> responseFunction)
    {
        onGetHarvestPlayerListResponse += responseFunction;
    }
    public void EraseGetHarvestPlayerListResponseFunction(Action<HarvestPlayerInfo[]> responseFunction)
    {
        onGetHarvestPlayerListResponse -= responseFunction;
    }
    public void CallGetHarvestPlayerListResponse(HarvestPlayerInfo[] content)
    {
        if (onGetHarvestPlayerListResponse != null)
        {
            onGetHarvestPlayerListResponse(content);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}

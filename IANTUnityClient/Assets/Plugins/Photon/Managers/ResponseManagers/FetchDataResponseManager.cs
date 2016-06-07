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
}

using UnityEngine;
using UnityEngine.UI;
using Managers;

public class AlertHandler : MonoBehaviour, IInformHandler
{
    [SerializeField]
    private GameObject alertPanel;
    [SerializeField]
    private Text alertText;

    void Awake()
    {
        RegisterEvents(IANTGame.InformManager);
    }

    void OnDestroy()
    {
        EraseEvents(IANTGame.InformManager);
    }

    private void OnAlert(string message)
    {
        alertPanel.SetActive(true);
        alertText.text = message;
    }

    public void RegisterEvents(InformManager informManager)
    {
        informManager.SystemInformManager.RegistrAlertInformFunction(OnAlert);
    }

    public void EraseEvents(InformManager informManager)
    {
        informManager.SystemInformManager.EraseAlertInformFunction(OnAlert);
    }
}

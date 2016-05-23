using UnityEngine;
using Managers;
using UnityEngine.UI;

public class DebugReturn : MonoBehaviour, IInformHandler
{
    [SerializeField]
    private Text debugText;

	void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        RegisterEvents(IANTGame.InformManager);
    }

    void OnDestroy()
    {
        EraseEvents(IANTGame.InformManager);
    }

    private void OnDebugReturn(string message)
    {
        //debugText.text += message + " |=| ";
        Debug.Log(message);
    }

    public void RegisterEvents(InformManager informManager)
    {
        informManager.SystemInformManager.RegistrDebugReturnInformFunction(OnDebugReturn);
    }

    public void EraseEvents(InformManager informManager)
    {
        informManager.SystemInformManager.EraseDebugReturnInformFunction(OnDebugReturn);
    }
}

using UnityEngine;
using Managers;

public class ConnectController : MonoBehaviour, IResponseHandler
{
    private bool ConnectStatus = true;

    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        RegisterEvents(IANTGame.ResponseManager);
    }

    void Start()
    {
        if (!IANTGame.Service.ServerConnected)
        {
            IANTGame.Service.Connect();
            IANTGame.Service.DebugReturn(ExitGames.Client.Photon.DebugLevel.INFO, "initial connect");
        }
        else
        {
            IANTGame.Service.DebugReturn(ExitGames.Client.Photon.DebugLevel.ERROR, "initial connect error");
        }
    }

    private void ConnectEventAction(bool Status)
    {
        if (Status)
        {
            Debug.Log("Connecting . . . . .");
            ConnectStatus = true;
        }
        else
        {
            Debug.Log("Connect Fail");
            ConnectStatus = false;
        }
    }

    void OnDestroy()
    {
        EraseEvents(IANTGame.ResponseManager);
    }

    void OnGUI()
    {
        if (ConnectStatus == false)
        {
            GUI.Label(new Rect(130, 10, 100, 20), "Connect fail");
        }

        if (IANTGame.Service.ServerConnected)
        {
            GUI.Label(new Rect(130, 10, 100, 20), "Connecting . . .");
        }
    }

    public void RegisterEvents(ResponseManager responseManager)
    {
        responseManager.SystemResponseManager.RegistrConnectResponseFunction(ConnectEventAction);
    }

    public void EraseEvents(ResponseManager responseManager)
    {
        responseManager.SystemResponseManager.EraseConnectResponseFunction(ConnectEventAction);
    }
}

using UnityEngine;
using Managers;

public class ConnectController : MonoBehaviour, IResponseHandler
{
    private bool ConnectStatus = true;
    [SerializeField]
    private Texture2D connectingIcon;
    [SerializeField]
    private Texture2D disconnectIcon;

    void Awake()
    {
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
        //GUI.Label(new Rect(20, 10, 100, 20), string.Format("{0}FPS", 1f / Time.smoothDeltaTime));
        if (ConnectStatus == false)
        {
            GUI.DrawTexture(new Rect(10, 1, 10, 10), disconnectIcon, ScaleMode.ScaleToFit, true);
        }

        if (IANTGame.Service.ServerConnected)
        {
            GUI.DrawTexture(new Rect(10, 1, 10, 10), connectingIcon, ScaleMode.ScaleToFit, true);
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

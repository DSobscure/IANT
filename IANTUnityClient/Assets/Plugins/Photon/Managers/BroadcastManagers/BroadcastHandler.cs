using ExitGames.Client.Photon;
using IANTProtocol;

public abstract class BroadcastHandler
{
    public virtual bool Handle(EventData eventData)
    {
        if (!CheckError(eventData))
        {
            IANTGame.InformManager.SystemInformManager.CallDebugReturn(string.Format("Broadcast Error On {0}", (BroadcastCode)eventData.Code));
            return false;
        }
        else
        {
            return true;
        }
    }
    public abstract bool CheckError(EventData eventData);
}

using UnityEngine;

public class CallService : MonoBehaviour
{
    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void FixedUpdate()
    {
        IANTGame.Service.Service();
    }

    void OnApplicationQuit()
    {
        IANTGame.Service.Disconnect();
    }
}

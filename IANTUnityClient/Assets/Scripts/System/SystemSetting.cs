using UnityEngine;
using System.Collections;

public class SystemSetting : MonoBehaviour
{
    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 0;
    }
}

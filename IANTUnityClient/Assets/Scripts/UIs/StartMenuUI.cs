using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField]
    private StartMenuAction startMenuAction;

    [SerializeField]
    internal Text resultText;

	void Start ()
    {
        startMenuAction = GameObject.FindGameObjectWithTag("Actions").GetComponent<StartMenuAction>();
    }
}

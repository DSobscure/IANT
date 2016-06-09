using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField]
    internal Text resultText;
    [SerializeField]
    private GameObject testGameButton;

    public void LoadedConfigurations()
    {
        resultText.text = "已載入遊戲設定";
        //testGameButton.SetActive(true);
    }
}

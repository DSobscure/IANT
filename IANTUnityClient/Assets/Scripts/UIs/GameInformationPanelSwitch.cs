using UnityEngine;
using System.Collections;

public class GameInformationPanelSwitch : MonoBehaviour
{
    [SerializeField]
    private bool isAtRight;

    public void Switch()
    {
        if(isAtRight)
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.localPosition = new Vector2(-430, 80);
            isAtRight = !isAtRight;
        }
        else
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.localPosition = new Vector2(430, 80);
            isAtRight = !isAtRight;
        }
    }
}

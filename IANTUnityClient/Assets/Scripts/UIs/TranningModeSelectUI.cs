using UnityEngine;
using UnityEngine.UI;
using System;

public class TranningModeSelectUI : MonoBehaviour
{
    [SerializeField]
    private Text usedCakeNumberText;
    [SerializeField]
    private Slider usedCakeSlider;

    void Start()
    {
        usedCakeSlider.maxValue = Math.Min(IANTGame.Player.CakeCount, 10);
        usedCakeSlider.minValue = 1;
        usedCakeSlider.onValueChanged.AddListener((value) => { usedCakeNumberText.text = ((int)value).ToString(); });
        usedCakeSlider.value = usedCakeSlider.maxValue;
    }

    public int GetUsedCakeNumber()
    {
        return (int)usedCakeSlider.value;
    }
}

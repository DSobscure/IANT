using UnityEngine;
using UnityEngine.UI;
using System;

public class KitchenUI : MonoBehaviour
{
    [SerializeField]
    private Text cakeCountText;
    [SerializeField]
    private Text remainedTimeForMakingCakeText;

    public void UpdateCakeInfo(DateTime lastTakeCakeTime)
    {
        TimeSpan duration = DateTime.Now - lastTakeCakeTime;
        if(duration.TotalMinutes >= 50)
        {
            cakeCountText.text = "10";
            remainedTimeForMakingCakeText.text = "蛋糕數量已滿";
        }
        else if(duration.TotalMinutes < 0)
        {
            cakeCountText.text = "0";
            remainedTimeForMakingCakeText.text = "";
        }
        else
        {
            cakeCountText.text = ((int)duration.TotalMinutes / 5).ToString();
            remainedTimeForMakingCakeText.text = string.Format("剩餘{0}分{1}秒", 4 - (duration.Minutes%5), 59 - duration.Seconds);
        }
    }
}

using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using IANTLibrary;

public class PlayerInfoController : MonoBehaviour
{
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Slider experiencePointsSlider;
    [SerializeField]
    private Text honorPointsText;
    [SerializeField]
    private Text cakeCountText;

    public void UpdatePlayerInfo(Player player)
    {
        if(player != null)
        {
            levelText.text = player.Level.ToString();
            experiencePointsSlider.maxValue = player.UpgradeExperiencePoints;
            experiencePointsSlider.value = player.ExperiencePoints;
            honorPointsText.text = player.HonorPoints.ToString();
            cakeCountText.text = player.Foods.Count(food => food is Cake).ToString();
        }
    }
}

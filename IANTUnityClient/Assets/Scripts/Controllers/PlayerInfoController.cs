using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using IANTLibrary;

public class PlayerInfoController : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Slider experiencePointsSlider;
    [SerializeField]
    private Text cakeCountText;
    [SerializeField]
    private Image profilePhoto;

    void Start()
    {
        UpdatePlayerInfo(IANTGame.Player);
    }

    public void UpdatePlayerInfo(Player player)
    {
        if(player != null)
        {
            levelText.text = player.Level.ToString();
            experiencePointsSlider.maxValue = player.EXP;
            experiencePointsSlider.value = player.EXP;
            if (player.FoodInfos.Count > 0)
                cakeCountText.text = player.FoodInfos.First(x => x.food is Cake).count.ToString();
            else
                cakeCountText.text = "0";
        }
    }

    void FixedUpdate()
    {
        if(profilePhoto.sprite == null && IANTGame.ProfilePicture != null)
        {
            profilePhoto.sprite = Sprite.Create(IANTGame.ProfilePicture, new Rect(0, 0, 50f, 50f), new Vector2(0, 0));
        }
        if(IANTGame.ProfileResult != null && IANTGame.ProfileResult.ContainsKey("name"))
        {
            nameText.text = (string)IANTGame.ProfileResult["name"];
        }
    }
}

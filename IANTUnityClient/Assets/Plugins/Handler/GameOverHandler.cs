using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private RawImage backgroundImage;
    [SerializeField]
    private Text harvestCakeCountText;
    bool isGameOver = false;
    float fadeDelta = 0;

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        if(IANTGame.GameType == "harvest")
        {
            harvestCakeCountText.text = IANTGame.Game.FoodFactory.TakenFoodCount.ToString();
        }
        isGameOver = true;
        backgroundImage.CrossFadeColor(new Color(0, 0, 0, 1), 3f, true, true);
    }

    void Update()
    {
        if(isGameOver)
        {
            fadeDelta += Time.deltaTime;
            Time.timeScale -= Time.deltaTime;
            backgroundImage.color = new Color(0, 0, 0, fadeDelta);
        }
    }
}

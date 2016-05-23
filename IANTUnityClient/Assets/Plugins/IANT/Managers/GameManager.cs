using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private FoodManagerTest test;

    [SerializeField]
    private Text waveText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text moneyText;

    [SerializeField]
    private GameObject createTowerButton;

    [SerializeField]
    private GameOverHandler gameOverHandler;
    [SerializeField]
    private AntGrowthPanelController antGrowthPanelController;

    public void UpdateWave(int wave)
    {
        waveText.text = wave.ToString();
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
    public void UpdateMoney(int money)
    {
        moneyText.text = money.ToString();
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
    public void StopGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void OnGameOver()
    {
        gameOverHandler.ShowGameOverPanel();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            createTowerButton.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            createTowerButton.SetActive(true);
        }
    }

    public void SetGrowth()
    {
        FoodManagerTest.AntGrowthProperties = antGrowthPanelController.GetGrowthProperties();
        RestartGame();
    }
}

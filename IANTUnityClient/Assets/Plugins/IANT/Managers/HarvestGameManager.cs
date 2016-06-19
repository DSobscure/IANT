using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IANTLibrary;
using Managers;
using System;

public class HarvestGameManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private GameObject foodPlateObject;
    [SerializeField]
    private GameObject nestObject;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private AntManager antManager;
    [SerializeField]
    private FoodController foodController;
    [SerializeField]
    private TowerManager towerManager;

    [SerializeField]
    private Text waveText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text remainedWaveText;

    [SerializeField]
    private GameOverHandler gameOverHandler;

    private int remainedWave = 0;

    private float deltaTime;
    void Start()
    {
        IANTGame.Game.OnGameOver += OnGameOver;
        StartGame();
    }
    void OnDestroy()
    {
        IANTGame.Game.OnGameOver -= OnGameOver;
        IANTGame.Game.OnWaveChange -= OnWaveChange;
    }

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
        
    }
    public void ExitGame()
    {
        Time.timeScale = 1;
        OnGameOver();
        SceneManager.LoadScene("Main");
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
        IANTGame.ActionManager.OperationManager.HarvestGameOver(IANTGame.Game.Wave, IANTGame.HarvestFacebookID, IANTGame.Game.FoodFactory.TakenFoodCount);
        gameOverHandler.ShowGameOverPanel();
    }
    public void OnWaveChange(int wave)
    {
        remainedWave--;
        if (remainedWave == -1)
        {
            OnGameOver();
        }
        else
        {
            remainedWaveText.text = remainedWave.ToString();
        }
    }
    public void StartGame()
    {
        Nest nest = IANTGame.Player.Nests[0];
        IANTGame.Game = new ClientGame();
        IANTGame.Game.OnWaveChange += OnWaveChange;
        Game game = IANTGame.Game;
        game.BindConfiguration(new GameConfiguration
        {
            foodPlatePositionX = foodPlateObject.transform.position.x,
            foodPlatePositionY = foodPlateObject.transform.position.y,
            nestPositionX = nestObject.transform.position.x,
            nestPositionY = nestObject.transform.position.y,
            antPrefab = new ClientAnt(new AntProperties
            {
                level = 1,
                food = null,
                hp = 5,
                maxHP = 5,
                velocity = 100,
            }, nest),
            towerPrefab = new ClientTower(new TowerProperties
            {
                name = "砲塔",
                upgradeCost = 50,
                destroyReturn = 50,
                bulletNumber = 1,
                bulletSpanRange = 10,
                damage = 3,
                level = 1,
                elementType = ElelmentType.Normal,
                frequency = 1f,
                range = 90f,
                speed = 300f
            }, bulletPrefab),
            leastTowerSpan = 60f,
            startMoney = 1000,
            antNumber = 6,
            foodPlateRadius = foodPlateObject.transform.localScale.x / 2,
            nestRadius = nestObject.transform.localScale.x / 2
        });
        nest.BindGame(game);
        (game.AntFactory as ClientAntFactory).BindAntManager(antManager);
        (game.TowerFactory as ClientTowerFactory).BindAntManager(towerManager);
        game.AntFactory.BindNest(nest);
        IANTGame.Game = game;
        (game as ClientGame).BindManager(this);
        foodController.RegisterEvents();
        (game.TowerFactory as ClientTowerFactory).LoadTowers(IANTGame.HarvestTargetDefenceDataString);
        (game.TowerFactory as ClientTowerFactory).DisplaceTowers();
        remainedWave = IANTGame.UsedCakeNumber * 3;
        remainedWaveText.text = remainedWave.ToString();
        IANTGame.Game.StartGame(new Cake(), IANTGame.HarvestableCakeNumber);
    }
}

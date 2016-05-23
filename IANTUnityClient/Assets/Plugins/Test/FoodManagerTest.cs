using UnityEngine;
using System.Collections;
using IANTLibrary;

public class FoodManagerTest : MonoBehaviour
{
    [SerializeField]
    private FoodController foodController;
    [SerializeField]
    private AntManager antManager;
    [SerializeField]
    private TowerManager towerManager;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject foodPlateObject;
    [SerializeField]
    private GameObject nestObject;

    [SerializeField]
    private WeightTest wt;

    public static AntGrowthProperties AntGrowthProperties;

    void Start ()
    {
        Application.targetFrameRate = 40;
        StartGame();
    }

    public void StartGame()
    {
        Nest nest = new Nest(AntGrowthProperties);
        ClientGame game = new ClientGame(new GameConfiguration
        {
            foodPlatePositionX = foodPlateObject.transform.position.x,
            foodPlatePositionY = foodPlateObject.transform.position.y,
            nestPositionX = nestObject.transform.position.x,
            nestPositionY = nestObject.transform.position.y,
            antPrefab = new ClientAnt(new AntProperties
            {
                level = 1,
                food = null,
                hp = 3,
                maxHP = 3,
                velocity = 100
            }, nest),
            towerPrefab = new ClientTower(new TowerProperties
            {
                name = "砲塔",
                upgradeCost = 50,
                destroyReturn = 50,
                bulletNumber = 1,
                bulletSpanRange = 0,
                damage = 1,
                level = 1,
                elementType = ElelmentType.Normal,
                frequency = 1f,
                range = 90f,
                speed = 300f
            }, bulletPrefab),
            leastTowerSpan = 60f,
            startMoney = 3000,
            antNumber = 10,
            foodPlateRadius = foodPlateObject.transform.localScale.x/2,
            nestRadius = nestObject.transform.localScale.x/2
        });
        nest.BindGame(game);
        (game.AntFactory as ClientAntFactory).BindAntManager(antManager);
        (game.TowerFactory as ClientTowerFactory).BindAntManager(towerManager);
        game.AntFactory.BindNest(nest);
        IANTGame.Game = game;
        game.BindManager(gameManager);
        foodController.RegisterEvents();
        IANTGame.Game.OnGameOver += gameManager.OnGameOver;
        wt.nest = nest;
        IANTGame.Game.StartGame(new Cake(), 50);      
    }
}

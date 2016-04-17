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

    void Start ()
    {
        ClientGame game = new ClientGame(new GameConfiguration
        {
            foodPlatePositionX = 250f,
            foodPlatePositionY = -210f,
            nestPositionX = -430,
            nestPositionY = 250,
            antPrefab = new ClientAnt(new AntProperties
            {
                level = 1,
                food = null,
                hp = 10,
                maxHP = 10
            }),
            towerPrefab = new ClientTower(new TowerProperties
            {
                upgradeCost = 100,
                destroyReturn = 50
            }, bulletPrefab),
            leastTowerSpan = 80f
        });
        (game.AntFactory as ClientAntFactory).BindAntManager(antManager);
        (game.TowerFactory as ClientTowerFactory).BindAntManager(towerManager);
        IANTGame.Game = game;
        game.BindManager(gameManager);
        foodController.RegisterEvents();
        IANTGame.Game.StartGame(6, new Cake(), 5, 100);
    }
}

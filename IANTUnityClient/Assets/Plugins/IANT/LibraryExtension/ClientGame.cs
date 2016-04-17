using UnityEngine;
using System.Collections;
using IANTLibrary;

public class ClientGame : Game
{
    private GameManager gameManager;
    public ClientGame(GameConfiguration configuration) : base(configuration)
    {
        AntFactory = new ClientAntFactory(configuration.nestPositionX, configuration.nestPositionY, configuration.antPrefab);
        FoodFactory = new ClientFoodFactory(configuration.foodPlatePositionX, configuration.foodPlatePositionY);
        TowerFactory = new ClientTowerFactory(configuration.towerPrefab, configuration.leastTowerSpan);
    }
    public void BindManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        OnMoneyChange += gameManager.UpdateMoney;
    }
}

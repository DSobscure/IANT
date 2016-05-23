using UnityEngine;
using System.Collections;
using IANTLibrary;

public class ClientGame : Game
{
    private GameManager gameManager;
    public ClientGame(GameConfiguration configuration) : base(configuration)
    {
        AntFactory = new ClientAntFactory(configuration.antPrefab, this);
        FoodFactory = new ClientFoodFactory(this);
        TowerFactory = new ClientTowerFactory(configuration.towerPrefab, configuration.leastTowerSpan, this);
    }
    public void BindManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        OnWaveChange += gameManager.UpdateWave;
        OnScoreChange += gameManager.UpdateScore;
        OnMoneyChange += gameManager.UpdateMoney;
        
    }
}

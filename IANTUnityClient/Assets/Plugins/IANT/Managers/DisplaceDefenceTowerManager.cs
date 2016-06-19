using UnityEngine;
using UnityEngine.UI;
using IANTLibrary;
using System;
using Managers;
using UnityEngine.SceneManagement;

public class DisplaceDefenceTowerManager : MonoBehaviour, IGameManager, IResponseHandler
{
    [SerializeField]
    private GameObject foodPlateObject;
    [SerializeField]
    private GameObject nestObject;

    [SerializeField]
    private Text remainedMoneyText;

    [SerializeField]
    private TowerManager towerManager;
    void Start()
    {
        StartDisplacement();
        RegisterEvents(IANTGame.ResponseManager);
    }
    void OnDestroy()
    {
        EraseEvents(IANTGame.ResponseManager);
    }

    public void StartDisplacement()
    {
        IANTGame.Game = new ClientGame();
        Nest nest = null;//IANTGame.Player.Nests[0];
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
            }, null),
            leastTowerSpan = 60f,
            startMoney = IANTGame.Player.DefenceBudget - IANTGame.Player.UsedDefenceBudget,
            antNumber = 0,
            foodPlateRadius = 90,
            nestRadius = 50
        });
        //nest.BindGame(game);
        //(game.AntFactory as ClientAntFactory).BindAntManager(antManager);
        (game.TowerFactory as ClientTowerFactory).BindAntManager(towerManager);
        //game.AntFactory.BindNest(nest);
        IANTGame.Game = game;
        (game as ClientGame).BindManager(this);
        IANTGame.Game.StartGame(new Cake(), 0);
        game.TowerFactory.LoadTowers(IANTGame.Player.DefenceDataString);
        (game.TowerFactory as ClientTowerFactory).DisplaceTowers();
        towerManager.UpdateBuildTowerCost(game.TowerFactory.NextTowerCost);
    }

    public void UpdateWave(int wave)
    {

    }

    public void UpdateScore(int score)
    {

    }

    public void UpdateMoney(int money)
    {
        remainedMoneyText.text = money.ToString();
    }

    public void SetDefence()
    {
        IANTGame.ActionManager.OperationManager.SetDefence(IANTGame.Game.TowerFactory, IANTGame.Player.DefenceBudget - IANTGame.Game.Money);
    }
    public void ResetDefence()
    {
        IANTGame.Player.DefenceDataString = null;
        IANTGame.Player.UsedDefenceBudget = 0;
        SceneManager.LoadScene("DisplaceDefenceTower");
    }

    public void RegisterEvents(ResponseManager responseManager)
    {
        responseManager.OperationResponseManager.RegistrSetDefenceResponseFunction(OnSetDefenceResponseAction);
    }

    public void EraseEvents(ResponseManager responseManager)
    {
        responseManager.OperationResponseManager.EraseSetDefenceResponseFunction(OnSetDefenceResponseAction);
    }

    private void OnSetDefenceResponseAction()
    {
        SceneManager.LoadScene("Main");
    }
}

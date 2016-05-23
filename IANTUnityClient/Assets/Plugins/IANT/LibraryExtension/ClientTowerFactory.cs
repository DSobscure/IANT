using UnityEngine.UI;
using IANTLibrary;
using UnityEngine;

public class ClientTowerFactory : TowerFactory
{
    private TowerManager towerManager;

    public ClientTowerFactory(Tower towerPrefab, float leastTowerSpan, Game game) : base(towerPrefab, leastTowerSpan, game)
    {

    }
    public void BindAntManager(TowerManager towerManager)
    {
        this.towerManager = towerManager;
        OnBuildTowerCostChange += towerManager.UpdateBuildTowerCost;
    }
    public override bool BuildTower(float positionX, float positionY, Game game, out Tower tower, out string errorMessage)
    {
        if (base.BuildTower(positionX, positionY, game, out tower, out errorMessage))
        {
            ClientTower targetTower = tower as ClientTower;
            Button button;
            targetTower.BindInstance(towerManager.InstantiateNewTower(positionX, positionY, out button), button);
            button.onClick.AddListener(() => (IANTGame.Game.TowerFactory as ClientTowerFactory).towerManager.SelectTower(targetTower));
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool DestroyTower(Tower tower, Game game)
    {
        if(base.DestroyTower(tower, game))
        {
            ClientTower targetTower = tower as ClientTower;
            towerManager.DestroyTowerInstance(targetTower);
            return true;
        }
        else
        {
            return false;
        }
    }
}

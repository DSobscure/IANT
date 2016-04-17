using UnityEngine;
using System.Collections;
using IANTLibrary;

public class ClientTowerFactory : TowerFactory
{
    private TowerManager towerManager;

    public ClientTowerFactory(Tower towerPrefab, float leastTowerSpan) : base(towerPrefab, leastTowerSpan)
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
            targetTower.BindInstance(towerManager.InstantiateNewTower(positionX, positionY));
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

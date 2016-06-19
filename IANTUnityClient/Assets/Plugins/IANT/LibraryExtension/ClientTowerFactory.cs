using UnityEngine.UI;
using IANTLibrary;
using UnityEngine;
using System.Linq;

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
    public void DisplaceTowers()
    {
        Tower[] towers = towerDictionary.Values.ToArray();

        for(int i = 0; i < towers.Length; i++)
        {
            ClientTower targetTower = towers[i] as ClientTower;
            Button button;
            targetTower.BindInstance(towerManager.InstantiateNewTower(targetTower.PositionX, targetTower.PositionY, out button), button);
            button.onClick.AddListener(() => (IANTGame.Game.TowerFactory as ClientTowerFactory).towerManager.SelectTower(targetTower));
            switch (targetTower.ElementType)
            {
                case ElelmentType.Ice:
                    targetTower.TowerInstance.GetComponent<SpriteRenderer>().color = new Color(170f / 255f, 255f / 255f, 228 / 255f);
                    break;
                case ElelmentType.Fire:
                    targetTower.TowerInstance.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 84f / 255f, 0f / 255f);
                    break;
                case ElelmentType.Thunder:
                    targetTower.TowerInstance.GetComponent<SpriteRenderer>().color = new Color(120f / 255f, 56f / 255f, 138f / 255f);
                    break;
                case ElelmentType.Wind:
                    targetTower.TowerInstance.GetComponent<SpriteRenderer>().color = new Color(160f / 255f, 255f / 255f, 171f / 255f);
                    break;
                case ElelmentType.Poison:
                    targetTower.TowerInstance.GetComponent<SpriteRenderer>().color = new Color(37f / 255f, 82f / 255f, 33f / 255f);
                    break;
                case ElelmentType.Wood:
                    targetTower.TowerInstance.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 197f / 255f, 9f / 255f);
                    break;
            }
            if (targetTower.ElementType == ElelmentType.Wind)
            {
                float radius = targetTower.Range / 30f;
                targetTower.TowerInstance.GetComponent<CircleCollider2D>().radius = radius;
                targetTower.TowerInstance.transform.GetChild(0).localScale = new Vector3(2f * radius, 2f * radius, 1f);
            }
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

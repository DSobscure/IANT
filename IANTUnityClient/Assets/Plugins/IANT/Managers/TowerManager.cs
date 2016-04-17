using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private Text buildTowerCostText;

    public GameObject InstantiateNewTower(float positionX, float positionY)
    {
        return Instantiate(towerPrefab, new Vector3(positionX, positionY, 3), Quaternion.identity) as GameObject;
    }
    public void DestroyTowerInstance(ClientTower tower)
    {
        Destroy(tower.towerInstance);
    }
    public void UpdateBuildTowerCost(int cost)
    {
        buildTowerCostText.text = string.Format("({0})", cost);
    }
}

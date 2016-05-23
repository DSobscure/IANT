using UnityEngine;
using UnityEngine.UI;
using IANTLibrary;

public class TowerManager : MonoBehaviour
{
    //Prefabs
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private Button towerButtonPrefab;
    //UIs
    [SerializeField]
    private Text buildTowerCostText;
    [SerializeField]
    private TowerInfoController towerInfoController;

    private ClientTower selectedTower;

    public GameObject InstantiateNewTower(float positionX, float positionY, out Button button)
    {
        button = Instantiate(towerButtonPrefab, new Vector3(positionX, positionY, 3), Quaternion.identity) as Button;
        return Instantiate(towerPrefab, new Vector3(positionX, positionY, 3), Quaternion.identity) as GameObject;
    }
    public void DestroyTowerInstance(ClientTower tower)
    {
        Destroy(tower.TowerInstance);
    }
    public void UpdateBuildTowerCost(int cost)
    {
        buildTowerCostText.text = string.Format("({0})", cost);
    }
    public void SelectTower(ClientTower tower)
    {
        if(selectedTower != null)
        {
            selectedTower.TowerInstance.transform.GetChild(0).gameObject.SetActive(false);
        }
        selectedTower = tower;
        towerInfoController.ShowInfo(tower);
        selectedTower.TowerInstance.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ClearSelect()
    {
        if (selectedTower != null)
        {
            selectedTower.TowerInstance.transform.GetChild(0).gameObject.SetActive(false);
        }
        selectedTower = null;
        towerInfoController.ShowInfo(null);
    }
    public void DestroyTower()
    {
        if(selectedTower != null)
        {
            IANTGame.Game.TowerFactory.DestroyTower(selectedTower, IANTGame.Game);
            ClearSelect();
        }
    }
    public void ShowSeletedTowerInfo()
    {
        towerInfoController.ShowTowerInfo(selectedTower);
    }
    public void ShowSeletedTowerDetroyInfo()
    {
        towerInfoController.ShowDestroyTowerReturn(selectedTower);
    }
    public void ShowTowerUpgradeInfo(int direction)
    {
        towerInfoController.ShowTowerUpgradeInfo(selectedTower, (TowerUpgradeDirection)direction);
    }
    public void ShowTowerElementUpgradeInfo(int elementType)
    {
        towerInfoController.ShowTowerElementUpgradeInfo(selectedTower, (ElelmentType)elementType);
    }
    public void UpgradeTower(int direction)
    {
        selectedTower.Upgrade((TowerUpgradeDirection)direction, IANTGame.Game);
        if(selectedTower.Level % 3 == 0)
        {
            towerInfoController.ShowInfo(selectedTower);
        }
        else
        {
            ShowTowerUpgradeInfo(direction);
        }
    }
    public void ElementUpgradeTower(int elementType)
    {
        selectedTower.ElementUpgrade((ElelmentType)elementType, IANTGame.Game);
        towerInfoController.ShowInfo(selectedTower);
    }
    public void ShowTowerElementUpgradeInfo()
    {
        towerInfoController.ShowTowerElementUpgradeInfo(selectedTower, selectedTower.ElementType);
    }
    public void ElementUpgradeTower()
    {
        selectedTower.ElementUpgrade(selectedTower.ElementType, IANTGame.Game);
        towerInfoController.ShowInfo(selectedTower);
    }
}

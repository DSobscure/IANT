using UnityEngine;
using UnityEngine.UI;
using IANTLibrary;
using System.Text;

public class TowerInfoController : MonoBehaviour
{
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private GameObject antInfoPanel;
    [SerializeField]
    private GameObject towerInfoPanel;
    [SerializeField]
    private GameObject basicUpgradePanel;
    [SerializeField]
    private GameObject elementSelectPanel;
    [SerializeField]
    private GameObject elementUpgradePanel;
    [SerializeField]
    private Text towerInfoText;

    public void ShowInfo(ClientTower tower)
    {
        if(tower == null)
        {
            infoPanel.SetActive(false);
        }
        else
        {
            infoPanel.SetActive(true);
            antInfoPanel.SetActive(false);
            towerInfoPanel.SetActive(true);
            if (tower.Level  == 3)
            {
                basicUpgradePanel.SetActive(false);
                elementSelectPanel.SetActive(true);
                elementUpgradePanel.SetActive(false);
            }
            else if (tower.Level % 3 == 0)
            {
                basicUpgradePanel.SetActive(false);
                elementSelectPanel.SetActive(false);
                elementUpgradePanel.SetActive(true);
            }
            else
            {
                basicUpgradePanel.SetActive(true);
                elementSelectPanel.SetActive(false);
                elementUpgradePanel.SetActive(false);
            }
            ShowTowerInfo(tower);
        }
    }

    public void ShowTowerInfo(Tower tower)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("{0}\n", tower.Name);
        stringBuilder.AppendFormat("傷害： {0}\n", tower.Damage);
        stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
        stringBuilder.AppendFormat("射速： {0} 次/秒\n", tower.Frequency);
        stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
        stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
        towerInfoText.text = stringBuilder.ToString();
    }
    public void ShowDestroyTowerReturn(Tower tower)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("退回金錢： {0}\n", tower.DestroyReturn);
        towerInfoText.text = stringBuilder.ToString();
    }
    public void ShowTowerUpgradeInfo(Tower tower, TowerUpgradeDirection direction)
    {
        Tower upgradedTower = tower.GetUpgradeSample(direction);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("{0} → {1}\n", tower.Name, upgradedTower.Name);
        stringBuilder.AppendFormat("消耗金錢 {0}\n", tower.UpgradeCost);
        switch(direction)
        {
            case TowerUpgradeDirection.Damage:
                stringBuilder.AppendFormat("傷害： {0} → {1}\n", tower.Damage, upgradedTower.Damage);
                stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
                stringBuilder.AppendFormat("射速： {0} 次/秒\n", tower.Frequency);
                stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
                stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
                break;
            case TowerUpgradeDirection.Range:
                stringBuilder.AppendFormat("傷害： {0}\n", tower.Damage);
                stringBuilder.AppendFormat("射程： {0} → {1}\n", tower.Range, upgradedTower.Range);
                stringBuilder.AppendFormat("射速： {0} 次/秒\n", tower.Frequency);
                stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
                stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
                break;
            case TowerUpgradeDirection.Frequency:
                stringBuilder.AppendFormat("傷害： {0}\n", tower.Damage);
                stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
                stringBuilder.AppendFormat("射速： {0} → {1} 次/秒\n", tower.Frequency, upgradedTower.Frequency);
                stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
                stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
                break;
            case TowerUpgradeDirection.Speed:
                stringBuilder.AppendFormat("傷害： {0}\n", tower.Damage);
                stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
                stringBuilder.AppendFormat("射速： {0} 次/秒\n", tower.Frequency);
                stringBuilder.AppendFormat("彈速： {0} → {1}\n", tower.Speed, upgradedTower.Speed);
                stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
                break;
            case TowerUpgradeDirection.BulletNumber:
                stringBuilder.AppendFormat("傷害： {0}\n", tower.Damage);
                stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
                stringBuilder.AppendFormat("射速： {0} 次/秒\n", tower.Frequency);
                stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
                stringBuilder.AppendFormat("彈數： {0} → {1}\n", tower.BulletNumber, upgradedTower.BulletNumber);
                break;
        }
        towerInfoText.text = stringBuilder.ToString();
    }
    public void ShowTowerElementUpgradeInfo(Tower tower, ElelmentType elementType)
    {
        Tower upgradedTower = tower.GetElementUpgradeSample(elementType);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("{0} → {1}\n", tower.Name, upgradedTower.Name);
        stringBuilder.AppendFormat("消耗金錢 {0}\n", tower.UpgradeCost);
        switch (elementType)
        {
            case ElelmentType.Ice:
                stringBuilder.AppendFormat("傷害： {0}\n", tower.Damage);
                stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
                stringBuilder.AppendFormat("射速： {0} 次/秒\n", tower.Frequency);
                stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
                stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
                stringBuilder.AppendFormat("砲彈夾角：{0} → {1}\n", tower.BulletSpanRange, upgradedTower.BulletSpanRange);
                break;
            case ElelmentType.Fire:
                stringBuilder.AppendFormat("傷害： {0} → {1}\n", tower.Damage, upgradedTower.Damage);
                stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
                stringBuilder.AppendFormat("射速： {0} → {1} 次/秒\n", tower.Frequency, upgradedTower.Frequency);
                stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
                stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
                break;
            case ElelmentType.Thunder:
                stringBuilder.AppendFormat("傷害： {0} → {1}\n", tower.Damage, upgradedTower.Damage);
                stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
                stringBuilder.AppendFormat("射速： {0} → {1} 次/秒\n", tower.Frequency, upgradedTower.Frequency);
                stringBuilder.AppendFormat("彈速： {0} → {1}\n", tower.Speed, upgradedTower.Speed);
                stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
                stringBuilder.AppendFormat("砲彈夾角：{0} → {1}\n", tower.BulletSpanRange, upgradedTower.BulletSpanRange);
                break;
            case ElelmentType.Wind:
                stringBuilder.AppendFormat("傷害： {0}\n", tower.Damage);
                stringBuilder.AppendFormat("射程： {0} → {1}\n", tower.Range, upgradedTower.Range);
                stringBuilder.AppendFormat("射速： {0} 次/秒\n", tower.Frequency);
                stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
                stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
                break;
            case ElelmentType.Poison:
                stringBuilder.AppendFormat("傷害： {0}\n", tower.Damage);
                stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
                stringBuilder.AppendFormat("射速： {0} → {1} 次/秒\n", tower.Frequency, upgradedTower.Frequency);
                stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
                stringBuilder.AppendFormat("彈數： {0} → {1}\n", tower.BulletNumber, upgradedTower.BulletNumber);
                break;
            case ElelmentType.Wood:
                stringBuilder.AppendFormat("傷害： {0} → {1}\n", tower.Damage, upgradedTower.Damage);
                stringBuilder.AppendFormat("射程： {0}\n", tower.Range);
                stringBuilder.AppendFormat("射速： {0} 次/秒\n", tower.Frequency);
                stringBuilder.AppendFormat("彈速： {0}\n", tower.Speed);
                stringBuilder.AppendFormat("彈數： {0}\n", tower.BulletNumber);
                break;
            default:
                stringBuilder.AppendFormat("還沒做\n");
                break;
        }
        towerInfoText.text = stringBuilder.ToString();
    }
}

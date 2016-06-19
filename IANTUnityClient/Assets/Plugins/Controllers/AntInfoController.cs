using UnityEngine;
using UnityEngine.UI;
using IANTLibrary;
using System.Text;

public class AntInfoController : MonoBehaviour
{
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private GameObject antInfoPanel;
    [SerializeField]
    private GameObject towerInfoPanel;
    [SerializeField]
    private Text antText;

    private Ant selectedAnt;

    public void ShowInfo(Ant ant)
    {
        infoPanel.SetActive(true);
        antInfoPanel.SetActive(true);
        towerInfoPanel.SetActive(false);
        selectedAnt = ant;
        ShowAntInfo(ant);
    }

    public void ShowAntInfo(Ant ant)
    {
        if(ant == null)
        {
            antText.text = "";
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("等級： {0}\n", ant.Level);
            stringBuilder.AppendFormat("血量： {0}/{1}\n", ant.HP, ant.MaxHP);
            stringBuilder.AppendFormat("速度： {0}\n", ant.Speed);
            if (ant.FireEffectDuration > 0)
            {
                stringBuilder.AppendFormat("燃燒剩餘時間： {0:#,0.#}秒\n", ant.FireEffectDuration);
            }
            if (ant.IceEffectDuration > 0)
            {
                stringBuilder.AppendFormat("冰凍剩餘時間： {0:#,0.#}秒\n", ant.IceEffectDuration);
            }
            if (ant.ThunderEffectDuration > 0)
            {
                stringBuilder.AppendFormat("麻痺剩餘時間： {0:#,0.#}秒\n", ant.ThunderEffectDuration);
            }
            if (ant.PoisonEffectDuration > 0)
            {
                stringBuilder.AppendFormat("中毒剩餘時間： {0:#,0.#}秒\n", ant.PoisonEffectDuration);
            }
            antText.text = stringBuilder.ToString();
        }
    }

    void Update()
    {
        if(selectedAnt != null)
        {
            ShowAntInfo(selectedAnt);
            IANTGame.Game.TowerFactory.AimOnTarget(selectedAnt);
            if (selectedAnt.HP <= 0)
            {
                selectedAnt = null;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using IANTLibrary;

public class NestInfoUI : MonoBehaviour
{
    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Text durationText;
    [SerializeField]
    private Text speedText;
    [SerializeField]
    private Text resistantText;
    [SerializeField]
    private Text populationText;
    [SerializeField]
    private Text sensitivityText;

    [SerializeField]
    private Button durationButton;
    [SerializeField]
    private Button speedButton;
    [SerializeField]
    private Button resistantButton;
    [SerializeField]
    private Button populationButton;
    [SerializeField]
    private Button sensitivityButton;

    [SerializeField]
    private GameObject formationPanel1;
    [SerializeField]
    private GameObject formationPanel2;
    [SerializeField]
    private GameObject formationPanel3;
    [SerializeField]
    private RawImage blockPrefab;

    private RawImage[,] blocks1;
    private RawImage[,] blocks2;
    private RawImage[,] blocks3;

    [SerializeField]
    private Text costText;

    void Start()
    {
        if(IANTGame.Player.Nests != null && IANTGame.Player.Nests.Count > 0)
        {
            IANTGame.Player.Nests[0].OnGrowthPropertiesChange += OnNestPropertiesChangeAction;
            OnNestPropertiesChangeAction(IANTGame.Player.Nests[0].GrowthProperties);
            Nest nest = IANTGame.Player.Nests[0];
            blocks1 = new RawImage[11, 11];
            blocks2 = new RawImage[11, 11];
            blocks3 = new RawImage[11, 11];
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    blocks1[x, y] = Instantiate(blockPrefab) as RawImage;
                    blocks1[x, y].rectTransform.SetParent(formationPanel1.transform);
                    blocks1[x, y].rectTransform.localScale = Vector3.one;
                    blocks1[x, y].rectTransform.localPosition = new Vector3(x * 8 - 40, y * 8 - 40, 1);
                    blocks1[x, y].color = new Color(1, 1, 1, 0.3f + 50 * (float)nest.distributionMaps[0].distributionWeight[x, y]);
                }
            }
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    blocks2[x, y] = Instantiate(blockPrefab) as RawImage;
                    blocks2[x, y].rectTransform.SetParent(formationPanel2.transform);
                    blocks2[x, y].rectTransform.localScale = Vector3.one;
                    blocks2[x, y].rectTransform.localPosition = new Vector3(x * 8 - 40, y * 8 - 40, 1);
                    blocks2[x, y].color = new Color(1, 1, 1, 0.3f + 50 * (float)nest.distributionMaps[1].distributionWeight[x, y]);
                }
            }
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    blocks3[x, y] = Instantiate(blockPrefab) as RawImage;
                    blocks3[x, y].rectTransform.SetParent(formationPanel3.transform);
                    blocks3[x, y].rectTransform.localScale = Vector3.one;
                    blocks3[x, y].rectTransform.localPosition = new Vector3(x * 8 - 40, y * 8 - 40, 1);
                    blocks3[x, y].color = new Color(1, 1, 1, 0.3f + 50 * (float)nest.distributionMaps[2].distributionWeight[x, y]);
                }
            }
        }
        else
        {
            Debug.Log("no nest");
        }
    }

    void OnDestroy()
    {
        if (IANTGame.Player.Nests != null && IANTGame.Player.Nests.Count > 0)
        {
            IANTGame.Player.Nests[0].OnGrowthPropertiesChange -= OnNestPropertiesChangeAction;
        }
        else
        {
            Debug.Log("no nest");
        }
    }

    private void OnNestPropertiesChangeAction(AntGrowthProperties properties)
    {
        levelText.text = string.Format("LV.{0}", properties.Level);

        durationText.text = properties.duration.ToString();
        speedText.text = properties.speed.ToString();
        resistantText.text = properties.resistant.ToString();
        populationText.text = properties.population.ToString();
        sensitivityText.text = properties.sensitivity.ToString();

        bool upgradable = NestLevelFoodTable.FoodForUpgrade(properties.Level) <= IANTGame.Player.CakeCount;

        durationButton.interactable = upgradable && properties.duration <= 5;
        speedButton.interactable = upgradable && properties.speed <= 5;
        resistantButton.interactable = upgradable && properties.resistant <= 5;
        populationButton.interactable = upgradable && properties.population <= 5;
        sensitivityButton.interactable = upgradable && properties.sensitivity <= 5;

        costText.text = NestLevelFoodTable.FoodForUpgrade(properties.Level).ToString();
    }
}

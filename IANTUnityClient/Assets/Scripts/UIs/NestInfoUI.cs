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
    private Text costText;

    void Start()
    {
        if(IANTGame.Player.Nests != null && IANTGame.Player.Nests.Count > 0)
        {
            IANTGame.Player.Nests[0].OnGrowthPropertiesChange += OnNestPropertiesChangeAction;
            OnNestPropertiesChangeAction(IANTGame.Player.Nests[0].GrowthProperties);
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

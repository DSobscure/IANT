using UnityEngine;
using UnityEngine.UI;
using IANTLibrary;

public class AntGrowthPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Slider durationSlider;
    [SerializeField]
    private Slider speedSlider;
    [SerializeField]
    private Slider resistantSlider;
    [SerializeField]
    private Slider numberSlider;
    [SerializeField]
    private Slider sensorSlider;

    void Start()
    {
        durationSlider.value = FoodManagerTest.AntGrowthProperties.duration;
        speedSlider.value = FoodManagerTest.AntGrowthProperties.speed;
        resistantSlider.value = FoodManagerTest.AntGrowthProperties.resistant;
        numberSlider.value = FoodManagerTest.AntGrowthProperties.population;
        sensorSlider.value = FoodManagerTest.AntGrowthProperties.sensitivity;
    }

    public void Open()
    {
        panel.SetActive(true);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
    public AntGrowthProperties GetGrowthProperties()
    {
        return new AntGrowthProperties
        {
            duration = (int)durationSlider.value,
            speed = (int)speedSlider.value,
            resistant = (int)resistantSlider.value,
            population = (int)numberSlider.value,
            sensitivity = (int)sensorSlider.value
        };
    }
}

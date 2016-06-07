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
        numberSlider.value = FoodManagerTest.AntGrowthProperties.number;
        sensorSlider.value = FoodManagerTest.AntGrowthProperties.sensorDistance;
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
            number = (int)numberSlider.value,
            sensorDistance = (int)sensorSlider.value
        };
    }
}

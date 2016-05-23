using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class FoodController : MonoBehaviour, IController
{
    [SerializeField]
    private Text foodText;

	void OnDestroy()
    {
        EraseEvents();
    }

    void UpdateFoodText(int value)
    {
        foodText.text = value.ToString();
    }

    public void RegisterEvents()
    {
        IANTGame.Game.FoodFactory.OnRemainedFoodCountChange += UpdateFoodText;
    }

    public void EraseEvents()
    {
        IANTGame.Game.FoodFactory.OnRemainedFoodCountChange -= UpdateFoodText;
    }
}

using UnityEngine;
using IANTLibrary;

public class FoodPlateHandler : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ant")
        {
            Ant ant = other.GetComponent<AntController>().ant;
            if (!ant.IsTakingFood)
                IANTGame.Game.FoodFactory.TakeFood(ant);
        }
    }
}

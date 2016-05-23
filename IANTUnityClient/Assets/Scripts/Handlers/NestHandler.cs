using UnityEngine;
using IANTLibrary;

public class NestHandler : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ant")
        {
            Ant ant = other.GetComponent<AntController>().ant;
            if (ant.IsTakingFood)
                ant.PutFood(IANTGame.Game.FoodFactory);
        }
    }
}

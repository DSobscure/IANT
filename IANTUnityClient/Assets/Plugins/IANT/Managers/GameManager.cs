using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text moneyText;

    public void UpdateMoney(int money)
    {
        moneyText.text = money.ToString();
    }
}

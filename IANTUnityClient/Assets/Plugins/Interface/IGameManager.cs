using UnityEngine;
using System.Collections;

public interface IGameManager
{
    void UpdateWave(int wave);
    void UpdateScore(int score);
    void UpdateMoney(int money);
}

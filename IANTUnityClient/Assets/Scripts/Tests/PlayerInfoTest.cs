using UnityEngine;
using IANTLibrary;
using System.Collections.Generic;

public class PlayerInfoTest : MonoBehaviour
{
    [SerializeField]
    private PlayerInfoController playerInfoController;

    void Start ()
    {
        IANTGame.Player = new Player(1, new PlayerProperties
        {
            level = 1,
            exp = 5,
            foodInfos = new List<FoodInfo>(),
            nests = new List<Nest>()
        });
        playerInfoController.UpdatePlayerInfo(IANTGame.Player);

    }
}

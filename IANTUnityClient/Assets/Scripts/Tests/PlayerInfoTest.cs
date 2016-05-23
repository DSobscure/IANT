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
            experiencePoints = 5,
            honorPoints = 20,
            foods = new List<Food>(),
            nests = new List<Nest>()
        });
        playerInfoController.UpdatePlayerInfo(IANTGame.Player);

    }
}

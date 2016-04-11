using UnityEngine;
using System.Collections;
using IANTLibrary;

public class FoodManagerTest : MonoBehaviour
{
    [SerializeField]
    private FoodController foodController;
    [SerializeField]
    private AntManager antManager;

    void Start ()
    {
        ClientGame game = new ClientGame(-430, 250);
        (game.AntFactory as ClientAntFactory).SetAntManager(antManager);
        IANTGame.Game = game;
        foodController.RegisterEvents();
        IANTGame.Game.StartGame(6, new Cake(), 5);
    }
}

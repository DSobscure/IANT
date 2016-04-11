using UnityEngine;
using System.Collections;
using IANTLibrary;

public class ClientGame : Game
{
    public ClientGame(float nestPositionX, float nestPositionY) : base(nestPositionX, nestPositionY)
    {
        AntFactory = new ClientAntFactory(nestPositionX, nestPositionY);
    }
}

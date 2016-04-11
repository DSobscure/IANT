using UnityEngine;
using System.Collections;
using IANTLibrary;

public class ClientAntFactory : AntFactory
{
    private AntManager antManager;
    public ClientAntFactory(float nestPositionX, float nestPositionY) : base(nestPositionX, nestPositionY)
    {
        antPrefab = new ClientAnt(new AntProperties()
        {
            positionX = nestPositionX,
            positionY = nestPositionY,
            food = null,
            hp = 3,
            maxHP = 3
        });
    }
    public void SetAntManager(AntManager antManager)
    {
        this.antManager = antManager;
    }
    public override Ant InstantiateNewAnt()
    {
        ClientAnt ant = base.InstantiateNewAnt() as ClientAnt;
        Debug.Log(ant == null);
        ant.SetInstance(antManager.InstantiateNewAnt(ant.PositionX, ant.PositionY));
        return ant;
    }
}

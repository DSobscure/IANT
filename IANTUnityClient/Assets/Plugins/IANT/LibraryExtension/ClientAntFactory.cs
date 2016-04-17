using UnityEngine;
using System.Collections;
using IANTLibrary;

public class ClientAntFactory : AntFactory
{
    private AntManager antManager;
    public ClientAntFactory(float nestPositionX, float nestPositionY, Ant antPrefab) : base(nestPositionX, nestPositionY, antPrefab)
    {
        
    }
    public void BindAntManager(AntManager antManager)
    {
        this.antManager = antManager;
    }
    public override Ant InstantiateNewAnt()
    {
        ClientAnt ant = base.InstantiateNewAnt() as ClientAnt;
        Debug.Log(ant == null);
        ant.BindInstance(antManager.InstantiateNewAnt(ant.PositionX, ant.PositionY));
        return ant;
    }
    public override void NotifyAntDead(Ant ant)
    {
        base.NotifyAntDead(ant);
        InstantiateNewAnt();
    }
}

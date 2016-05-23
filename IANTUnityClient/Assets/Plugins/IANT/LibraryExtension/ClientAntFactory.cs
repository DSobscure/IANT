using UnityEngine;
using System.Collections;
using IANTLibrary;

public class ClientAntFactory : AntFactory
{
    private AntManager antManager;
    public ClientAntFactory(Ant antPrefab, Game game) : base(antPrefab, game)
    {
        
    }
    public void BindAntManager(AntManager antManager)
    {
        this.antManager = antManager;
    }
    public override Ant InstantiateNewAnt()
    {
        ClientAnt ant = base.InstantiateNewAnt() as ClientAnt;
        ant.BindInstance(antManager.InstantiateNewAnt(ant.PositionX, ant.PositionY, ant.Rotation));
        return ant;
    }
}

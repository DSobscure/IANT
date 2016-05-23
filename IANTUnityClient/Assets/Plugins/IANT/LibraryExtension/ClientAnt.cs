using UnityEngine;
using System;
using IANTLibrary;

public class ClientAnt : Ant
{
    private GameObject antInstance;
    private AntController antController;
    public ClientAnt(AntProperties properties, Nest nest) : base(properties, nest)
    {
    }
    public void BindInstance(GameObject instance)
    {
        antInstance = instance;
        antController = instance.GetComponent<AntController>();
        antController.ant = this;
        antController.RegisterEvents();
    }
    public override Ant Duplicate()
    {
        return new ClientAnt(properties, nest);
    }
    public override void Move()
    {
        UpdatePosition(antInstance.transform.position.x, antInstance.transform.position.y);
        base.Move();
        antController.SetVelocity();
    }
}

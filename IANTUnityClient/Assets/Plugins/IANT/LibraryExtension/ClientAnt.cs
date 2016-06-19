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
        Rigidbody2D r2d = antInstance.GetComponent<Rigidbody2D>();
        UpdateTransform(r2d.position.x, r2d.position.y, r2d.rotation);
        //UpdatePosition(antInstance.transform.position.x, antInstance.transform.position.y);
        base.Move();
        antController.SetVelocity();
    }
}

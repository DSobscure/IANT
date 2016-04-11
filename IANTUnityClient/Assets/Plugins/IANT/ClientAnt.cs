using UnityEngine;
using System.Collections;
using IANTLibrary;

public class ClientAnt : Ant
{
    private GameObject antInstance;
    private AntController antController;
    public ClientAnt(AntProperties properties) : base(properties)
    {
    }
    public void SetInstance(GameObject instance)
    {
        antInstance = instance;
        antController = instance.GetComponent<AntController>();
        antController.ant = this;
    }
    public override Ant Duplicate()
    {
        return new ClientAnt(properties);
    }
    public override void Move()
    {
        antController.Move();
        properties.positionX = antInstance.transform.position.x;
        properties.positionY = antInstance.transform.position.y;
    }
}

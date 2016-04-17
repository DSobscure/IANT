using UnityEngine;
using System.Collections;
using IANTLibrary;
using System;

public class ClientTower : Tower
{
    public GameObject towerInstance { get; protected set; }
    private TowerController towerController;
    private GameObject bulletPrefab;

    public ClientTower(TowerProperties properties, GameObject bulletPrefab) : base(properties)
    {
        this.bulletPrefab = bulletPrefab;
    }

    public override Tower Duplicate()
    {
        return new ClientTower(properties, bulletPrefab);
    }

    public void BindInstance(GameObject instance)
    {
        towerInstance = instance;
        towerController = instance.GetComponent<TowerController>();
        towerController.tower = this;
    }
    public void DestroyInstance()
    {
        
    }
    public override bool Fire()
    {
        if(base.Fire())
        {
            Vector2 velocity = new Vector2(TargetAimed.PositionX - PositionX, TargetAimed.PositionY - PositionY).normalized * 300;
            towerController.InstanceBullet(bulletPrefab, new Vector3(PositionX, PositionY, 2), velocity);
            return true;
        }
        else
        {
            return false;
        }
    }
}

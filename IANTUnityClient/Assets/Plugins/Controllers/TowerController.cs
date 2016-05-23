using UnityEngine;
using System.Collections;
using System;
using IANTLibrary;

public class TowerController : MonoBehaviour, IController
{
    public ClientTower tower;

    public void EraseEvents()
    {
        throw new NotImplementedException();
    }

    public void RegisterEvents()
    {
        throw new NotImplementedException();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ant")
        {
            Ant ant = other.GetComponent<AntController>().ant;
            tower.TargetEnterRange(ant);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ant")
        {
            Ant ant = other.GetComponent<AntController>().ant;
            tower.TargetExitRange(ant);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ant")
        {
            Ant ant = other.GetComponent<AntController>().ant;
            tower.TargetEnterRange(ant);
        }
    }

    void Update()
    {
        if(tower.TargetAimed != null && !tower.InReloading)
        {
            Bullet[] bullets;
            tower.Fire(out bullets);
        }
    }

    public void StartReload()
    {
        Invoke("Reload", 1f / tower.Frequency);
    }
    void Reload()
    {
        tower.InReloading = false;
    }

    public GameObject InstanceBullet(GameObject bulletPrefab, Vector3 position, Vector2 velocity)
    {
        Rigidbody2D rigidbody = (Instantiate(bulletPrefab, position, Quaternion.identity) as GameObject).GetComponent<Rigidbody2D>();
        rigidbody.velocity = velocity;
        return rigidbody.gameObject;
    }
}

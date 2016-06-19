using UnityEngine;
using IANTLibrary;
using UnityEngine.UI;

public class ClientTower : Tower
{
    public GameObject TowerInstance { get; protected set; }
    public Button TowerButton { get; protected set; }
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
    public override Tower Instantiate(TowerProperties properties)
    {
        return new ClientTower(properties, bulletPrefab);
    }

    public void BindInstance(GameObject instance, Button button)
    {
        TowerInstance = instance;
        TowerButton = button;
        towerController = instance.GetComponent<TowerController>();
        towerController.tower = this;
    }
    public void DestroyInstance()
    {
        
    }
    public override bool Fire(out Bullet[] bullets)
    {
        if(base.Fire(out bullets))
        {
            foreach(Bullet bullet in bullets)
            {
                Vector2 velocity = bullet.Speed * new Vector2(Mathf.Cos(Mathf.Deg2Rad*bullet.Rotation), Mathf.Sin(Mathf.Deg2Rad*bullet.Rotation)).normalized;
                GameObject bulletInstance = towerController.InstanceBullet(bulletPrefab, new Vector3(PositionX, PositionY, 2), velocity);
                bulletInstance.GetComponent<BulletController>().BindBullet(bullet);
            }
            towerController.StartReload();
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void Upgrade(TowerUpgradeDirection direction, Game game)
    {
        base.Upgrade(direction, game);
        if(direction == TowerUpgradeDirection.Range)
        {
            float radius = Range / 30f;
            TowerInstance.GetComponent<CircleCollider2D>().radius = radius;
            TowerInstance.transform.GetChild(0).localScale = new Vector3(2f * radius, 2f * radius, 1f);
        }
    }
    public override void ElementUpgrade(ElelmentType elementType, Game game)
    {
        base.ElementUpgrade(elementType, game);
        switch(elementType)
        {
            case ElelmentType.Ice:
                TowerInstance.GetComponent<SpriteRenderer>().color = new Color(170f/255f, 255f/255f, 228/255f);
                break;
            case ElelmentType.Fire:
                TowerInstance.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 84f / 255f, 0f / 255f);
                break;
            case ElelmentType.Thunder:
                TowerInstance.GetComponent<SpriteRenderer>().color = new Color(120f / 255f, 56f / 255f, 138f / 255f);
                break;
            case ElelmentType.Wind:
                TowerInstance.GetComponent<SpriteRenderer>().color = new Color(160f / 255f, 255f / 255f, 171f / 255f);
                break;
            case ElelmentType.Poison:
                TowerInstance.GetComponent<SpriteRenderer>().color = new Color(37f / 255f, 82f / 255f, 33f / 255f);
                break;
            case ElelmentType.Wood:
                TowerInstance.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 197f / 255f, 9f / 255f);
                break;
        }
        if (elementType == ElelmentType.Wind)
        {
            float radius = Range / 30f;
            TowerInstance.GetComponent<CircleCollider2D>().radius = radius;
            TowerInstance.transform.GetChild(0).localScale = new Vector3(2f * radius, 2f * radius, 1f);
        }
    }
}

using UnityEngine;
using IANTLibrary;
using IANTLibrary.Bullets;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private Material iceMaterial;
    [SerializeField]
    private Material fireMaterial;
    [SerializeField]
    private Material thunderMaterial;
    [SerializeField]
    private Material windMaterial;
    [SerializeField]
    private Material poisonMaterial;
    [SerializeField]
    private Material woodMaterial;

    private float lifeTime = 5f;
    private Bullet bullet;

	void Update ()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0)
        {
            Destroy(gameObject);
        }
        if(bullet is WindBullet)
        {
            (bullet as WindBullet).TimePass(Time.deltaTime);
            Vector2 velocity = bullet.Speed * new Vector2(Mathf.Cos(Mathf.Deg2Rad * bullet.Rotation), Mathf.Sin(Mathf.Deg2Rad * bullet.Rotation)).normalized;
            GetComponent<Rigidbody2D>().velocity = velocity;
        }
        if (bullet.Tower.DistanceFrom(transform.position.x, transform.position.y) > bullet.Tower.Range)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ant")
        {
            bullet.Hit(other.GetComponent<AntController>().ant);
            if(!bullet.IsPenetrable)
            {
                Destroy(gameObject);
            }
        }
    }

    public void BindBullet(Bullet bullet)
    {
        this.bullet = bullet;
        switch (bullet.ElementTyple)
        {
            case ElelmentType.Ice:
                GetComponent<SpriteRenderer>().material = iceMaterial;
                break;
            case ElelmentType.Fire:
                GetComponent<SpriteRenderer>().material = fireMaterial;
                break;
            case ElelmentType.Thunder:
                GetComponent<SpriteRenderer>().material = thunderMaterial;
                break;
            case ElelmentType.Wind:
                GetComponent<SpriteRenderer>().material = windMaterial;
                break;
            case ElelmentType.Poison:
                GetComponent<SpriteRenderer>().material = poisonMaterial;
                break;
            case ElelmentType.Wood:
                GetComponent<SpriteRenderer>().material = woodMaterial;
                break;
        }
    }
}

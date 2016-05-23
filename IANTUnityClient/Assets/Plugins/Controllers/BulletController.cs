using UnityEngine;
using IANTLibrary;
using IANTLibrary.Bullets;

public class BulletController : MonoBehaviour
{
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
                GetComponent<SpriteRenderer>().color = new Color(170f / 255f, 255f / 255f, 228 / 255f);
                break;
            case ElelmentType.Fire:
                GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 84f / 255f, 0f / 255f);
                break;
            case ElelmentType.Thunder:
                GetComponent<SpriteRenderer>().color = new Color(120f / 255f, 56f / 255f, 138f / 255f);
                break;
            case ElelmentType.Wind:
                GetComponent<SpriteRenderer>().color = new Color(160f / 255f, 255f / 255f, 171f / 255f);
                break;
            case ElelmentType.Poison:
                GetComponent<SpriteRenderer>().color = new Color(37f / 255f, 82f / 255f, 33f / 255f);
                break;
            case ElelmentType.Wood:
                GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 197f / 255f, 9f / 255f);
                break;
        }
    }
}

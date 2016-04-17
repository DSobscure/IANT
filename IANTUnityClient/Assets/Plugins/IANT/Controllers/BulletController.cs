using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    private float lifeTime = 1.5f;
	void Update ()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}

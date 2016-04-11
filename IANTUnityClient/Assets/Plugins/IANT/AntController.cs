using UnityEngine;
using IANTLibrary;
using System;

public class AntController : MonoBehaviour, IController
{
    public ClientAnt ant;
    private int counter = 0;

    public void Move()
    {
        Vector3 moveDirection = UnityEngine.Random.onUnitSphere;
        if (ant.IsTakingFood)
        {
            if ((transform.position - new Vector3(-430, 250)).magnitude > (transform.position + moveDirection - new Vector3(-430, 250)).magnitude)
            {
                float angleVelocity = Mathf.Atan2(moveDirection.y, moveDirection.x) / Mathf.PI * 180 * 0.3f + gameObject.GetComponent<Rigidbody2D>().angularVelocity * 0.7f;
                gameObject.GetComponent<Rigidbody2D>().MoveRotation(Mathf.Atan2(moveDirection.y, moveDirection.x) / Mathf.PI * 180);
                gameObject.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(100, 0));
            }
        }
        else
        {
            if ((transform.position - new Vector3(250, -210)).magnitude > (transform.position + moveDirection - new Vector3(250, -210)).magnitude)
            {
                gameObject.GetComponent<Rigidbody2D>().MoveRotation(Mathf.Atan2(moveDirection.y, moveDirection.x)/Mathf.PI*180);
                gameObject.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(100, 0));
            }
        }
    }
    private void ChangeAntColor()
    {
        if(ant.IsTakingFood)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    public void RegisterEvents()
    {
        ant.OnFoodChanged += ChangeAntColor;
    }
    public void EraseEvents()
    {
        ant.OnFoodChanged -= ChangeAntColor;
    }


    void Start()
    {
        RegisterEvents();
    }
    void OnDestroy()
    {
        EraseEvents();
    }
    void FixedUpdate()
    {
        if(counter % 5 == 0)
            ant.Move();
        counter++;
    }
}

using UnityEngine;
using IANTLibrary;
using System.Collections.Generic;

public class AntController : MonoBehaviour, IController
{
    public ClientAnt ant;
    private int counter = 0;

    [SerializeField]
    private Sprite antSprite;
    [SerializeField]
    private Sprite aPieceCakeSprite;

    public void SetVelocity()
    {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.rotation = Mathf.LerpAngle(rigidbody.rotation, ant.Rotation, 0.1f);
        rigidbody.velocity = transform.right * ant.Speed;
    }
    private void ChangeAntSprite()
    {
        if (ant.IsTakingFood)
        {
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = aPieceCakeSprite;
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    private void AntDead()
    {
        IANTGame.Game.AntFactory.NotifyAntDead(ant, IANTGame.Game.FoodFactory);
        IANTGame.Game.Score += ant.Level * 10;
        IANTGame.Game.Money += ant.Level * 10;
        Destroy(gameObject);
    }

    private void UpdateAntHP(int hp)
    {
        float newXScale = 0.5f * (hp / (float)ant.MaxHP);
        transform.GetChild(0).localScale = new Vector3(newXScale, 0.1f, 1);
        transform.GetChild(0).localPosition = new Vector3(-0.03f - (0.5f - newXScale) / 2, 0.3f, 0);
    }
    public void RegisterEvents()
    {
        ant.OnFoodChanged += ChangeAntSprite;
        ant.OnHPChange += UpdateAntHP;
        ant.OnAntDead += AntDead;
    }
    public void EraseEvents()
    {
        ant.OnFoodChanged -= ChangeAntSprite;
        ant.OnHPChange -= UpdateAntHP;
        ant.OnAntDead -= AntDead;
    }


    void Start()
    {
        UpdateAntHP(ant.HP);
    }
    void OnDestroy()
    {
        EraseEvents();
    }
    void FixedUpdate()
    {
        counter++;
        if(counter%4 == 0)
            ant.Move();
    }
    void Update()
    {
        ant.TimePass(Time.deltaTime);
        if (ant.FireEffectDuration > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (ant.IceEffectDuration > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else if (ant.ThunderEffectDuration > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }
        if (ant.PositionX > 480 || ant.PositionX < -480 || ant.PositionY > 300 || ant.PositionY < -300)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(new Vector2(0, 0));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public struct AntProperties
    {
        public Food food;
        public int hp;
        public int maxHP;
        public float positionX;
        public float positionY;
    }
    public class Ant
    {
        protected AntProperties properties;
        public bool IsTakingFood { get { return properties.food != null; } }
        static int AntCounter = 0;
        public int ID { get; protected set; }
        public int HP
        {
            get
            {
                return properties.hp;
            }
            protected set
            {
                properties.hp = value;
                if(properties.hp <= 0)
                {
                    OnAntDead?.Invoke();
                }
            }
        }
        public int MaxHP { get { return properties.maxHP; } }
        public float PositionX { get { return properties.positionX; } }
        public float PositionY { get { return properties.positionY; } }

        public Action OnAntDead;
        public Action OnFoodChanged;

        public Ant(AntProperties properties)
        {
            this.properties = properties;
            ID = AntCounter;
            AntCounter++;
        }

        public void TakeFood(Food food)
        {
            properties.food = food;
            OnFoodChanged?.Invoke();
        }
        public void ReleaseFood(FoodFactory foodManager)
        {
            foodManager.ReplaceFood(properties.food);
            properties.food = null;
            OnFoodChanged?.Invoke();
        }
        public virtual Ant Duplicate()
        {
            return new Ant(properties);
        }
        public void UpdatePosition(float x, float y)
        {
            properties.positionX = x;
            properties.positionY = y;
        }
        public void Hit(int damage)
        {
            properties.hp -= damage;
        }
        public virtual void Move()
        {

        }
    }
}

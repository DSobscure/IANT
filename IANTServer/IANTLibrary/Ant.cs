using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public struct AntProperties
    {
        public int level;
        public Food food;
        public int hp;
        public int maxHP;
        public float positionX;
        public float positionY;
    }
    public abstract class Ant
    {
        protected AntProperties properties;
        public bool IsTakingFood { get { return properties.food != null; } }
        static int AntCounter = 0;
        public int ID { get; protected set; }
        public int Level { get { return properties.level; } protected set { properties.level = value; } }
        public int HP
        {
            get
            {
                return properties.hp;
            }
            protected set
            {
                properties.hp = value;
                OnHPChange?.Invoke(properties.hp);
                if (properties.hp <= 0)
                {
                    OnAntDead?.Invoke();
                }
            }
        }
        public int MaxHP { get { return properties.maxHP; } }
        public float PositionX { get { return properties.positionX; } protected set { properties.positionX = value; } }
        public float PositionY { get { return properties.positionY; } protected set { properties.positionY = value; } }

        public Action OnAntDead;
        public Action OnFoodChanged;
        public Action<int> OnHPChange;

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
        public abstract Ant Duplicate();
        public void UpdatePosition(float x, float y)
        {
            PositionX = x;
            PositionY = y;
        }
        public void Hit(int damage)
        {
            HP -= damage;
        }
        public virtual void Move()
        {

        }
        public void LevelUp()
        {
            Level += 1;
            properties.maxHP += 5;
            properties.hp = properties.maxHP;
        }
    }
}

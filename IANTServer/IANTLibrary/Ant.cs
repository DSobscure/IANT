using System;
using IANTLibrary.Bullets;
using System.Collections.Generic;

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
        public float rotation;
        public float velocity;
        public float resistant;
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
                if (!isDead)
                {
                    properties.hp = Math.Min(value, MaxHP);
                    PositionInfo positionInfo = new PositionInfo { x = PositionX, y = PositionY };
                    AntStayInfo stayInfo = new AntStayInfo { hpRatioDelta = HPRatio - hpRatioPreStay, timeDelta = 0.01f };
                    if (antStayInfo.ContainsKey(positionInfo))
                    {
                        antStayInfo[positionInfo] = new AntStayInfo() { hpRatioDelta = antStayInfo[positionInfo].hpRatioDelta + stayInfo.hpRatioDelta, timeDelta = antStayInfo[positionInfo].timeDelta + stayInfo.timeDelta };
                    }
                    else
                    {
                        antStayInfo.Add(positionInfo, stayInfo);
                    }
                    hpRatioPreStay = HPRatio;
                    OnHPChange?.Invoke(properties.hp);
                    if (properties.hp <= 0)
                    {
                        isDead = true;
                        OnAntDead?.Invoke();
                        nest.AntStay(antStayInfo);
                        nest.GoodFormation(-1);
                    }
                }
            }
        }
        public int MaxHP { get { return properties.maxHP; } }
        public float PositionX { get { return properties.positionX; } protected set { properties.positionX = value; } }
        public float PositionY { get { return properties.positionY; } protected set { properties.positionY = value; } }
        public float Rotation { get { return properties.rotation; } protected set { properties.rotation = value; } }
        public float Speed
        {
            get
            {
                if (ThunderEffectDuration > 0)
                {
                    return 0;
                }
                else if (IceEffectDuration > 0 || FireEffectDuration > 0)
                {
                    return properties.velocity / Math.Max(IceEffectSlowDownRatio + 1, 1) * Math.Max(1 + FireEffectSpeedUp, 1);
                }
                else
                {
                    return properties.velocity;
                }
            }
            protected set { properties.velocity = value; }
        }
        public float Resistant { get { return properties.resistant; } protected set { properties.resistant = value; } }
        protected Nest nest;
        protected Dictionary<PositionInfo, AntStayInfo> antStayInfo = new Dictionary<PositionInfo, AntStayInfo>();
        protected float hpRatioPreStay;
        protected float deltaTimeCounter = 0;
        public float HPRatio { get { return HP / (float)MaxHP; } }

        public float IceEffectDuration { get; protected set; } = 0;
        public float IceEffectSlowDownRatio { get; protected set; } = 1f;
        public float FireEffectDuration { get; protected set; } = 0;
        public float FireEffectSpeedUp { get; protected set; } = 0;
        public float ThunderEffectDuration { get; protected set; } = 0;
        public float PoisonEffectDuration { get; protected set; } = 0;
        public float Poison { get; protected set; } = 0;
        private float poisonHP;
        private bool isDead = false;

        public Action OnAntDead;
        public Action OnFoodChanged;
        public Action<int> OnHPChange;

        public Ant(AntProperties properties, Nest nest)
        {
            this.properties = properties;
            ID = AntCounter;
            AntCounter++;
            poisonHP = MaxHP;
            this.nest = nest;
            hpRatioPreStay = HPRatio;
        }

        public bool TakeFood(Food food)
        {
            if (!isDead)
            {
                properties.food = food;
                HP += MaxHP / 4;
                OnFoodChanged?.Invoke();
                nest.GoodFormation(2);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ReleaseFood(FoodFactory foodManager)
        {
            foodManager.ReplaceFood(properties.food);
            properties.food = null;
            OnFoodChanged?.Invoke();
            nest.GoodFormation(-0.5);
        }
        public void PutFood(FoodFactory foodManager)
        {
            foodManager.EraseFood(properties.food);
            properties.food = null;
            OnFoodChanged?.Invoke();
            nest.AntStay(antStayInfo);
            antStayInfo.Clear();
            nest.GoodFormation(2000);
        }
        public abstract Ant Duplicate();
        public void UpdatePosition(float x, float y)
        {
            PositionX = x;
            PositionY = y;
        }
        public void Hurt(int damage)
        {
            if (damage > nest.GrowthProperties.duration * Level / 10)
            {
                HP -= damage;
            }
        }
        public void Heal(int value)
        {
            HP += value;
        }
        public virtual void Move()
        {
            float rotation = nest.GetProperRotation(this);
            if(rotation >= 0)
            {
                Rotation = nest.GetProperRotation(this);
            }
        }
        public void LevelUp()
        {
            Level += 1;
            properties.maxHP = Convert.ToInt32(5 + Math.Pow(1 + Level / 3.5f, 2.1 + nest.GrowthProperties.duration / 8.0));
            HP = MaxHP;
            Speed += nest.GrowthProperties.speed;
            Resistant = (float)(Math.Atan((nest.GrowthProperties.resistant+0.5) * Level / 100) / Math.PI * 2);
        }
        public void UpdateTransform(float x, float y, float rotation)
        {
            PositionX = x;
            PositionY = y;
            Rotation = rotation;
        }
        public float DistanceBetween(Ant ant)
        {
            return (float)Math.Sqrt(Math.Pow(PositionX - ant.PositionX, 2) + Math.Pow(PositionY - ant.PositionY, 2));
        }

        public void TimePass(float deltaTime)
        {
            FireEffectTimePass(deltaTime);
            IceEffectTimePass(deltaTime);
            ThunderEffectTimePass(deltaTime);
            PoisonEffectTimePass(deltaTime);
            PositionInfo positionInfo = new PositionInfo { x = PositionX, y = PositionY };
            AntStayInfo stayInfo = new AntStayInfo { hpRatioDelta = HPRatio - hpRatioPreStay - deltaTime * 0.2f, timeDelta = deltaTime };
            if (IsTakingFood)
            {
                stayInfo.hpRatioDelta += deltaTime * 0.1f * HPRatio;
            }
            if (antStayInfo.ContainsKey(positionInfo))
            {
                antStayInfo[positionInfo] = new AntStayInfo() { hpRatioDelta = antStayInfo[positionInfo].hpRatioDelta + stayInfo.hpRatioDelta, timeDelta = antStayInfo[positionInfo].timeDelta + stayInfo.timeDelta };
            }
            else
            {
                antStayInfo.Add(positionInfo, stayInfo);
            }
            deltaTimeCounter += deltaTime;
            hpRatioPreStay = HP / (float)MaxHP;
            if (deltaTimeCounter % 2 > 1)
            {
                nest.AntStay(antStayInfo);
            }
            if (deltaTimeCounter > 5)
            {
                nest.AntStay(antStayInfo);
                antStayInfo.Clear();
                deltaTimeCounter = 0;
                nest.GoodFormation(0.2);
            }
        }
        public void FireEffectTimePass(float deltaTime)
        {
            if (FireEffectDuration > 0)
            {
                FireEffectDuration = Math.Max(FireEffectDuration - deltaTime, 0);
                if (FireEffectDuration == 0)
                {
                    FireEffectSpeedUp = 0;
                }
            }
        }
        public void IceEffectTimePass(float deltaTime)
        {
            if (IceEffectDuration > 0)
            {
                IceEffectDuration = Math.Max(IceEffectDuration - deltaTime, 0);
                IceEffectSlowDownRatio = Math.Max(IceEffectSlowDownRatio - deltaTime, 1f);
                if (IceEffectDuration == 0)
                {
                    IceEffectSlowDownRatio = 1;
                }
            }
        }
        public void ThunderEffectTimePass(float deltaTime)
        {
            if (ThunderEffectDuration > 0)
            {
                ThunderEffectDuration = Math.Max(ThunderEffectDuration - deltaTime, 0);
            }
        }
        public void PoisonEffectTimePass(float deltaTime)
        {
            if (PoisonEffectDuration > 0)
            {
                poisonHP -= Poison * deltaTime;
                if (Convert.ToInt32(poisonHP) < HP)
                {
                    HP = Convert.ToInt32(poisonHP);
                }
                PoisonEffectDuration = Math.Max(PoisonEffectDuration - deltaTime, 0);
                if (PoisonEffectDuration == 0)
                {
                    Poison = 0;
                    poisonHP = 0;
                }
            }
        }


        public void FireEffect(FireBullet bullet)
        {
            float maxDuration = 1 * (1 - Resistant);
            float maxSpeedUp = 0.5f * bullet.ElementLevel * (1 - Resistant);
            if (FireEffectDuration + bullet.EffectDuration > maxDuration)
            {
                FireEffectDuration = Math.Max(FireEffectDuration, maxDuration);
            }
            else
            {
                FireEffectDuration = FireEffectDuration + bullet.EffectDuration;
            }
            if (FireEffectSpeedUp + bullet.SpeedUp > maxSpeedUp)
            {
                FireEffectSpeedUp = Math.Max(FireEffectSpeedUp, maxSpeedUp);
            }
            else
            {
                FireEffectSpeedUp = FireEffectSpeedUp + bullet.SpeedUp;
            }
        }
        public void IceEffect(IceBullet bullet)
        {
            float maxDuration = 1f * bullet.ElementLevel * (1 - Resistant); ;
            float maxSlowDownRation = 1.3f * bullet.ElementLevel * (1 - Resistant); ;
            if (IceEffectDuration + bullet.EffectDuration > maxDuration)
            {
                IceEffectDuration = Math.Max(IceEffectDuration, maxDuration);
            }
            else
            {
                IceEffectDuration = IceEffectDuration + bullet.EffectDuration;
            }
            if (IceEffectSlowDownRatio + bullet.SlowDownRatio > maxSlowDownRation)
            {
                IceEffectSlowDownRatio = Math.Max(IceEffectSlowDownRatio, maxSlowDownRation);
            }
            else
            {
                IceEffectSlowDownRatio = IceEffectSlowDownRatio + bullet.SlowDownRatio;
            }
        }
        public void ThunderEffect(ThunderBullet bullet)
        {
            Random random = new Random();
            if (random.NextDouble() < bullet.ParalysisProbability * (1 - Resistant))
            {
                float maxDuration = 0.2f * bullet.ElementLevel * (1 - Resistant); ;
                if (ThunderEffectDuration + bullet.EffectDuration > maxDuration)
                {
                    ThunderEffectDuration = Math.Max(ThunderEffectDuration, maxDuration);
                }
                else
                {
                    ThunderEffectDuration = ThunderEffectDuration + bullet.EffectDuration;
                }
            }
        }
        public void PoisonEffect(PoisonBullet bullet)
        {
            float maxDuration = 5f * bullet.ElementLevel * (1 - Resistant); ;
            float maxPoison = 3 * bullet.ElementLevel * (1 - Resistant); ;
            if (PoisonEffectDuration + bullet.EffectDuration > maxDuration)
            {
                PoisonEffectDuration = Math.Max(PoisonEffectDuration, maxDuration);
            }
            else
            {
                PoisonEffectDuration = PoisonEffectDuration + bullet.EffectDuration;
            }
            if (Poison + bullet.Poison > maxPoison)
            {
                Poison = Math.Max(Poison, maxPoison);
            }
            else
            {
                Poison = Poison + bullet.Poison;
            }
            if (poisonHP == 0)
            {
                poisonHP = HP;
            }
        }
    }
}
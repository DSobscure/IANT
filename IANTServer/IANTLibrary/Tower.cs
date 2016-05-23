using System;
using System.Collections.Generic;
using System.Linq;
using IANTLibrary.Bullets;

namespace IANTLibrary
{
    public struct TowerProperties
    {
        public int level;
        public string name;
        public float positionX;
        public float positionY;
        public int upgradeCost;
        public int destroyReturn;
        public float range;
        public float frequency;
        public float speed;
        public int bulletNumber;
        public float bulletSpanRange;
        public int damage;
        public ElelmentType elementType;
    }
    public class Tower
    {
        static int TowerCounter = 0;
        protected TowerProperties properties;
        public int UniqueID { get; }
        public string Name { get { return properties.name + Level.ToString(); } protected set { properties.name = value; } }
        public int Level { get { return properties.level; } protected set { properties.level = value; } }
        public float PositionX { get { return properties.positionX; } protected set { properties.positionX = value; } }
        public float PositionY { get { return properties.positionY; } protected set { properties.positionY = value; } }
        public int UpgradeCost { get { return properties.upgradeCost; } protected set { properties.upgradeCost = value; } }
        public int DestroyReturn { get { return properties.destroyReturn; } protected set { properties.destroyReturn = value; } }
        public float Range { get { return properties.range; } protected set { properties.range = value; } }
        public float Frequency { get { return properties.frequency; } protected set { properties.frequency = value; } }
        public float Speed { get { return properties.speed; } protected set { properties.speed = value; } }
        public int BulletNumber { get { return properties.bulletNumber; } protected set { properties.bulletNumber = value; } }
        public float BulletSpanRange { get { return properties.bulletSpanRange; } protected set { properties.bulletSpanRange = value; } }
        public int Damage { get { return properties.damage; } protected set { properties.damage = value; } }
        public ElelmentType ElementType { get { return properties.elementType; } protected set { properties.elementType = value; } }
        public Ant TargetAimed { get; protected set; }
        protected List<Ant> targetsInRange;
        public bool InReloading { get; set; }


        public Tower(TowerProperties properties)
        {
            this.properties = properties;
            UniqueID = TowerCounter;
            TowerCounter++;
            targetsInRange = new List<Ant>();
        }
        public float DistanceFrom(float x, float y)
        {
            return Convert.ToSingle(Math.Sqrt(Math.Pow(PositionX - x, 2) + Math.Pow(PositionY - y, 2)));
        }
        public void Locate(float x, float y)
        {
            PositionX = x;
            PositionY = y;
        }
        public void TargetEnterRange(Ant target)
        {
            if(!targetsInRange.Contains(target))
                targetsInRange.Add(target);
            AimOnTarget();
        }
        public void TargetExitRange(Ant target)
        {
            if (targetsInRange.Contains(target))
                targetsInRange.Remove(target);
            AimOnTarget();
        }
        public void AimOnTarget()
        {
            if (targetsInRange.Count > 0)
            {
                TargetAimed = targetsInRange.First();
                if (TargetAimed.HP <= 0)
                    TargetExitRange(TargetAimed);
            }
            else
                TargetAimed = null;
        }
        public virtual bool Fire(out Bullet[] bullets)
        {
            if(TargetAimed != null && TargetAimed.HP > 0 && !InReloading)
            {
                if(Range+32f < DistanceFrom(TargetAimed.PositionX, TargetAimed.PositionY))
                {
                    TargetExitRange(TargetAimed);
                    return Fire(out bullets);
                }
                else
                {
                    InReloading = true;
                    bullets = new Bullet[BulletNumber];
                    for (int i = 0; i < BulletNumber; i++)
                    {
                        BulletProperties bulletProperties = new BulletProperties
                        {
                            damage = Damage,
                            elementType = ElementType,
                            isPenetrable = false,
                            positionX = PositionX,
                            positionY = PositionY,
                            speed = Speed,
                            rotation = Convert.ToSingle(Math.Atan2(TargetAimed.PositionY - PositionY, TargetAimed.PositionX - PositionX) * 180.0 / Math.PI - BulletSpanRange * (BulletNumber - 1) / 2.0 + BulletSpanRange * i)
                        };
                        switch (ElementType)
                        {
                            case ElelmentType.Normal:
                                bullets[i] = new Bullet(bulletProperties, this);
                                break;
                            case ElelmentType.Ice:
                                bullets[i] = new IceBullet(bulletProperties, this, (Level-1)/3);
                                break;
                            case ElelmentType.Fire:
                                bullets[i] = new FireBullet(bulletProperties, this, (Level - 1) / 3);
                                break;
                            case ElelmentType.Thunder:
                                bulletProperties.isPenetrable = true;
                                bullets[i] = new ThunderBullet(bulletProperties, this, (Level - 1) / 3);
                                break;
                            case ElelmentType.Wind:
                                bulletProperties.speed /= 2f;
                                bullets[i] = new WindBullet(bulletProperties, this, (Level - 1) / 3, TargetAimed);
                                break;
                            case ElelmentType.Poison:
                                bulletProperties.isPenetrable = true;
                                bullets[i] = new PoisonBullet(bulletProperties, this, (Level - 1) / 3);
                                break;
                            case ElelmentType.Wood:
                                bullets[i] = new WoodBullet(bulletProperties, this);
                                break;
                        }
                    }
                    return true;
                }
            }
            else
            {
                TargetAimed = null;
                bullets = null;
                return false;
            }
        }
        public virtual Tower Duplicate()
        {
            return new Tower(properties);
        }
        public Tower GetUpgradeSample(TowerUpgradeDirection direction)
        {
            TowerProperties newTowerProperties = new TowerProperties
            {
                level = Level + 1,
                positionX = PositionX,
                positionY = PositionY,
                upgradeCost = UpgradeCost * 2,
                destroyReturn = DestroyReturn * 2,
                elementType = ElelmentType.Normal
            };
            switch (direction)
            {
                case TowerUpgradeDirection.Damage:
                    newTowerProperties.damage = Damage + 2;
                    break;
                case TowerUpgradeDirection.Speed:
                    newTowerProperties.speed = Speed + 50;
                    break;
                case TowerUpgradeDirection.Range:
                    newTowerProperties.range = Range + 30;
                    break;
                case TowerUpgradeDirection.Frequency:
                    newTowerProperties.frequency = Frequency + 0.5f;
                    break;
                case TowerUpgradeDirection.BulletNumber:
                    newTowerProperties.bulletNumber = BulletNumber + 1;
                    newTowerProperties.bulletSpanRange = 10;
                    break;
            }
            return new Tower(newTowerProperties);
        }
        public Tower GetElementUpgradeSample(ElelmentType elementType)
        {
            TowerProperties newTowerProperties = new TowerProperties
            {
                level = Level + 1,
                positionX = PositionX,
                positionY = PositionY,
                upgradeCost = UpgradeCost * 2,
                destroyReturn = DestroyReturn * 2,
                elementType = elementType
            };
            switch (elementType)
            {
                case ElelmentType.Ice:
                    newTowerProperties.bulletSpanRange = BulletSpanRange + 1;
                    newTowerProperties.name = "冰凍塔";
                    break;
                case ElelmentType.Fire:
                    newTowerProperties.damage = Convert.ToInt32(Damage * 1.3f);
                    newTowerProperties.frequency = Frequency * 1.5f;
                    newTowerProperties.name = "火焰塔";
                    break;
                case ElelmentType.Thunder:
                    newTowerProperties.speed = Speed * 0.95f;
                    newTowerProperties.bulletSpanRange = BulletSpanRange * 0.7f;
                    newTowerProperties.damage = Convert.ToInt32(Damage * 1.4f);
                    newTowerProperties.frequency = Frequency * 1.1f;
                    newTowerProperties.name = "閃電塔";
                    break;
                case ElelmentType.Wind:
                    newTowerProperties.range = Range * 1.3f;
                    newTowerProperties.name = "風暴塔";
                    break;
                case ElelmentType.Poison:
                    newTowerProperties.frequency = Frequency * 0.9f;
                    newTowerProperties.bulletNumber = BulletNumber + 1;
                    newTowerProperties.name = "毒物塔";
                    break;
                case ElelmentType.Wood:
                    newTowerProperties.damage = Convert.ToInt32(Damage * 1.5f);
                    newTowerProperties.name = "森林塔";
                    break;
            }
            return new Tower(newTowerProperties);
        }
        public virtual void Upgrade(TowerUpgradeDirection direction, Game game)
        {
            if(game.Money >= UpgradeCost)
            {
                game.Money -= UpgradeCost;
                Level = Level + 1;
                UpgradeCost = Convert.ToInt32(Math.Ceiling(UpgradeCost * 1.6));
                DestroyReturn = Convert.ToInt32(Math.Ceiling(DestroyReturn * 1.6));
                switch (direction)
                {
                    case TowerUpgradeDirection.Damage:
                        Damage = Damage + 2;
                        break;
                    case TowerUpgradeDirection.Speed:
                        Speed = Speed + 50;
                        break;
                    case TowerUpgradeDirection.Range:
                        Range = Range + 30;
                        break;
                    case TowerUpgradeDirection.Frequency:
                        Frequency = Frequency + 0.5f;
                        break;
                    case TowerUpgradeDirection.BulletNumber:
                        BulletNumber = BulletNumber + 1;
                        BulletSpanRange = 10;
                        break;
                }
            }
        }
        public virtual void ElementUpgrade(ElelmentType elementType, Game game)
        {
            if (game.Money >= UpgradeCost)
            {
                game.Money -= UpgradeCost;
                Level = Level + 1;
                UpgradeCost = Convert.ToInt32(Math.Ceiling(UpgradeCost * 1.6));
                DestroyReturn = Convert.ToInt32(Math.Ceiling(DestroyReturn * 1.6));
                if (ElementType == ElelmentType.Normal)
                    ElementType = elementType;
                switch (ElementType)
                {
                    case ElelmentType.Ice:
                        BulletSpanRange = BulletSpanRange + 1;
                        Name = "冰凍塔";
                        break;
                    case ElelmentType.Fire:
                        Damage = Convert.ToInt32(Damage * 1.3f);
                        Frequency = Frequency * 1.5f;
                        Name = "火焰塔";
                        break;
                    case ElelmentType.Thunder:
                        Speed = Speed * 0.95f;
                        Damage = Convert.ToInt32(Damage * 1.4f);
                        BulletSpanRange = BulletSpanRange * 0.7f;
                        Frequency = Frequency * 1.1f;
                        Name = "閃電塔";
                        break;
                    case ElelmentType.Wind:
                        Range = Range * 1.3f;
                        Name = "風暴塔";
                        break;
                    case ElelmentType.Poison:
                        Frequency = Frequency * 0.9f;
                        BulletNumber = BulletNumber + 1;
                        BulletSpanRange = 15;
                        Name = "毒物塔";
                        break;
                    case ElelmentType.Wood:
                        Damage = Convert.ToInt32(Damage * 1.5f);
                        Name = "森林塔";
                        break;
                }
            }
        }
    }
}

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
        public TowerProperties Properties { get { return properties; } }
        public int UniqueID { get; }
        public string Name { get { return properties.name + Level.ToString(); } protected set { properties.name = value; } }
        public int Level { get { return properties.level; } protected set { properties.level = value; } }
        public int ElementLevel { get { return (Level - 1) / 3; } }
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
        public void AimOnTarget(Ant target)
        {
            if(targetsInRange.Contains(target))
            {
                TargetAimed = target;
            }
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
                                bullets[i] = new IceBullet(bulletProperties, this, ElementLevel);
                                break;
                            case ElelmentType.Fire:
                                bullets[i] = new FireBullet(bulletProperties, this, ElementLevel);
                                break;
                            case ElelmentType.Thunder:
                                bulletProperties.isPenetrable = true;
                                bullets[i] = new ThunderBullet(bulletProperties, this, ElementLevel);
                                break;
                            case ElelmentType.Wind:
                                bullets[i] = new WindBullet(bulletProperties, this, ElementLevel, TargetAimed);
                                break;
                            case ElelmentType.Poison:
                                bulletProperties.isPenetrable = true;
                                bullets[i] = new PoisonBullet(bulletProperties, this, ElementLevel);
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
        public virtual Tower Instantiate(TowerProperties properties)
        {
            return new Tower(properties);
        }
        public Tower GetUpgradeSample(TowerUpgradeDirection direction)
        {
            Tower newTower = Duplicate();
            newTower.Upgrade(direction);
            return newTower;
        }
        public Tower GetElementUpgradeSample(ElelmentType elementType)
        {
            Tower newTower = Duplicate();
            newTower.ElementUpgrade(elementType);
            return newTower;
        }
        public virtual void Upgrade(TowerUpgradeDirection direction)
        {
            Level = Level + 1;
            UpgradeCost = Convert.ToInt32(Math.Ceiling(UpgradeCost * TowerUpgradeConfiguration.UpgradeCostIncreaseRatio));
            DestroyReturn = Convert.ToInt32(Math.Ceiling(DestroyReturn * TowerUpgradeConfiguration.UpgradeCostIncreaseRatio));
            switch (direction)
            {
                case TowerUpgradeDirection.Damage:
                    Damage = Damage + TowerUpgradeConfiguration.UpgradeDamageDelta;
                    break;
                case TowerUpgradeDirection.Speed:
                    Speed = Speed + TowerUpgradeConfiguration.UpgradeSpeedDelta;
                    break;
                case TowerUpgradeDirection.Range:
                    Range = Range + TowerUpgradeConfiguration.UpgradeRangeDelta;
                    break;
                case TowerUpgradeDirection.Frequency:
                    Frequency = Frequency + TowerUpgradeConfiguration.UpgradeFrequencyDelta;
                    break;
                case TowerUpgradeDirection.BulletNumber:
                    BulletNumber = BulletNumber + TowerUpgradeConfiguration.UpgradeBulletNumberDelta;
                    BulletSpanRange = TowerUpgradeConfiguration.UpgradeBulletSpanRangeDelta;
                    break;
            }
        }
        public virtual void Upgrade(TowerUpgradeDirection direction, Game game)
        {
            if(game.Money >= UpgradeCost)
            {
                game.Money -= UpgradeCost;
                Upgrade(direction);
            }
        }
        public virtual void ElementUpgrade(ElelmentType elementType)
        {
            Level = Level + 1;
            UpgradeCost = Convert.ToInt32(Math.Ceiling(UpgradeCost * 1.9));
            DestroyReturn = Convert.ToInt32(Math.Ceiling(DestroyReturn * 1.9));
            if (ElementType == ElelmentType.Normal)
                ElementType = elementType;
            switch (ElementType)
            {
                case ElelmentType.Ice:
                    Name = "冰凍塔";
                    Damage = Convert.ToInt32(Damage * TowerUpgradeConfiguration.IceTowerUpgradeDamageRatio);
                    Range = Range * TowerUpgradeConfiguration.IceTowerUpgradeRangeRatio;
                    Frequency = Frequency * TowerUpgradeConfiguration.IceTowerUpgradeFrequencyRatio;
                    Speed = Speed * TowerUpgradeConfiguration.IceTowerUpgradeSpeedRatio;
                    BulletNumber = BulletNumber + TowerUpgradeConfiguration.IceTowerUpgradeBulletNumberDelta;
                    BulletSpanRange = BulletSpanRange + TowerUpgradeConfiguration.IceTowerUpgradeBulletSpanRangeDelta;
                    break;
                case ElelmentType.Fire:
                    Name = "火焰塔";
                    Damage = Convert.ToInt32(Damage * TowerUpgradeConfiguration.FireTowerUpgradeDamageRatio);
                    Range = Range * TowerUpgradeConfiguration.FireTowerUpgradeRangeRatio;
                    Frequency = Frequency * TowerUpgradeConfiguration.FireTowerUpgradeFrequencyRatio;
                    Speed = Speed * TowerUpgradeConfiguration.FireTowerUpgradeSpeedRatio;
                    BulletNumber = BulletNumber + TowerUpgradeConfiguration.FireTowerUpgradeBulletNumberDelta;
                    BulletSpanRange = BulletSpanRange + TowerUpgradeConfiguration.FireTowerUpgradeBulletSpanRangeDelta;
                    break;
                case ElelmentType.Thunder:
                    Name = "閃電塔";
                    Damage = Convert.ToInt32(Damage * TowerUpgradeConfiguration.ThunderTowerUpgradeDamageRatio);
                    Range = Range * TowerUpgradeConfiguration.ThunderTowerUpgradeRangeRatio;
                    Frequency = Frequency * TowerUpgradeConfiguration.ThunderTowerUpgradeFrequencyRatio;
                    Speed = Speed * TowerUpgradeConfiguration.ThunderTowerUpgradeSpeedRatio;
                    BulletNumber = BulletNumber + TowerUpgradeConfiguration.ThunderTowerUpgradeBulletNumberDelta;
                    BulletSpanRange = BulletSpanRange + TowerUpgradeConfiguration.ThunderTowerUpgradeBulletSpanRangeDelta;
                    break;
                case ElelmentType.Wind:
                    Name = "風暴塔";
                    Damage = Convert.ToInt32(Damage * TowerUpgradeConfiguration.WindTowerUpgradeDamageRatio);
                    Range = Range * TowerUpgradeConfiguration.WindTowerUpgradeRangeRatio;
                    Frequency = Frequency * TowerUpgradeConfiguration.WindTowerUpgradeFrequencyRatio;
                    Speed = Speed * TowerUpgradeConfiguration.WindTowerUpgradeSpeedRatio;
                    BulletNumber = BulletNumber + TowerUpgradeConfiguration.WindTowerUpgradeBulletNumberDelta;
                    BulletSpanRange = BulletSpanRange + TowerUpgradeConfiguration.WindTowerUpgradeBulletSpanRangeDelta;
                    break;
                case ElelmentType.Poison:
                    Name = "毒物塔";
                    Damage = Convert.ToInt32(Damage * TowerUpgradeConfiguration.PoisonTowerUpgradeDamageRatio);
                    Range = Range * TowerUpgradeConfiguration.PoisonTowerUpgradeRangeRatio;
                    Frequency = Frequency * TowerUpgradeConfiguration.PoisonTowerUpgradeFrequencyRatio;
                    Speed = Speed * TowerUpgradeConfiguration.PoisonTowerUpgradeSpeedRatio;
                    BulletNumber = BulletNumber + TowerUpgradeConfiguration.PoisonTowerUpgradeBulletNumberDelta;
                    BulletSpanRange = BulletSpanRange + TowerUpgradeConfiguration.PoisonTowerUpgradeBulletSpanRangeDelta;
                    break;
                case ElelmentType.Wood:
                    Name = "森林塔";
                    Damage = Convert.ToInt32(Damage * TowerUpgradeConfiguration.WoodTowerUpgradeDamageRatio);
                    Range = Range * TowerUpgradeConfiguration.WoodTowerUpgradeRangeRatio;
                    Frequency = Frequency * TowerUpgradeConfiguration.WoodTowerUpgradeFrequencyRatio;
                    Speed = Speed * TowerUpgradeConfiguration.WoodTowerUpgradeSpeedRatio;
                    BulletNumber = BulletNumber + TowerUpgradeConfiguration.WoodTowerUpgradeBulletNumberDelta;
                    BulletSpanRange = BulletSpanRange + TowerUpgradeConfiguration.WoodTowerUpgradeBulletSpanRangeDelta;
                    break;
            }
        }
        public virtual void ElementUpgrade(ElelmentType elementType, Game game)
        {
            if (game.Money >= UpgradeCost)
            {
                game.Money -= UpgradeCost;
                ElementUpgrade(elementType);
            }
        }
    }
}

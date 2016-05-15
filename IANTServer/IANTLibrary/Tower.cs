using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public struct TowerProperties
    {
        public float positionX;
        public float positionY;
        public int upgradeCost;
        public int degradeReturn;
        public float range;
        public float frequency;
        public float speed;
        public int bulletNumber;
        public float bulletSpanRange;
        public int damage;
        public ElelmentType elementType;
        public Bullet bulletPrefab;
    }
    public abstract class Tower
    {
        static int TowerCounter = 0;
        protected TowerProperties properties;
        public int UniqueID { get; }
        public float PositionX { get { return properties.positionX; } protected set { properties.positionX = value; } }
        public float PositionY { get { return properties.positionY; } protected set { properties.positionY = value; } }
        public int UpgradeCost { get { return properties.upgradeCost; } protected set { properties.upgradeCost = value; } }
        public int DegradeReturn { get { return properties.degradeReturn; } protected set { properties.degradeReturn = value; } }
        public float Range { get { return properties.range; } protected set { properties.range = value; } }
        public float Frequency { get { return properties.frequency; } protected set { properties.frequency = value; } }
        public float Speed { get { return properties.speed; } protected set { properties.speed = value; } }
        public int BulletNumber { get { return properties.bulletNumber; } protected set { properties.bulletNumber = value; } }
        public float BulletSpanRange { get { return properties.bulletSpanRange; } protected set { properties.bulletSpanRange = value; } }
        public int Damage { get { return properties.damage; } protected set { properties.damage = value; } }
        public ElelmentType ElementType { get { return properties.elementType; } protected set { properties.elementType = value; } }
        public Bullet BulletPrefab { get { return properties.bulletPrefab; } protected set { properties.bulletPrefab = value; } }
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
            return (float)Math.Sqrt(Math.Pow(PositionX - x, 2) + Math.Pow(PositionY - y, 2));
        }
        public void Locate(float x, float y)
        {
            PositionX = x;
            PositionY = y;
        }
        public void TargetEnterRange(Ant target)
        {
            targetsInRange.Add(target);
            if(TargetAimed == null)
            {
                TargetAimed = target;
            }
        }
        public void TargetExitRange(Ant target)
        {
            targetsInRange.Remove(target);
            if (targetsInRange.Count > 0)
            {
                TargetAimed = targetsInRange[0];
            }
            else
            {
                TargetAimed = null;
            }
        }
        public virtual bool Fire(out Bullet bullet)
        {
            if(TargetAimed.HP > 0 && !InReloading)
            {
                InReloading = true;
                bullet = BulletPrefab.Duplicate();
                return true;
            }
            else
            {
                TargetAimed = null;
                bullet = null;
                return false;
            }
        }
        public abstract Tower Duplicate();
    }
}

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
        public int destroyReturn;
    }
    public abstract class Tower
    {
        static int TowerCounter = 0;
        protected TowerProperties properties;
        public int UniqueID { get; }
        public float PositionX { get { return properties.positionX; } protected set { properties.positionX = value; } }
        public float PositionY { get { return properties.positionY; } protected set { properties.positionY = value; } }
        public int UpgradeCost { get { return properties.upgradeCost; } protected set { properties.upgradeCost = value; } }
        public int DestroyReturn { get { return properties.destroyReturn; } protected set { properties.destroyReturn = value; } }
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
        public virtual bool Fire()
        {
            if(TargetAimed.HP > 0 && !InReloading)
            {
                InReloading = true;
                return true;
            }
            else
            {
                TargetAimed = null;
                return false;
            }
        }
        public abstract Tower Duplicate();
    }
}

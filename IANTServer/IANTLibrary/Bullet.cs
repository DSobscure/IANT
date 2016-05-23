using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public struct BulletProperties
    {
        public float speed;
        public float rotation;
        public float positionX;
        public float positionY;
        public bool isPenetrable;
        public ElelmentType elementType;
        public int damage;
    }

    public class Bullet
    {
        protected BulletProperties properties;
        public float Speed { get { return properties.speed; } protected set { properties.speed = value; } }
        public float Rotation { get { return properties.rotation; } protected set { properties.rotation = value; } }
        public float PositionX { get { return properties.positionX; } protected set { properties.positionX = value; } }
        public float PositionY { get { return properties.positionY; } protected set { properties.positionY = value; } }
        public bool IsPenetrable { get { return properties.isPenetrable; } protected set { properties.isPenetrable = value; } }
        public ElelmentType ElementTyple { get { return properties.elementType; } protected set { properties.elementType = value; } }
        public int Damage { get { return properties.damage; } protected set { properties.damage = value; } }
        public Tower Tower { get; protected set; }

        public Bullet(BulletProperties properties, Tower tower)
        {
            this.properties = properties;
            Tower = tower;
        }

        public virtual void Hit(Ant ant)
        {
            ant.Hurt(Damage);
        }
    }
}

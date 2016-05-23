using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary.Bullets
{
    public class WindBullet : Bullet
    {
        public int ElementLevel { get; protected set; }
        public Ant FocusTarget { get; protected set; }
        private float accumulativeDamage = 0;
        private float accuracy;

        public WindBullet(BulletProperties properties, Tower tower, int elementLevel, Ant focusTarget) : base(properties, tower)
        {
            ElementLevel = elementLevel;
            FocusTarget = focusTarget;
            accumulativeDamage = 1f;
            accuracy = 0f;
        }

        public void TimePass(float deltaTime)
        {
            Rotation += accuracy * (Convert.ToSingle(Math.Atan2(FocusTarget.PositionY - PositionY, FocusTarget.PositionX - PositionX) * 180.0 / Math.PI) - Rotation);
            accumulativeDamage += ElementLevel * deltaTime;
            Speed += ElementLevel * deltaTime * 80;
            accuracy = Math.Min(1f, accuracy + deltaTime/1.2f);
        }

        public override void Hit(Ant ant)
        {
            Damage = (int)(Damage * accumulativeDamage);
            Damage = Math.Min(ElementLevel * 100, Damage);
            base.Hit(ant);
        }
    }
}

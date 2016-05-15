using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary.Bullets
{
    public class FireBullet : Bullet
    {
        public float DamagePerSecond { get; protected set; }
        public float EffectDuration { get; protected set; }

        public FireBullet(BulletProperties properties, float damagePerSecond, float effectDuration) : base(properties)
        {
            DamagePerSecond = damagePerSecond;
            EffectDuration = effectDuration;
        }

        public override Bullet Duplicate()
        {
            return new IceBullet(properties, DamagePerSecond, EffectDuration);
        }
        public override void Hit(Ant ant)
        {
            base.Hit(ant);
        }
    }
}

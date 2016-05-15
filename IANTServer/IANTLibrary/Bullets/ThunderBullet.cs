using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary.Bullets
{
    public class ThunderBullet : Bullet
    {
        public float ParalysisProbability { get; protected set; }
        public float EffectDuration { get; protected set; }

        public ThunderBullet(BulletProperties properties, float paralysisProbability, float effectDuration) : base(properties)
        {
            ParalysisProbability = paralysisProbability;
            EffectDuration = effectDuration;
        }

        public override Bullet Duplicate()
        {
            return new IceBullet(properties, ParalysisProbability, EffectDuration);
        }
        public override void Hit(Ant ant)
        {
            base.Hit(ant);
        }
    }
}

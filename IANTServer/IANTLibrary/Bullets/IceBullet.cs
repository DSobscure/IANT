using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary.Bullets
{
    public class IceBullet : Bullet
    {
        public float SlowDownRate { get; protected set; }
        public float EffectDuration { get; protected set; }

        public IceBullet(BulletProperties properties, float slowDownRate, float effectDuration) : base(properties)
        {
            SlowDownRate = slowDownRate;
            EffectDuration = effectDuration;
        }

        public override Bullet Duplicate()
        {
            return new IceBullet(properties, SlowDownRate, EffectDuration);
        }
        public override void Hit(Ant ant)
        {
            base.Hit(ant);
        }
    }
}

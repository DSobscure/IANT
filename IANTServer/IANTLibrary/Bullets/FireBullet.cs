using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary.Bullets
{
    public class FireBullet : Bullet
    {
        public int ElementLevel { get; protected set; }
        public float SpeedUp { get; protected set; }
        public float EffectDuration { get; protected set; }

        public FireBullet(BulletProperties properties, Tower tower, int elementLevel) : base(properties, tower)
        {
            ElementLevel = elementLevel;
            SpeedUp = elementLevel * 0.1f;
            EffectDuration = 1;
        }

        public override void Hit(Ant ant)
        {
            base.Hit(ant);
            ant.FireEffect(this);
        }
    }
}

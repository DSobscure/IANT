using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary.Bullets
{
    public class IceBullet : Bullet
    {
        public int ElementLevel { get; protected set; }
        public float SlowDownRatio { get; protected set; }
        public float EffectDuration { get; protected set; }

        public IceBullet(BulletProperties properties, Tower tower, int elementLevel) : base(properties, tower)
        {
            ElementLevel = elementLevel;
            SlowDownRatio = elementLevel * 0.2f;
            EffectDuration = elementLevel / 3f;
        }

        public override void Hit(Ant ant)
        {
            base.Hit(ant);
            ant.IceEffect(this);
        }
    }
}

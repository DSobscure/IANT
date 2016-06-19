using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary.Bullets
{
    public class ThunderBullet : Bullet
    {
        public int ElementLevel { get; protected set; }
        public double ParalysisProbability { get; protected set; }
        public float EffectDuration { get; protected set; }

        public ThunderBullet(BulletProperties properties, Tower tower, int elementLevel) : base(properties, tower)
        {
            ElementLevel = elementLevel;
            ParalysisProbability = Math.Pow(0.1, 1 / (double)elementLevel);
            EffectDuration = elementLevel * 0.1f;
        }

        public override void Hit(Ant ant)
        {
            base.Hit(ant);
            ant.ThunderEffect(this);
        }
    }
}

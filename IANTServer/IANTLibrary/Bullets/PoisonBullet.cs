using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary.Bullets
{
    public class PoisonBullet : Bullet
    {
        public int ElementLevel { get; protected set; }
        public float Poison { get; protected set; }
        public float EffectDuration { get; protected set; }

        public PoisonBullet(BulletProperties properties, Tower tower, int elementLevel) : base(properties, tower)
        {
            ElementLevel = elementLevel;
            Poison = elementLevel;
            EffectDuration = elementLevel * 2;
        }

        public override void Hit(Ant ant)
        {
            base.Hit(ant);
            ant.PoisonEffect(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary.Bullets
{
    public class WoodBullet : Bullet
    {
        public WoodBullet(BulletProperties properties, Tower tower) : base(properties, tower)
        {
        }

        public override void Hit(Ant ant)
        {
            ant.Heal(Damage);
            Damage = 0;
            base.Hit(ant);
        }
    }
}

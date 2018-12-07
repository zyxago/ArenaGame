using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class ExplosiveBullet:Projectile//W.I.P
    {
        public int explosiveRange;
        public ExplosiveBullet(Texture2D tex, Vector2 pos, int speed, int size) : base(tex, pos, speed, size)
        {
            explosiveRange = 3;
        }
    }
}

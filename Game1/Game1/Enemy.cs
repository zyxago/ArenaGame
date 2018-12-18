using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Enemy:Karaktär
    {
        Vector2 direction;
        int defaultDirTime = 20;
        int dirTime;
        bool move;

        public Enemy(Texture2D tex, Vector2 pos, int hp):base(tex, pos)
        {
            dmg = 1;
            this.hp = hp;
            size = 40;
            speed = 3;
            attackDelay = 20;
            dirTime = defaultDirTime;
            Direction();
        }

        public override void Update(Karta karta)
        {
            move = true;
            CheckWallCollision(karta);
            Move();
            if(dirTime <= 0)
            {
                Direction();
                dirTime = defaultDirTime;
            }
            dirTime--;
            base.Update(karta);
        }
        private void CheckWallCollision(Karta karta)
        {
            collisionDir = CollisionDir.None;
            foreach (Grid grid in karta.gridArray)
            {
                if(hitbox.Intersects(grid.GridBox) && grid.isSolid)
                {
                    direction.X = -direction.X;
                    direction.Y = -direction.Y;
                    break;
                }
            }
        }

        public override void Move()
        {
            position.Y += speed * direction.Y;
            position.X += speed * direction.X;
        }

        private void Direction()
        {
            direction = new Vector2(Game1.rng.Next(-100,100), Game1.rng.Next(-100,100));
            direction.Normalize();
        }
    }
}

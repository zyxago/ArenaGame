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

        public Enemy(Texture2D tex, Vector2 pos):base(tex, pos)
        {
            dmg = 1;
            hp = 10;
            size = 40;
            speed = 3;
            attackDelay = 20;
            dirTime = defaultDirTime;
        }

        public override void Update(Karta karta)
        {
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
                //Check wall collision
                hitboxTemp = new Rectangle((int)position.X, (int)(position.Y - speed), size, size);
                if (hitboxTemp.Intersects(grid.GridBox) && grid.isSolid)
                {
                    collisionDir |= CollisionDir.North;
                }
                hitboxTemp = new Rectangle((int)(position.X - speed), (int)position.Y, size, size);
                if (hitboxTemp.Intersects(grid.GridBox) && grid.isSolid)
                {
                    collisionDir |= CollisionDir.West;
                }
                hitboxTemp = new Rectangle((int)(position.X - speed), (int)position.Y, size, size);
                if (hitboxTemp.Intersects(grid.GridBox) && grid.isSolid)
                {
                    collisionDir |= CollisionDir.West;
                }
                hitboxTemp = new Rectangle((int)(position.X + speed), (int)position.Y, size, size);
                if (hitboxTemp.Intersects(grid.GridBox) && grid.isSolid)
                {
                    collisionDir |= CollisionDir.East;
                }
            }
        }

        public override void Move()
        {
            if ((collisionDir & CollisionDir.North) != CollisionDir.North && position.Y > 0)
            {
                position.Y -= speed * direction.Y;
            }
            else if ((collisionDir & CollisionDir.South) != CollisionDir.South && position.Y < Game1.window.Height)
            {
                position.Y += speed * direction.Y;
            }
            if ((collisionDir & CollisionDir.West) != CollisionDir.West && position.X > 0)
            {
                position.X -= speed * direction.X;
            }
            else if ((collisionDir & CollisionDir.East) != CollisionDir.East && position.X < Game1.window.Width)
            {
                position.X += speed * direction.X;
            }
        }

        private void Direction()
        {
            direction = new Vector2(Game1.rng.Next(-100,100), Game1.rng.Next(-100,100));
            direction.Normalize();
        }
    }
}

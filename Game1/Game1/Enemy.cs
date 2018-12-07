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
        Random rng = new Random();
        Vector2 direction;
        int defaultDirTime = 20;
        int dirTime;

        public Enemy(Texture2D tex, Vector2 pos):base(tex, pos)
        {
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

        public void CheckWallCollision(Karta karta)
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

        public void Move()//Fix!!!
        {
            if ((collisionDir & CollisionDir.North) != CollisionDir.North)
            {
                position += speed * direction;
            }
            if ((collisionDir & CollisionDir.South) != CollisionDir.South)
            {
                position += speed * direction;
            }
            if ((collisionDir & CollisionDir.West) != CollisionDir.West)
            {
                position += speed * direction;
            }
            if ((collisionDir & CollisionDir.East) != CollisionDir.East)
            {
                position += speed * direction;
            }
        }

        public void Direction()
        {
            direction = new Vector2(rng.Next(-100,100), rng.Next(-100,100));
            direction.Normalize();
        }

        public void Attack()
        {

        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Shot:Projectile
    {
        public int BounceTimes { get; set; }

        public Shot(Texture2D tex, Vector2 pos, int speed, int size, int dmg) : base(tex, pos, speed, size, dmg)
        {
            BounceTimes = 2;
        }

        public override void Update(Karta karta)
        {
            base.Update(karta);
            ShotToWallCollision(karta);
        }

        //Wall collision
        public void ShotToWallCollision(Karta karta)
        {
            foreach (Grid grid in karta.gridArray)
            {
                Rectangle tempHitbox = Hitbox;
                if (tempHitbox.Intersects(grid.GridBox) && grid.isSolid && BounceTimes != 0)
                {
                    Bounce(grid.GridBox);
                }
                else if (tempHitbox.Intersects(grid.GridBox) && grid.isSolid && BounceTimes <= 0)
                {
                    IsDead = true;
                    break;
                }
            }
        }

        //Bounce function for shot
        public void Bounce(Rectangle gridBox)
        {
            bool bounce = false;
            for(int i = 0; bounce == false; i++)
            {
                Vector2 tempDir = direction;
                Vector2 tempPos = pos;
                Rectangle tempHitbox = Hitbox;
                if(i == 0)
                {
                    tempDir.X *= -1;
                    tempPos += tempDir * speed;
                    tempHitbox = new Rectangle((int)tempPos.X, (int)tempPos.Y, size, size);
                    if (tempHitbox.Intersects(gridBox))
                    {
                        bounce = false;
                    }
                    else
                    {
                        direction = tempDir;
                        bounce = true;
                    }
                }
                else if(i == 1)
                {
                    tempDir.Y *= -1;
                    tempPos += tempDir * speed;
                    tempHitbox = new Rectangle((int)tempPos.X, (int)tempPos.Y, size, size);
                    if (tempHitbox.Intersects(gridBox))
                    {
                        bounce = false;
                    }
                    else
                    {
                        direction = tempDir;
                        bounce = true;
                    }
                }
            }
            BounceTimes--;
        }
    }
}

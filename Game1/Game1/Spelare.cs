﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Game1
{
    class Spelare:Karaktär
    {
        BinaryWriter bw;
        BinaryReader br;
        List<Projectile> projectileList;
        int shotSpeed;
        int shotSize;
        Texture2D shotTex;
        public Spelare(Texture2D tex, Vector2 pos, Texture2D shotTex):base(tex, pos)
        {
            LoadData(br, bw);
            projectileList = new List<Projectile>();
            this.shotTex = shotTex;
        }

        private void LoadData(BinaryReader br, BinaryWriter bw)
        {
            if (!File.Exists("SpelareStats.dat"))
            {
                bw = new BinaryWriter(new FileStream("SpelareStats.dat", FileMode.OpenOrCreate, FileAccess.Write));
                bw.Write(3);//hp
                bw.Write(30);//size
                bw.Write(3);//speed
                bw.Write(8);//shotSize
                bw.Write(5);//shotSpeed
                bw.Write(20);//attackDelay
                bw.Close();
                br = new BinaryReader(new FileStream("SpelareStats.dat", FileMode.Open, FileAccess.Read));
                hp = br.ReadInt32();
                size = br.ReadInt32();
                speed = br.ReadInt32();
                shotSize = br.ReadInt32();
                shotSpeed = br.ReadInt32();
                attackDelay = br.ReadInt32();
                br.Close();
            }
            else
            {
                br = new BinaryReader(new FileStream("SpelareStats.dat", FileMode.Open, FileAccess.Read));
                hp = br.ReadInt32();
                size = br.ReadInt32();
                speed = br.ReadInt32();
                shotSize = br.ReadInt32();
                shotSpeed = br.ReadInt32();
                attackDelay = br.ReadInt32();
                br.Close();
            }
        }

        private void SaveData(BinaryWriter bw)
        {
            bw = new BinaryWriter(new FileStream("SpelareStats.dat", FileMode.OpenOrCreate, FileAccess.Write));
            bw.Write(hp);
            bw.Write(size);
            bw.Write(speed);
            bw.Write(shotSize);
            bw.Write(shotSpeed);
            bw.Write(attackDelay);
            bw.Close();
        }

        public override void Update(Karta karta)
        {
            CheckWallCollision(karta);
            Move();
            //Attack
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && attackDelay <= 0)
            {
                Attack();
                attackDelay = 20;
            }
            attackDelay--;
            for (int i = 0; i < projectileList.Count; i++)
            {
                projectileList[i].Update(karta);
                if (projectileList[i].IsDead)
                {
                    projectileList.RemoveAt(i);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                SaveData(bw);
            }
            base.Update(karta);
        }

        private void Attack()
        {
            projectileList.Add(new Shot(shotTex, new Vector2(position.X + size / 2, position.Y + size / 2), shotSpeed, shotSize));
        }

        public override void Move()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.W) && position.Y > 0 && (collisionDir & CollisionDir.North) != CollisionDir.North)
            {
                position.Y -= speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && position.Y < Game1.window.Height-size && (collisionDir & CollisionDir.South) != CollisionDir.South)
            {
                position.Y += speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A) && position.X > 0 && (collisionDir & CollisionDir.West) != CollisionDir.West)
            {
                position.X -= speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) && position.X < Game1.window.Width-size && (collisionDir & CollisionDir.East) != CollisionDir.East)
            {
                position.X += speed;
            }
        }
        private void CheckWallCollision(Karta karta)
        {
            collisionDir = CollisionDir.None;
            foreach (Grid grid in karta.gridArray)
            {
                //Check wall collision
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    hitboxTemp = new Rectangle((int)position.X, (int)(position.Y - speed), size, size);
                    if (hitboxTemp.Intersects(grid.GridBox) && grid.isSolid)
                    {
                        collisionDir |= CollisionDir.North;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    hitboxTemp = new Rectangle((int)(position.X - speed), (int)position.Y, size, size);
                    if (hitboxTemp.Intersects(grid.GridBox) && grid.isSolid)
                    {
                        collisionDir |= CollisionDir.West;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    hitboxTemp = new Rectangle((int)position.X, (int)(position.Y + speed), size, size);
                    if (hitboxTemp.Intersects(grid.GridBox) && grid.isSolid)
                    {
                        collisionDir |= CollisionDir.South;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    hitboxTemp = new Rectangle((int)(position.X + speed), (int)position.Y, size, size);
                    if (hitboxTemp.Intersects(grid.GridBox) && grid.isSolid)
                    {
                        collisionDir |= CollisionDir.East;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur, hitbox, Color.White);
            for (int i = 0; i < projectileList.Count; i++)
            {
                projectileList[i].Draw(spriteBatch);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    abstract class Projectile
    {
        protected int speed;
        protected int size;
        public int dmg;
        protected Texture2D tex;
        protected Vector2 pos { get; set; }
        protected Vector2 direction;
        public Rectangle Hitbox { get; set; }
        public bool IsDead { get; set; }

        public Projectile(Texture2D tex, Vector2 pos, int speed, int size, int dmg)
        {
            IsDead = false;
            this.tex = tex;
            this.pos = pos;
            this.speed = speed;
            this.size = size;
            this.dmg = dmg;
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, size, size);
            Direction();
        }

        public virtual void Update(Karta karta)
        {
            Move(karta);
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, size, size);
        }
        //Förflyttar projectile
        public virtual void Move(Karta karta)
        {
            pos += direction * speed;
        }
        //Bestämmer riktning på projectile
        public virtual void Direction()
        {
            MouseState state = Mouse.GetState();
            direction = new Vector2(state.X, state.Y)-pos;
            direction.Normalize();
        }
        //Ritar ut projectile
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, Hitbox, Color.White);
        }
    }
}

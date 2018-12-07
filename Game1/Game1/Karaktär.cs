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
    public interface IMove
    {
        void Move();
    }
    public interface IAttack
    {
        void Attack();
    }

    abstract class Karaktär
    {
        protected Texture2D textur;
        protected Texture2D shotTex;
        protected Rectangle hitbox { get; set; }
        protected Rectangle hitboxTemp { get; set; }
        protected Vector2 position;
        protected int size;
        protected int hp;
        protected float speed;
        protected List<Projectile> projectileList;
        protected int shotSpeed;
        protected int shotSize;
        protected int attackDelay;
        protected CollisionDir collisionDir;
        [Flags]
        protected enum CollisionDir
        {
            None = 0,
            North = 1,
            South = 2,
            West = 4,
            East = 8
        }

        public Karaktär(Texture2D tex, Vector2 pos)
        {
            position = pos;
            hitbox = new Rectangle((int)position.X, (int)position.Y, size, size);
            textur = tex;
        }

        public virtual void Update(Karta karta)
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, size, size);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur,hitbox,Color.Black);
        }
    }
}

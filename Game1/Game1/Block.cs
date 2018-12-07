using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Block
    {
        Texture2D texture;
        Vector2 pos;
        int size;
        Rectangle hitbox;

        public Block(Texture2D tex, Vector2 pos, int size)
        {
            texture = tex;
            this.size = size;
            this.pos = pos;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, size, size);
        }

        public Rectangle Hitbox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
    }
}

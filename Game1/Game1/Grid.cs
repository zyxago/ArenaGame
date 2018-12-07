using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Grid
    {
        Vector2 posistion;
        Texture2D textur;
        public Rectangle GridBox { get; set; }
        public bool isSolid { get; set; }

        public Grid(Vector2 pos, Texture2D tex, bool isSolid)
        {
            this.isSolid = isSolid;
            posistion = pos;
            textur = tex;
            GridBox = new Rectangle((int)posistion.X, (int)posistion.Y, 40, 40);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur, GridBox, Color.White);
        }
    }
}

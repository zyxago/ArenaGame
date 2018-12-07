using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Karta
    {
        Texture2D kartaTex;
        Vector2 kartaPos;
        Rectangle kartaBox;
        Texture2D stenTex, markTex;
        public Grid[,] gridArray { get; set; }

        public Karta(Texture2D tex, Vector2 pos, Texture2D stenTex, Texture2D markTex)
        {
            kartaTex = tex;
            kartaPos = pos;
            kartaBox = new Rectangle((int)pos.X, (int)pos.Y, 800, 500);
            this.stenTex = stenTex;
            this.markTex = markTex;
            gridArray = new Grid[20, 12];
            GridArray(gridArray);
            
        }

        private void GridArray(Grid[,] gridArray)
        {
            //Hämta bildpixlar
            Color[] data = new Color[kartaTex.Width * kartaTex.Height];
            kartaTex.GetData(data);
            Vector2 gridPos = new Vector2(0, 0);
            for (int x = 0; x < kartaTex.Width; x++)
            {
                for (int y = 0; y < kartaTex.Height; y++)
                {
                    if(data[y * kartaTex.Width + x] == Color.Black)
                    {
                        gridPos = new Vector2(x*40, y*40);
                        gridArray[x, y] = new Grid(gridPos, stenTex, true);
                    }
                    else if(data[y * kartaTex.Width + x] == Color.White)
                    {
                        gridPos = new Vector2(x * 40, y * 40);
                        gridArray[x, y] = new Grid(gridPos, markTex, false);
                    }
                }
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(kartaTex, kartaBox, Color.White);
            for(int i = 0; i < 20; i++)
            {
                for(int j = 0; j < 12; j++)
                {
                    gridArray[i, j].Draw(spriteBatch);
                }
            }
        }
    }
}

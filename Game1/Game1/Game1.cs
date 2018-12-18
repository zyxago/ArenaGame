using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        enum Meny
        {
            main,
            pause,
            play,
            gameover,
            restart
        }
        public static Rectangle window;
        public static Random rng;
        Meny gameState = Meny.main;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Karta karta;
        List<Karaktär> karaktärsList = new List<Karaktär>();
        Texture2D spelareTex, stenTex, markTex, shotTex, kartaTex, enemyTex;
        Vector2 StartPos;
        SpriteFont font1;
        int score, enemyHP, spawnTimer, tempSpawnTimer;
        List<int> scoretable = new List<int>();
        BinaryWriter bw;
        BinaryReader br;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            IsMouseVisible = true;
            karta = new Karta(kartaTex, new Vector2(0, 0), stenTex, markTex);
            window = Window.ClientBounds;
            rng = new Random();
            spawnTimer = 60;
            br = new BinaryReader(new FileStream("scoretable.dat", FileMode.OpenOrCreate, FileAccess.Read));
            enemyHP = 1;
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                scoretable.Add(br.ReadInt32());
            }
            br.Close();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spelareTex = Content.Load<Texture2D>("Karaktärer/karaktär");
            enemyTex = Content.Load<Texture2D>("Karaktärer/Enemy");
            stenTex = Content.Load<Texture2D>("Miljö/stenBlock");
            markTex = Content.Load<Texture2D>("Miljö/markBlock");
            kartaTex = Content.Load<Texture2D>("Miljö/map");
            shotTex = Content.Load<Texture2D>("Projectile/shot");
            font1 = Content.Load<SpriteFont>("font1");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            if(gameState == Meny.main)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    gameState = Meny.play;
                }
                //Difficulity
                else if (Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    enemyHP = 1;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    enemyHP = 2;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D3))
                {
                    enemyHP = 3;
                }
            }
            else if(gameState == Meny.play)
            {
                //Difficulity
                if(score == 10)
                {
                    spawnTimer = 30;
                }
                else if(score == 25)
                {
                    spawnTimer = 20;
                }
                else if(score >= 50)
                {
                    spawnTimer = 10;
                }
                else if(score >= 99)
                {
                    spawnTimer = 5;
                }
                if(karaktärsList.Count == 0)//Lägger till spelaren
                {
                    StartPos = new Vector2(80,80);
                    karaktärsList.Add(new Spelare(spelareTex, StartPos, shotTex));
                }
                if(tempSpawnTimer <= 0)//Lägger till fiender
                {
                    karaktärsList.Add(new Enemy(enemyTex, new Vector2(window.Width / 2 - 20, window.Height / 2 - 20), enemyHP));
                    tempSpawnTimer = spawnTimer;
                }
                tempSpawnTimer--;
                //Karaktär logic:
                for(int i = 0; i < karaktärsList.Count; i++)
                {
                    karaktärsList[i].Update(karta);
                    if(karaktärsList[i] is Enemy)
                    {
                        if(karaktärsList[i].isDead == true)
                        {
                            karaktärsList.Remove(karaktärsList[i]);
                            score++;
                            i--;
                            continue;
                        }
                        foreach(Karaktär karaktär in karaktärsList)
                        {
                            if (karaktärsList[i].hitbox.Intersects(karaktär.hitbox) && karaktär is Spelare)
                            {
                                karaktär.hp -= karaktärsList[i].dmg;
                            }
                        }
                    }
                    if(karaktärsList[i] is Spelare)
                    {
                        //Check if projectile intersects enemy
                        Spelare spelare = karaktärsList[i] as Spelare;
                        for (int j = 0; spelare.projectileList.Count > j; j++)
                        {
                            foreach(Karaktär karaktär in karaktärsList)
                            {
                                if (spelare.projectileList[j].Hitbox.Intersects(karaktär.hitbox) && karaktär is Enemy)
                                {
                                    karaktär.hp -= spelare.projectileList[j].dmg;
                                    spelare.projectileList[j].IsDead = true;
                                }
                            }
                        }
                        if(spelare.isDead) 
                        {
                            gameState = Meny.gameover;
                        }
                    }
                    if(karaktärsList[i].hp <= 0)
                    {
                        karaktärsList[i].isDead = true;
                    }
                }
                karta.Update();
                base.Update(gameTime);
            }
            else if(gameState == Meny.gameover)
            {
                if(score != 0)
                {
                    scoretable.Add(score);
                    scoretable.Sort((a, b) => -1 * a.CompareTo(b));
                    score = 0;
                }
                bw = new BinaryWriter(new FileStream("scoretable.dat", FileMode.OpenOrCreate, FileAccess.Write));
                for(int i = 0; i < scoretable.Count; i++)
                {
                    bw.Write(scoretable[i]);
                }
                bw.Close();
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    gameState = Meny.play;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    gameState = Meny.main;
                }
                karaktärsList.Clear();
                spawnTimer = 60;
            }
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if(gameState == Meny.main)
            {
                spriteBatch.DrawString(font1, "Press 'Enter' to start", new Vector2(300,100), Color.Black);
                spriteBatch.DrawString(font1, "Highscore", new Vector2(150, 120), Color.Black);
                spriteBatch.DrawString(font1, "Press '1' '2' or '3' to change difficulity", new Vector2(300, 140), Color.Black);
                for (int i = 0; i < scoretable.Count; i++)
                {
                    spriteBatch.DrawString(font1, (i+1)+": "+scoretable[i],new Vector2(150, 150+(i*20)),Color.Black);
                    if(i >= 9)
                    {
                        break;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    spriteBatch.DrawString(font1, "Difficulity changed!", new Vector2(300, 160), Color.Black);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    spriteBatch.DrawString(font1, "Difficulity changed!", new Vector2(300, 160), Color.Black);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D3))
                {
                    spriteBatch.DrawString(font1, "Difficulity changed!", new Vector2(300, 160), Color.Black);
                }
            }
            else if(gameState == Meny.play)
            {
                karta.Draw(spriteBatch);
                spriteBatch.DrawString(font1, "Score: " + score, new Vector2(10, 10), Color.Black);
                foreach (Karaktär karaktär in karaktärsList)
                {
                    karaktär.Draw(spriteBatch);
                }
            }
            else if(gameState == Meny.gameover)
            {
                karta.Draw(spriteBatch);
                spriteBatch.DrawString(font1, "GAMEOVER", new Vector2(300, 200), Color.Black);
                spriteBatch.DrawString(font1, "Press 'R' to restart", new Vector2(280, 220), Color.Black);
                spriteBatch.DrawString(font1, "Press 'E' to enter main menu", new Vector2(280, 240), Color.Black);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
    }
}

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
            play
        }
        public static Rectangle window;
        public static Random rng;
        Meny gameState = Meny.main;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Karta karta;
        List<Karaktär> karaktärsList = new List<Karaktär>();
        Texture2D spelareTex;
        Texture2D stenTex, markTex;
        Texture2D shotTex;
        Texture2D kartaTex;
        Vector2 StartPos = new Vector2(80, 80);
        SpriteFont font1;

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
            karaktärsList.Add(new Spelare(spelareTex, StartPos, shotTex));
            karta = new Karta(kartaTex, new Vector2(0, 0), stenTex, markTex);
            window = Window.ClientBounds;
            rng = new Random();
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
            stenTex = Content.Load<Texture2D>("Miljö/stenBlock");
            markTex = Content.Load<Texture2D>("Miljö/markBlock");
            kartaTex = Content.Load<Texture2D>("Miljö/map");
            shotTex = Content.Load<Texture2D>("Miljö/stenBlock"); //Temporary
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
                    gameState = Meny.play;
            }
            else if(gameState == Meny.play)
            {
                if(karaktärsList.Count <= 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        karaktärsList.Add(new Enemy(spelareTex, new Vector2(400, 200)));
                    }
                }
                foreach(Karaktär karaktär in karaktärsList)
                {
                    karaktär.Update(karta);
                    if(karaktär is Enemy)
                    {
                        foreach(Karaktär karaktär2 in karaktärsList)
                        {
                            if (karaktär.hitbox.Intersects(karaktär2.hitbox) && karaktär2 is Spelare)
                            {
                                karaktär2.hp -= karaktär.dmg;
                            }
                        }
                    }
                    if(karaktär.hp <= 0)
                    {
                        //karaktär ISDEAD
                    }
                }
                karta.Update();
                base.Update(gameTime);
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
                spriteBatch.DrawString(font1, "Press Enter to start", new Vector2(300,200), Color.Black);
            }
            else if(gameState == Meny.play)
            {
                karta.Draw(spriteBatch);
                foreach (Karaktär karaktär in karaktärsList)
                {
                    karaktär.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
    }
}

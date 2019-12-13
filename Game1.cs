using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Galaga
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D ship;
        Player player;

        List<Projectile> bullets;
        KeyboardState kbOld;

        StarBackground starBackgroundObject;
        MainMenu mainMenuObject;

        Rectangle[] lives;

        public int currentMenu;

        Boolean menuChangeOnFrame;

        GameOverScreen GAMEOVER;
        private int hits;
        private int shots;

        public int Score;

        long timer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 580;
            IsMouseVisible = true;
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
            player = new Player();
            bullets = new List<Projectile>();
            kbOld = Keyboard.GetState();
            currentMenu = 0;
            menuChangeOnFrame = false;
            timer = 0;
            hits = 0;
            shots = 0;

            lives = new Rectangle[]
            {
                new Rectangle(25,680,35,35),
                new Rectangle(65,680,35,35)
            };

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ship = Content.Load<Texture2D>("pShip");
            // TODO: use this.Content to load your game content here
            starBackgroundObject = new StarBackground(spriteBatch, GraphicsDevice, Content);
            starBackgroundObject.Initialize();
            starBackgroundObject.LoadContent();
            mainMenuObject = new MainMenu(spriteBatch, GraphicsDevice, Content);
            GAMEOVER = new GameOverScreen(spriteBatch, GraphicsDevice, Content, shots, hits);
            mainMenuObject.Initialize();
            mainMenuObject.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            KeyboardState kb = Keyboard.GetState();
            menuChangeOnFrame = false;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            if(currentMenu == 0)
            {
                mainMenuObject.Update();
                if(mainMenuObject.buttonColors[0] == Color.Green && kb.IsKeyDown(Keys.Space) && !kbOld.IsKeyDown(Keys.Space))
                {
                    currentMenu = 1;
                    menuChangeOnFrame = true;
                }
            }

            if(currentMenu == 1)
            {
                if (kb.IsKeyDown(Keys.Space) && kbOld.IsKeyUp(Keys.Space) && player.bullets > 0 && menuChangeOnFrame == false)
                {
                    bullets.Add(new Projectile(player.getRectangle(), 1, new Vector2(0, -12), 0, GraphicsDevice, Content));
                    player.bullets -= 1;
                }

                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    bullets[i].UpdatePos();
                    if (!bullets[i].OnScreen())
                        bullets.Remove(bullets[i]);
                }

                if (timer == 30)
                {
                    if (player.bullets < 2)
                    {
                        player.bullets++;
                    }
                    timer = 0;
                }
                if (currentMenu == 2 && kb.IsKeyDown(Keys.Space))
                {
                    currentMenu = 0;
                }

                if (kb.IsKeyDown(Keys.A) && menuChangeOnFrame == false)
                {
                    player.moveLeft();
                }

                if (kb.IsKeyDown(Keys.D) && menuChangeOnFrame == false)
                {
                    player.moveRight();
                }

                if (kb.IsKeyDown(Keys.Right) && menuChangeOnFrame == false)
                {
                    player.moveRight();
                }
                if (kb.IsKeyDown(Keys.Left) && menuChangeOnFrame == false)
                {
                    player.moveLeft();
                }
                if (kb.IsKeyDown(Keys.U))
                {
                    currentMenu = 2;
                }
            }

            if (starBackgroundObject.starBackgroundDisplayed == true)
            {
                starBackgroundObject.Update();
            }

            kbOld = kb;
            if (player.bullets < 2)
                timer++;
            else
                timer = 0;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (starBackgroundObject.starBackgroundDisplayed == true)
            {
                starBackgroundObject.Draw();
            }

            if(currentMenu == 0)
            {
                mainMenuObject.Draw();
            }

            if(currentMenu == 1)
            {
                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    bullets[i].Draw(spriteBatch);
                }

                for (int i = 0; i < player.lives - 1; i++)
                {
                    spriteBatch.Draw(ship, lives[i], Color.White);
                }

                spriteBatch.Draw(ship, player.getRectangle(), Color.White);
                
            }
            if (currentMenu == 2)
            {
                GAMEOVER.Draw();
            }
                spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

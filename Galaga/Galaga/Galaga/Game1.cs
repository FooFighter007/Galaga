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
        public Player player;

        Random rand = new Random();

        public List<Projectile> bullets;
        KeyboardState kbOld;

        StarBackground starBackgroundObject;
        MainMenu mainMenuObject;

        EnemyMovement em;

        public int currentMenu;
        public int maxEnemiesPerRound;
        public int maxSpeed;
        public int diveFreq;
        public int diveChance;

        int chance;

        Boolean menuChangeOnFrame;

        GameOverScreen GAMEOVER;
        GameOverlay gOverlay;

        long timer;
        double spawnTimer;
        double diveTimer;

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
            em = new EnemyMovement(this);
            player = new Player();
            bullets = new List<Projectile>();
            kbOld = Keyboard.GetState();
            currentMenu = 0;
            menuChangeOnFrame = false;
            timer = 0;
            spawnTimer = 0;
            diveTimer = 0;

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
            GAMEOVER = new GameOverScreen(this, spriteBatch);
            gOverlay = new GameOverlay(this, spriteBatch, ref player);
            gOverlay.LoadContent(Content);
            em.enemy1 = Content.Load<Texture2D>("GalagaEnemy1");
            em.enemy2 = Content.Load<Texture2D>("GalagaEnemy2");
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

                    //Difficulty Changes
                    maxEnemiesPerRound = 14;
                    maxSpeed = 5;
                    diveFreq = 90;
                    diveChance = 2; 
                }
            }

            if(currentMenu == 1)
            {
                if (kb.IsKeyDown(Keys.Space) && kbOld.IsKeyUp(Keys.Space) && player.bullets > 0 && menuChangeOnFrame == false)
                {
                    bullets.Add(new Projectile(player.getRectangle(), 1, new Vector2(0, -12), 0, Content, GraphicsDevice));
                    player.bullets -= 1;
                    player.shots++;
                }

                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    bullets[i].UpdatePos();
                    if (!bullets[i].OnScreen())
                        bullets.Remove(bullets[i]);

                    if (bullets[i].missileType == 1)
                    {
                        for (int f = 0; f < em.enemies.Count; f++)
                        {
                            if (bullets[i].IntersectingRectangle(em.enemies[f].enemyPos) && !em.enemies[f].isHit)
                            {
                                em.enemies[f].Hit();
                                bullets.Remove(bullets[i]);
                                player.hits++;
                                break;
                            }
                        }
                    }
                    else if (bullets[i].IntersectingRectangle(player.getRectangle()))
                    {
                        player.lives--;
                        bullets.Remove(bullets[i]);
                        if (player.lives == 0)
                        {
                            currentMenu = 2;
                        }
                        break;
                    }
                }

                if (timer == 30)
                {
                    if (player.bullets < 2)
                    {
                        player.bullets++;
                    }
                    timer = 0;
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

                //Random Enemies Entering
                if(spawnTimer == 180)
                {
                    chance = rand.Next(0,2);
                    if(chance == 0)
                        em.RandomAddEnemy(maxEnemiesPerRound, maxSpeed);
                    spawnTimer = 0;
                }

                if (diveTimer == diveFreq)
                {
                    if (rand.Next(0, diveChance + 1) == 0)
                    {
                        em.RandomDive();
                    }
                    diveTimer = 0;
                }

                //Updates Enemy Spawn Timer
                spawnTimer++;
                diveTimer++;
                //Updates EnemyMovement
                em.update();


            }
            else if (currentMenu == 2 && kb.IsKeyDown(Keys.Space))
            {
                player.newGame();
                em.StartNewRound();
                currentMenu = 0;
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

                spriteBatch.Draw(ship, player.getRectangle(), Color.White);
                em.draw(spriteBatch);
            }

            if (currentMenu != 0)
                gOverlay.Render();

            if (currentMenu == 2)
            {
                GAMEOVER.Draw();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

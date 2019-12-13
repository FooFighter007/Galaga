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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (kb.IsKeyDown(Keys.Space) && kbOld.IsKeyUp(Keys.Space))
            {
                bullets.Add(new Projectile(player.getRectangle(), 1, new Vector2(0,-5), 0, GraphicsDevice, Content));
            }

            for (int i = bullets.Count -1; i >= 0; i--)
            {
                bullets[i].UpdatePos();
                if (!bullets[i].OnScreen())
                    bullets.Remove(bullets[i]);
            }

            if (kb.IsKeyDown(Keys.A))
            {
                player.moveLeft();
            }

            if (kb.IsKeyDown(Keys.D))
            {
                player.moveRight();
            }

            if (kb.IsKeyDown(Keys.Right))
            {
                player.moveRight();
            }
            if (kb.IsKeyDown(Keys.Left))
            {
                player.moveLeft();
            }

            kbOld = kb;
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

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Draw(spriteBatch);
            }

            spriteBatch.Draw(ship,player.getRectangle(),Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

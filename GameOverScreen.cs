using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Galaga
{
    class GameOverScreen
    {
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        ContentManager Content;
        private int shots;
        private int hits;
        SpriteFont GameFont;

        public GameOverScreen()
        {
        }
        
        public GameOverScreen(SpriteBatch temporarySpriteBatch, GraphicsDevice temporaryDevice, ContentManager temporaryContent, int s, int h)
        {
            spriteBatch = temporarySpriteBatch;
            device = temporaryDevice;
            Content = temporaryContent;
            shots = s;
            hits = h;
            LoadContent();
        }

        public void Initialize()
        {
            
        }


        public void LoadContent()
        {
            GameFont = Content.Load<SpriteFont>("ArialSmallSpriteFont");
        }


        public void UnloadContent()
        {
            
        }

        public void Update()
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Space))
            {
                //reset
            }
        }

       public void Draw()
        {
            spriteBatch.DrawString(GameFont,"---RESULTS---",new Vector2(220,280),Color.Red);
            spriteBatch.DrawString(GameFont, "SHOTS FIRED:" + shots + "\nNUMBER OF HITS:" + hits, new Vector2(190, 320), Color.Yellow);
            spriteBatch.DrawString(GameFont, "HIT MISS RATIO:%"+(shots/100*hits), new Vector2(190, 380), Color.White);
            spriteBatch.DrawString(GameFont, "PRESS SPACE TO RESTART", new Vector2(100, 680), Color.White);
        }
    }
}

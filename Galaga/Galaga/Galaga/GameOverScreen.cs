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
            GameFont = Content.Load<SpriteFont>("ClassicLarge");
        }

       public void Draw()
        {
            spriteBatch.DrawString(GameFont,"---RESULTS---",new Vector2(200,280),Color.Red);
            spriteBatch.DrawString(GameFont, "SHOTS  FIRED:  " + shots + "\nNUMBER  OF  HITS:  " + hits, new Vector2(170, 320), Color.Yellow);
            double ratio = 0;
            if (hits > 0)
                ratio = (double) shots / hits;
            spriteBatch.DrawString(GameFont, "HIT  MISS  RATIO:  " + ratio + "%", new Vector2(170, 380), Color.White);
            spriteBatch.DrawString(GameFont, "PRESS  SPACE  TO  RESTART", new Vector2(140, 680), Color.White);
        }
    }
}

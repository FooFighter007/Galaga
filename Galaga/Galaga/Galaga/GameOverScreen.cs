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
        SpriteFont GameFont;
        Game1 game;

        public GameOverScreen()
        {
        }
        
        public GameOverScreen(Game1 g, SpriteBatch temporarySpriteBatch)
        {
            spriteBatch = temporarySpriteBatch;
            game = g;
            LoadContent();
        }

        public void LoadContent()
        {
            GameFont = game.Content.Load<SpriteFont>("ClassicLarge");
        }

       public void Draw()
        {
            spriteBatch.DrawString(GameFont,"---RESULTS---",new Vector2(200,280),Color.Red);
            spriteBatch.DrawString(GameFont, "SHOTS  FIRED:  " + game.player.shots + "\nNUMBER  OF  HITS:  " + game.player.hits, new Vector2(170, 320), Color.Yellow);
            double ratio = 0;
            if (game.player.shots > 0)
                ratio = Math.Round((double) game.player.hits / game.player.shots * 100, 2);
            spriteBatch.DrawString(GameFont, "HIT  MISS  RATIO:  " + ratio + "%", new Vector2(170, 380), Color.White);
            spriteBatch.DrawString(GameFont, "PRESS  SPACE  TO  RESTART", new Vector2(140, 680), Color.White);
        }
    }
}

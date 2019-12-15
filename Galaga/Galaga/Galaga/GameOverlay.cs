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
    class GameOverlay
    {

        Game1 game;
        SpriteBatch sb;
        Rectangle[] lives;
        Rectangle[] bullets;
        Player player;
        Texture2D ship;
        Texture2D b;

        SpriteFont large;

        Vector2[] positions;

        public GameOverlay(Game1 g, SpriteBatch s, ref Player p)
        {
            game = g;
            sb = s;
            player = p;
            Setup();
        }

        public void Setup()
        {
            lives = new Rectangle[]
            {
                new Rectangle(25,680,35,35),
                new Rectangle(65,680,35,35)
            };

            bullets = new Rectangle[]
            {
                new Rectangle(525,25,10,20),
                new Rectangle(500,25,10,20)
            };

            positions = new Vector2[]
            {
                new Vector2(15,15),
                new Vector2(15,40),
                new Vector2(230,15),
                new Vector2(230,40),
            };
        }

        public void LoadContent(ContentManager cm)
        {
            ship = cm.Load<Texture2D>("pShip");
            b = cm.Load<Texture2D>("pMissile");
            large = cm.Load<SpriteFont>("ClassicLarge");
        }

        public void Render()
        {
            for (int i = 0; i < player.lives - 1; i++)
            {
                sb.Draw(ship, lives[i], Color.White);
            }

            for (int i = 0; i < player.bullets; i++)
            {
                sb.Draw(b, bullets[i], Color.White);
            }

            sb.DrawString(large, "SCORE", positions[0], Color.Red);
            sb.DrawString(large, "" + player.score, positions[1], Color.White);
            sb.DrawString(large, "HIGHSCORE", positions[2], Color.Red);
            sb.DrawString(large, "" + player.highScore, positions[3], Color.White);
            sb.DrawString(large, "LEVEL:  " + game.level.ToString(), new Vector2(440, 680), Color.White);
        }
    }
}

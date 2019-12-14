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
    class Player
    {
        //580 width 720 height
        //player 50x50

        private Rectangle player;
        public int lives;
        public int bullets;

        public Player()
        {
            player = new Rectangle(230, 595, 50, 50);
            lives = 3;
            bullets = 2;
        }

        public Rectangle getRectangle()
        {
            return player;
        }

        public void moveRight()
        {
            if (player.X < 530)
            {
                player.X += 4;
            }
        }
        public void moveLeft()
        {
            if (player.X > 0)
            {
                player.X -= 4;
            }
        }
    }
}

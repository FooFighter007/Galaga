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
        //560 width 720 height
        //player 50x50
        private Rectangle player;
        public Player()
        {
            player = new Rectangle(230, 670, 50, 50);
        }

        public Rectangle getRectangle()
        {
            return player;
        }

        public void moveRight()
        {
            player.X++;
        }
        public void moveLeft()
        {
            player.X--;
        }
    }
}

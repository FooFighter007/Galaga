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
    class Star
    {
        GraphicsDevice device;

        ContentManager Content;

        Texture2D starTexture;

        Color starColor;

        Rectangle starRect;

        public int starType;

        int starTimer;
        int twinkleTime;

        public Star()
        {

        }

        public Star(GraphicsDevice temporaryDevice, ContentManager temporaryContent, Color temporaryStarColor, Rectangle temporaryStarRect, int temporaryStarType, int temporaryTwinkleTime)
        {
            device = temporaryDevice;
            Content = temporaryContent;
            starColor = temporaryStarColor;
            starRect = temporaryStarRect;
            starType = temporaryStarType;
            twinkleTime = temporaryTwinkleTime;
            starTimer = 0;
        }

        public void LoadContent()
        {
            starTexture = Content.Load<Texture2D>("White Circle");
        }

        public void Update()
        {
            if (starRect.Y > device.Viewport.Height)
            {
                starRect.Y = 0 - (starRect.Height * 2);
            }
            starRect.Y += 3;

            if (starTimer >= 121)
            {
                starTimer = 0;
            }
            else
            {
                starTimer++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (starType == 1)
            {
                if (starTimer < twinkleTime || starTimer >= twinkleTime + 60)
                {
                    spriteBatch.Draw(starTexture, starRect, starColor);
                }
            }
            else
            {
                spriteBatch.Draw(starTexture, starRect, starColor);
            }
        }
    }
}
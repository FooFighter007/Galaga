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
    class StarBackground
    {
        SpriteBatch spriteBatch;

        GraphicsDevice device;

        ContentManager Content;

        Texture2D whiteTexture;

        Rectangle wholeScreenRect;

        public Boolean starBackgroundDisplayed;

        public int numberOfStars;

        int randomStarType;
        int randomTwinkleTime;

        Random rnd;

        List<Star> starList = new List<Star>();

        public StarBackground()
        {

        }

        public StarBackground(SpriteBatch temporarySpriteBatch, GraphicsDevice temporaryDevice, ContentManager temporaryContent)
        {
            spriteBatch = temporarySpriteBatch;
            device = temporaryDevice;
            Content = temporaryContent;
        }

        public void Initialize()
        {
            wholeScreenRect = new Rectangle(0, 0, device.Viewport.Width, device.Viewport.Height);
            starBackgroundDisplayed = true;
            rnd = new Random();
            numberOfStars = 150;
            randomStarType = 0;
            randomTwinkleTime = 0;
            for (int x = 0; x < numberOfStars; x++)
            {
                if (rnd.Next(0, 9) < 6)
                {
                    randomStarType = 0;
                }
                else
                {
                    randomStarType = 1;
                }
                randomTwinkleTime = 20 * rnd.Next(1, 4);
                starList.Add(new Star(device, Content, new Color(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), new Rectangle(rnd.Next(0, device.Viewport.Width - 5), rnd.Next(0, device.Viewport.Height - 5), 5, 5), randomStarType, randomTwinkleTime));
            }
        }

        public void LoadContent()
        {
            whiteTexture = Content.Load<Texture2D>("White");
            for (int x = 0; x < starList.Count; x++)
            {
                starList[x].LoadContent();
            }
        }

        public void Update()
        {
            for (int x = 0; x < starList.Count; x++)
            {
                starList[x].Update();
            }
        }

        public void Draw()
        {
            spriteBatch.Draw(whiteTexture, wholeScreenRect, Color.Black);
            for (int x = 0; x < starList.Count; x++)
            {
                starList[x].Draw(spriteBatch);
            }
        }
    }
}

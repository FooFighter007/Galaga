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
    class MainMenu
    {
        GraphicsDevice device;

        ContentManager Content;

        SpriteBatch spriteBatch;

        Texture2D galagaLogoTexture;
        Texture2D copyrightSymbolTexture;

        SpriteFont arialBigFont;
        SpriteFont arialSmallFont;

        Rectangle galagaLogoRect;
        Rectangle copyrightSymbolRect;

        public Color[] buttonColors;
        String[] buttonText;
        Vector2[] buttonLocations;
        Rectangle[] enemyRects;
        Texture2D[] enemyTextures;

        public int buttonSelected;

        Boolean buttonChangeOnFrame;

        KeyboardState kb;
        KeyboardState oldKb;

        int timerValue;
        int animation;

        Boolean animationChangeOnFrame;

        public MainMenu()
        {

        }

        public MainMenu(SpriteBatch temporarySpriteBatch, GraphicsDevice temporaryDevice, ContentManager temporaryContent)
        {
            spriteBatch = temporarySpriteBatch;
            device = temporaryDevice;
            Content = temporaryContent;
        }
        
        public void Initialize()
        {
            galagaLogoRect = new Rectangle((device.Viewport.Width / 2) - (1281 / 8), (device.Viewport.Height / 4) - (663 / 8), 1281 / 4, 663 / 4);
            copyrightSymbolRect = new Rectangle(138, 645, 20, 20);
            buttonSelected = 0;
            buttonColors = new Color[3];
            for(int x = 0; x < buttonColors.Length; x++)
            {
                buttonColors[x] = Color.Red;
            }
            buttonText = new String[3];
            buttonText[0] = "PLAY GAME";
            buttonText[1] = "HIGH SCORES";
            buttonText[2] = "CREDITS";
            buttonLocations = new Vector2[3];
            buttonLocations[0] = new Vector2(225, 367);
            buttonLocations[1] = new Vector2(213, 400);
            buttonLocations[2] = new Vector2(238, 433);
            buttonChangeOnFrame = false;
            kb = Keyboard.GetState();
            oldKb = Keyboard.GetState();
            enemyRects = new Rectangle[2];
            enemyRects[0] = new Rectangle(163, 360, 35, 35);    //+35 Y for each button
            enemyRects[1] = new Rectangle(363, 360, 35, 35);
            timerValue = 0;
            animation = 0;
            animationChangeOnFrame = false;
        }

        public void LoadContent()
        {
            galagaLogoTexture = Content.Load<Texture2D>("GalagaLogo");
            copyrightSymbolTexture = Content.Load<Texture2D>("copyright-128");
            arialBigFont = Content.Load<SpriteFont>("ArialBigSpriteFont");
            arialSmallFont = Content.Load<SpriteFont>("ArialSmallSpriteFont");
            enemyTextures = new Texture2D[2];
            enemyTextures[0] = Content.Load<Texture2D>("GalagaEnemy1");
            enemyTextures[1] = Content.Load<Texture2D>("GalagaEnemy2");
        }

        public void Update()
        {
            kb = Keyboard.GetState();
            buttonChangeOnFrame = false;
            animationChangeOnFrame = false;

            if((buttonSelected == 0 || buttonSelected == 1) && kb.IsKeyDown(Keys.Down) && !oldKb.IsKeyDown(Keys.Down) && buttonChangeOnFrame == false)
            {
                buttonSelected++;
                buttonChangeOnFrame = true;
            }
            if((buttonSelected == 1 || buttonSelected == 2) && kb.IsKeyDown(Keys.Up) && !oldKb.IsKeyDown(Keys.Up) && buttonChangeOnFrame == false)
            {
                buttonSelected--;
                buttonChangeOnFrame = true;
            }
            if(buttonSelected == 0 && kb.IsKeyDown(Keys.Up) && !oldKb.IsKeyDown(Keys.Up) && buttonChangeOnFrame == false)
            {
                buttonSelected = 2;
                buttonChangeOnFrame = true;
            }
            if(buttonSelected == 2 && kb.IsKeyDown(Keys.Down) && !oldKb.IsKeyDown(Keys.Down) && buttonChangeOnFrame == false)
            {
                buttonSelected = 0;
                buttonChangeOnFrame = true;
            }

            for (int x = 0; x < buttonColors.Length; x++)
            {
                buttonColors[x] = Color.Red;
            }
            buttonColors[buttonSelected] = Color.Green;

            if(timerValue % 30 == 0 && timerValue != 0)
            {
                if(animation == 0 && animationChangeOnFrame == false)
                {
                    animation = 1;
                    animationChangeOnFrame = true;
                }
                if(animation == 1 && animationChangeOnFrame == false)
                {
                    animation = 0;
                    animationChangeOnFrame = true;
                }
            }

            for(int x = 0; x < enemyRects.Length; x++)
            {
                enemyRects[x].Y = 360 + (35 * buttonSelected);
            }

            timerValue++;
            oldKb = kb;
        }

        public void Draw()
        {
            spriteBatch.Draw(galagaLogoTexture, galagaLogoRect, Color.White);
            spriteBatch.DrawString(arialBigFont, "2019", new Vector2(154, 210), Color.Red);
            spriteBatch.DrawString(arialSmallFont, "2019 GALAGA GROUP GAMES", new Vector2(162, 645), Color.White);
            spriteBatch.DrawString(arialSmallFont, "PROGRAM BY CIARAN VAILLE, CHRISTIAN WHITE,\n        DYLAN ROGERS, XANDER PHAM-ROJAS", new Vector2(56, 670), Color.White);
            spriteBatch.Draw(copyrightSymbolTexture, copyrightSymbolRect, Color.White);
            for(int x = 0; x < buttonText.Length; x++)
            {
                spriteBatch.DrawString(arialSmallFont, buttonText[x], buttonLocations[x], buttonColors[x]);
            }
            for(int x = 0; x < enemyRects.Length; x++)
            {
                spriteBatch.Draw(enemyTextures[animation], enemyRects[x], Color.White);
            }
        }
    }
}

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
    class Projectile
    {

        private int missileType;        //1 is Player, 2 is Alien.
        private Texture2D missile;
        private Rectangle rec;
        private int rotation;
        private Vector2 velocityVector;
        private Vector2 positionVector;
        private GraphicsDevice device;
        private ContentManager Content;


        public Projectile(Rectangle shooter, int type, Vector2 temporaryVel, int rot, GraphicsDevice tempDevice, ContentManager tempContent)
        {
            missileType = type;
            velocityVector = temporaryVel;
            device = tempDevice;
            Content = tempContent;
            rec = new Rectangle(shooter.Center.X, shooter.Top - (type * 5), 10,30);
            positionVector.X = rec.X;
            positionVector.Y = rec.Y;
            rotation = rot;
            LoadContent();
        }

        public void LoadContent()
        {
            if (missileType == 1)
                missile = Content.Load<Texture2D>("pMissile");      //Player
            else
                missile = Content.Load<Texture2D>("eMissile");      //Alien
        }

        public void UpdatePos()
        {
            positionVector.X += velocityVector.X;
            positionVector.Y += velocityVector.Y;
            rec.X = (int)positionVector.X;
            rec.Y = (int)positionVector.Y;
        }

        public bool OnScreen()
        {
            if (rec.X < -5 && rec.X > device.Viewport.Width + 5)
                if (rec.Y < -15 && rec.Y > device.Viewport.Height + 15)
                    return false;

            return true;
        }

        public bool IntersectingRectangle(Rectangle tempRect)
        {
            return rec.Intersects(tempRect);
        }

        public void Draw(SpriteBatch b)
        {
            b.Draw(missile, rec, new Rectangle(0, 0, 190, 380), Color.White, MathHelper.ToRadians(rotation), new Vector2(95, 190), SpriteEffects.None, 1);
        }
    }
}

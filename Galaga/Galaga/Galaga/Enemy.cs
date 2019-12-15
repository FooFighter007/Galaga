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
    class Enemy : Microsoft.Xna.Framework.Game
    {
        //Objects
        public Rectangle enemyPos;
        public Color hit;
        Path activePath;
        public EnemyMovement em;
        Rectangle[] open;
        Random rand = new Random();
        Rectangle choosen;
        Rectangle diveRect;
        Game1 game;

        //timer
        double timer;
        double deathTime;

        //Condition Bools
        bool isDiving;
        bool isHit;
        bool isEntering;
        public bool inFormation;
        bool enteringForm;
        bool hasShot;

        //Postion Floats
        float x1;
        float y1;
        public float rotate;

        //Int Postions Affectors
        int step;
        int speed;
        int slotId;
        int dir;
        int randShoot;
        int health;

        //Construtor
        public Enemy(Game1 g)
        {
            hit = Color.White;
            isDiving = false;
            isHit = false;
            isEntering = false;
            inFormation = false;
            enteringForm = false;
            hasShot = false;

            game = g;

            health = 1;
        }

        //Called to Move enemy into game
        public void EnemyEnter(int s, Path ap)
        {
            activePath = ap;
            speed = s;
            enemyPos = new Rectangle((int)activePath.pathPoints[0].X, (int)activePath.pathPoints[0].Y, 40, 40);
            x1 = enemyPos.X;
            y1 = enemyPos.Y;
            step = 0;
            isEntering = true;
        }

        //Called When Hit
        public void Hit()
        {
            hit = Color.Red;
            health--;

            if (health == 0)
            {
                em.enemies.Remove(this);
            }
        }

        public void Dive()
        {
            dir = rand.Next(0,3);
            inFormation = false;
            isDiving = true;
            if(dir == 0)
                diveRect = new Rectangle(enemyPos.X - speed / 2, 760, speed, speed);
            if (dir == 1)
                diveRect = new Rectangle((enemyPos.X + 100) - speed / 2, 760, speed, speed);
            if (dir == 2)
                diveRect = new Rectangle((enemyPos.X - 100) - speed / 2, 760, speed, speed);
            timer = 0;
            randShoot = rand.Next(15, 46);
        }

        //Main Enemy Update Method
        public void update()
        {
            //When Enemy Entering Play

            if (isEntering == true)
            {
                Vector2 slope = new Vector2((enemyPos.X - activePath.pathPoints[step + 1].X), (enemyPos.Y - activePath.pathPoints[step + 1].Y));
                slope.Normalize();

                x1 -= slope.X * speed;
                y1 -= slope.Y * speed;

                rotate = (float)Math.Atan2(slope.Y, slope.X) + MathHelper.ToRadians(-90);

                enemyPos.X = (int)x1;
                enemyPos.Y = (int)y1;



                if (enemyPos.Intersects(new Rectangle((int)activePath.pathPoints[step + 1].X - speed / 2, (int)activePath.pathPoints[step + 1].Y - speed / 2, speed, speed)))
                {
                    step++;
                }

                if (step == activePath.pathPoints.Count - 1)
                {
                    isEntering = false;
                    enteringForm = true;
                    do
                    {
                        open = em.GetOpenSpots();
                        slotId = rand.Next(0, 27);
                    } while(open[slotId] == new Rectangle(0,0,0,0));
                    choosen = open[slotId];
                    em.spotIndex[slotId] = 1;
                }
            }


            //EnterSlot On Board
            if (enteringForm == true)
            {

                Vector2 slope = new Vector2((enemyPos.X - choosen.X), (enemyPos.Y - choosen.Y));
                slope.Normalize();

                x1 -= slope.X * speed;
                y1 -= slope.Y * speed;

                rotate = (float)Math.Atan2(slope.Y, slope.X) + MathHelper.ToRadians(-90);

                enemyPos.X = (int)x1;
                enemyPos.Y = (int)y1;

                if (enemyPos.Intersects(new Rectangle(choosen.X - speed /2, choosen.Y - speed / 2, speed, speed)))
                {
                    enemyPos.X = choosen.X;
                    enemyPos.Y = choosen.Y;
                    rotate = MathHelper.ToRadians(180);
                    enteringForm = false;
                    inFormation = true;
                }
            }

            //Runs When Diving
            if (isDiving == true)
            {
                Vector2 slope = new Vector2((enemyPos.X - diveRect.X), (enemyPos.Y - diveRect.Y));
                slope.Normalize();

                x1 -= slope.X * speed;
                y1 -= slope.Y * speed;

                rotate = (float)Math.Atan2(slope.Y, slope.X) + MathHelper.ToRadians(-90);

                enemyPos.X = (int)x1;
                enemyPos.Y = (int)y1;


                //!!FOR CIARAN!!
                if (timer == randShoot && hasShot == false)
                {
                    game.bullets.Add(new Projectile(enemyPos, -1, new Vector2(0, 7), 180, game.Content, game.GraphicsDevice));
                    game.bullets.Add(new Projectile(enemyPos, -1, new Vector2(-2, 7), 180+30, game.Content, game.GraphicsDevice));
                    game.bullets.Add(new Projectile(enemyPos, -1, new Vector2(2, 7), 180-35, game.Content, game.GraphicsDevice));
                    hasShot = true;
                }

                //Exits Dive
                if (enemyPos.Y > 720)
                {
                    enemyPos.Y = -40;
                    y1 = -40;
                    isDiving = false;
                    enteringForm = true;
                    hasShot = false;
                }
                timer++;
            }

            //Moves Via Global Offset
            if (inFormation == true)
            {
                enemyPos.X = choosen.X + em.offset;
            }
        }



    }
}

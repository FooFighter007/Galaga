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
        public Rectangle src;
        public Color hit;
        Path activePath;
        public EnemyMovement em;
        Rectangle[] open;
        Random rand = new Random();
        Rectangle choosen;
        Rectangle diveRect;
        Game1 game;
        public Texture2D state;

        //timer
        double timer;
        double deathTime;
        double animTimer;

        //Condition Bools
        public bool isDiving;
        public bool isHit;
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

        //Enemy Textures
        Texture2D enemy1a;
        Texture2D enemy1b;
        Texture2D enemy2a;
        Texture2D enemy2b;
        Texture2D damage;
        int enemyType;

        //Sound
        SoundEffect fire;
        SoundEffect dive;
        SoundEffect kill;

        //Construtor
        public Enemy(Game1 g)
        {
            src = new Rectangle(0, 0,40,40);
            hit = Color.White;
            isDiving = false;
            isHit = false;
            isEntering = false;
            inFormation = false;
            enteringForm = false;
            hasShot = false;
            enemyType = rand.Next(0,2);
            game = g;

        }

        //Called to Move enemy into game
        public void EnemyEnter(int s, Path ap)
        {
            animTimer = 0;
            activePath = ap;
            speed = s;
            enemyPos = new Rectangle((int)activePath.pathPoints[0].X, (int)activePath.pathPoints[0].Y, 40, 40);
            x1 = enemyPos.X;
            y1 = enemyPos.Y;
            step = 0;
            isEntering = true;
            enemy1a = em.enemy1a;
            enemy1b = em.enemy1b;
            enemy2a = em.enemy2a;
            enemy2b = em.enemy2b;
            damage = em.damage;
            kill = em.kill;
            fire = em.firing;
            dive = em.dive;
            if (enemyType == 0)
                state = enemy1a;
            else
                state = enemy2a;           
        }

        //Called When Hit
        public void Hit()
        {
            kill.Play();
            deathTime = 0;
            isHit = true;
        }

        public void Dive()
        {
            dive.Play();
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
                    fire.Play();
                    game.bullets.Add(new Projectile(enemyPos, -1, new Vector2(0, 8), 180, game.Content, game.GraphicsDevice));
                    game.bullets.Add(new Projectile(enemyPos, -1, new Vector2(-2, 8), 180+30, game.Content, game.GraphicsDevice));
                    game.bullets.Add(new Projectile(enemyPos, -1, new Vector2(2, 8), 180-35, game.Content, game.GraphicsDevice));
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

            //Deletes The Object After Time
            if (isHit == true)
            {
                state = damage;
                inFormation = false;
                enteringForm = false;
                isDiving = false;
                isEntering = false;

                if (deathTime == 15)
                {
                    em.enemies.Remove(this);
                }

                src.X = ((int)deathTime / 5) * 40;

                deathTime++;
            }

            if (animTimer / 20 == 1 && isHit == false)
            {
                if (enemyType == 0)
                    state = enemy1b;
                else
                    state = enemy2b;
            }
            if (animTimer == 40 && isHit == false) 
            {
                if (enemyType == 0)
                    state = enemy1a;
                else
                    state = enemy2a;
                animTimer = 0;
            }

            animTimer++;
        }



    }
}

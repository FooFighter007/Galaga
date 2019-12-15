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
    class EnemyMovement : Microsoft.Xna.Framework.Game
    {
        Game1 game;

        //Textures
        public Texture2D enemy1;
        public Texture2D enemy2;

        Texture2D current;

        Path activePath;
        bool isAdding;

        //Temporary Enemy
        Enemy tempenem;
        Enemy tempDive;

        //Global Offset
        public int offset = 0;
        int direction = 1;

        //Adding Enemies
        double timer = 0;
        int x;

        //Paths
        public Path leftZig = new Path();
        public Path rightZig = new Path();
        public Path leftSpiral = new Path();
        public Path rightSpiral = new Path();
        int randomPath = 0;
        int enemyAmount = 0;
        int randomSpeed = 0;

        //Lists of Slots and Enemies
        private Rectangle[] spots = new Rectangle[27];
        public int[] spotIndex = new int[27];
        public List<Enemy> enemies = new List<Enemy>();
        Random rand = new Random();
        
        //Construtor
        public EnemyMovement(Game1 g)
        {
            isAdding = false;
            CreatePaths();
            CreateTable();
            game = g;
        }

        //Creates Top Slots
        void CreateTable()
        {
            int x = 40;
            int y = 100;
            for(int i = 0; i <= 26; i++)
            {
                x += 50;
                if (x == 540)
                {
                    x = 90;
                    y += 50;
                }
                spots[i] = new Rectangle(x, y, 25, 25);
                spotIndex[i] = 0;
            }

            
        }

        //Creates All Paths
        void CreatePaths()
        {
            leftZig.addPoint(-10, 500);
            leftZig.addPoint(50, 450);
            leftZig.addPoint(100, 350);
            leftZig.addPoint(200, 150);
            rightZig.addPoint(570, 500);
            rightZig.addPoint(510, 450);
            rightZig.addPoint(460, 350);
            rightZig.addPoint(360, 150);
            leftSpiral.addPoint(-10,450);
            leftSpiral.addPoint(62, 495);
            leftSpiral.addPoint(122,560);
            leftSpiral.addPoint(200, 580);
            leftSpiral.addPoint(295,540);
            leftSpiral.addPoint(295, 540);
            leftSpiral.addPoint(340, 460);
            leftSpiral.addPoint(315, 340);
            leftSpiral.addPoint(256, 292);
            leftSpiral.addPoint(197, 280);
            leftSpiral.addPoint(110, 307);
            leftSpiral.addPoint(54, 384);
            leftSpiral.addPoint(62, 495);
            leftSpiral.addPoint(122, 560);
            leftSpiral.addPoint(200, 580);
            leftSpiral.addPoint(295, 540);
            leftSpiral.addPoint(295, 540);
            leftSpiral.addPoint(340, 460);
            leftSpiral.addPoint(315, 340);
            leftSpiral.addPoint(250, 400);
            leftSpiral.addPoint(400, 150);
            rightSpiral.addPoint(590, 450);
            rightSpiral.addPoint(518, 495);
            rightSpiral.addPoint(458, 560);
            rightSpiral.addPoint(480, 580);
            rightSpiral.addPoint(275, 540);
            rightSpiral.addPoint(240, 460);
            rightSpiral.addPoint(265, 340);
            rightSpiral.addPoint(280, 292);
            rightSpiral.addPoint(377, 280);
            rightSpiral.addPoint(470, 307);
            rightSpiral.addPoint(526, 384);
            rightSpiral.addPoint(518, 495);
            rightSpiral.addPoint(458, 560);
            rightSpiral.addPoint(480, 580);
            rightSpiral.addPoint(275, 540);
            rightSpiral.addPoint(240, 460);
            rightSpiral.addPoint(265, 340);
            rightSpiral.addPoint(320, 300);
            rightSpiral.addPoint(310, 150);
        }
        


        //Adds Enemies at a random speed, amount and path
        public void RandomAddEnemy(int max, int speed)
        {
            randomPath = rand.Next(0, 4);
            if (enemies.Count == max)
                return;
            do
            {
                enemyAmount = rand.Next(1, 9);
            } while (enemyAmount > 27 - enemies.Count || (enemies.Count + enemyAmount) > max);
            randomSpeed = rand.Next(3, speed + 1);

            if (randomPath == 0)
            {
                activePath = leftZig;
            }

            if (randomPath == 1)
            {
                activePath = rightZig;
            }

            if (randomPath == 2)
            {
                activePath = leftSpiral;
            }

            if (randomPath == 3)
            {
                activePath = rightSpiral;
            }
            x = 0;
            timer = 0;
            isAdding = true;
        }


        //Gets Filled Slots, Chooses Enemy
        public void RandomDive()
        {
            bool isPossible = false;
            int slotId;
            List<Enemy> e = new List<Enemy>();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].inFormation == true)
                {
                    isPossible = true;
                }
            }

            if (isPossible == true)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].inFormation == true)
                    {
                        e.Add(enemies[i]);
                    }
                }
                slotId = rand.Next(0, e.Count);

                e[slotId].Dive();

            }
        }



        //Returns Array Of Open Enemy Slots
        public Rectangle[] GetOpenSpots()
        {
            Rectangle[] rects = new Rectangle[27];
            for (int i = 0; i <= 26; i++)
            {
                if (spotIndex[i] == 0)
                {
                    rects[i] = spots[i];
                }
            }
            return rects;
        }

        //Returns Array Of Enemy Filled Spots
        public Rectangle[] GetClosedSpots()
        {
            Rectangle[] rects = new Rectangle[27];
            for (int i = 0; i <= 26; i++)
            {
                if (spotIndex[i] != 0)
                {
                    rects[i] = spots[i];
                }
            }
            return rects;
        }


        public void StartNewRound()
        {
            enemies.Clear();
        }


        //Adds New Enemy
        public void addEnemy(Path ap, int s)
        {
            tempenem = new Enemy(game);
            tempenem.EnemyEnter(s, ap);
            tempenem.em = this;
            enemies.Add(tempenem);
        }



        //Main Update Method
        public void update()
        {
            offset += 1 * direction;

            if (offset < -20)
            {
                direction = 1;
                current = enemy1;
            }
            if (offset > 20)
            {
                direction = -1;
                current = enemy2;
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].update();
            }

            if (isAdding == true)
            {

                    if (timer == 20)
                    {
                        addEnemy(activePath, randomSpeed);
                        timer = 0;
                        x++;
                    }

                timer++;
                if (x == enemyAmount)
                    isAdding = false;
            }

        }

        //Draw Method
        public void draw(SpriteBatch sb)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                sb.Draw(current, enemies[i].enemyPos, null, enemies[i].hit, enemies[i].rotate, new Vector2(15 / 2, 15 / 2), SpriteEffects.None, 0);
            }
        }


    }
}

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
    public class Player
    {
        //580 width 720 height
        //player 50x50

        private Rectangle player;
        public int lives;
        public int bullets;
        public int score;
        public int highScore;
        public int shots;
        public int hits;
        Game1 game;

        public Player(Game1 g)
        {
            player = new Rectangle(265, 595, 50, 50);
            lives = 3;
            bullets = 2;
            score = 0;

            if (!System.IO.File.Exists("highscore.txt"))
            {
                highScore = 0;
            }
            else
            {
                System.IO.StreamReader sr = new System.IO.StreamReader("highscore.txt");
                int score = int.Parse(sr.ReadLine());
                sr.Close();

                highScore = score;
            }

            game = g;
        }

        public void Hit()
        {
            lives--;
            if (lives == 0)
            {
                game.currentMenu = 2;

                if (!System.IO.File.Exists("highscore.txt"))
                {
                    System.IO.File.Create("highscore.txt").Close();
                }

                if (score >= highScore)
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter("highscore.txt");
                    sw.WriteLine(score + "");
                    sw.Close();
                }
            }
        }

        public Rectangle getRectangle()
        {
            return player;
        }

        public void newGame()
        {
            lives = 3;
            bullets = 2;
            score = 0;
            shots = 0;
            hits = 0;
        }
        
        public void addScore(int points)
        {
            score += points;
            if (score > highScore)
            {
                highScore = score;
            }
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

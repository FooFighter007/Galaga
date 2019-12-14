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
    class Path : Microsoft.Xna.Framework.Game
    {
        //Creates List Of Points
        public List<Vector2> pathPoints = new List<Vector2>(); 

        public Path()
        {

        }

        //Add to List Of Points
        public void addPoint(float x, float y)
        {
            pathPoints.Add(new Vector2(x,y));
        }       
    }
}

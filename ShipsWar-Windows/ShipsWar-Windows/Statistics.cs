///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ShipsWar.GameStates
{
    class Statistics
    {
        static int tick;
        public static List<int>[] planetDominion;
        public static int nplayers;
        public static int universeSize;
        public static int[] points;
        static float acum;
        static Universe universe;
        public static TimeSpan totalGameTime;

        public static void Reset(int _nplayers, Universe _universe)
        {
            nplayers = _nplayers;
            universe = _universe;
            universeSize = universe.planets.Length;
            tick = 0;            

            planetDominion = new List<int>[nplayers];
            points = new int[nplayers];

            for (int i = 0; i < nplayers; i++)
            {
                planetDominion[i] = new List<int>();
            }
            acum = 0;
            totalGameTime = new TimeSpan();
                    
        }


        public static void Update(GameTime gameTime, bool force)
        {
            totalGameTime += gameTime.ElapsedRealTime;
            int updateTime = 5000;
            acum += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (acum >= updateTime || force)
            {
                if (force)
                {
                    acum = 0;
                }
                else
                {
                    acum -= updateTime;
                }
                int []nPlanets = new int[nplayers];
                for(int i = 0; i < universe.planets.Length; i++)
                {
                    Planet p = universe.planets[i];
                    if (p.side != 0)
                    {
                        nPlanets[p.side - 1]++;
                    }
                }

                for (int i = 0; i < nplayers; i++)
                {
                    planetDominion[i].Add(nPlanets[i]);
                    points[i] += nPlanets[i];
                }

                tick++;
            }
        }
    }
}

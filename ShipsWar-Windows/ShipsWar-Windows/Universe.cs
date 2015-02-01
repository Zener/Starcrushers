///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace ShipsWar
{
    

    class Universe
    {
        public Random rnd;
        public Planet[] planets;
        private int planetCount;
        const float MIN_DISTANCE_BETWEEN_PLANETS = 1.4f;
        public List<Ship> ships = new List<Ship>();
        private int rows;
        private int columns;
        public List<Particle> particles = new List<Particle>();
        public TimeSpan universeTime;

        public Universe(int _planetCount)
        {
            planetCount = _planetCount;
            planets = new Planet[planetCount];

            if (GameVars.tutorialMode)
            {
                rnd = new Random(0);
            }
            else
            {
                rnd = new Random();
            }

            for (int i = 0; i < planets.Length; i++)
            {
                planets[i] = new Planet(this);
                planets[i].setRadius(1.5f + (float)rnd.NextDouble() * 3.0f);
                planets[i].textureID = 0 + (i%6);
                SetPlanetPosition(i);                
            }

            CalculatePlanetNeighbours();

            if (GameVars.tutorialMode)
            {
                planets[3].side = 2;
                AddShipsToPlanet(planets[3], 5);
            }
            //ships = new Ship[shipCount];

            /*for (int i = 0; i < shipCount; i++)
            {
                Ship s = new Ship(this);
                s.setPosition(new Vector3(5.0f + (float)rnd.NextDouble() * 90, 5.0f + (float)rnd.NextDouble() * 90, 0));
                s.oldPos = s.getPosition();
                ships.Add(s);
            }*/
        }


        public void SetPlanetPosition(int i)
        {
            /*if (i == 0)
            {
                planets[0].setPosition(new Vector3(10,30,0));
                return;
            }*/

            float distanceBetweenPlanets = MIN_DISTANCE_BETWEEN_PLANETS;
            float displayableWidth = 90;
            float displayableHeight = 55;

            float ratio = displayableWidth / displayableHeight;
            
            float rowsF = (float)Math.Sqrt(planets.Length / ratio);
            float columnsF = rowsF * ratio;

            rows = (int)rowsF;
            columns = (int)columnsF;

            if (rows*columns < planets.Length)
            {
                rows++;
                if (rows * columns < planets.Length)
                {
                    columns++;
                }
            }

            float gapWidth = displayableWidth / (float)columns;
            float gapHeight = displayableHeight / rows;
            float rndWidth =  (float)(rnd.NextDouble() - rnd.NextDouble()) * gapWidth / 2.4f;
            float rndHeight =  (float)(rnd.NextDouble() - rnd.NextDouble()) * gapHeight / 2.4f;

            float startX = 50.0f - (gapWidth * (columns-1) * 0.5f);
            //rndWidth = rndHeight = 0;

            float planetZ = (float)Math.Abs(rnd.NextDouble());
            planets[i].setPosition(new Vector3(startX/*- (distanceBetweenPlanets * 0.5f * (float)columns) */ + rndWidth + (/*distanceBetweenPlanets */ (i % columns)) * gapWidth, 20.0f + rndHeight + (distanceBetweenPlanets * (i / columns)) * gapHeight, -planetZ * GameVars.planetRangeZ));

            float minRadius = (float)(7.71f / Math.Sqrt(planets.Length));
            float radius = minRadius * 2;
            planets[i].setRadius(minRadius + ((1.0f - planetZ)*(radius / 1))/*(float)rnd.NextDouble() * radius*/);
            /*
            bool correct;
            int tries = 0;

            do
            {
                planets[i].setPosition(new Vector3(-10.0f + (float)rnd.NextDouble() * 120, 20.0f + (float)rnd.NextDouble() * 60, 0));

                correct = true;

                for (int j = 0; j < i; j++)
                {
                    if (planets[j].distance(planets[i]) < distanceBetweenPlanets * planets[i].getRadius() * planets[j].getRadius())
                    {
                        correct = false;
                        tries++;
                    }
                }

                if (tries > 2000)
                {
                    tries = 0;
                    distanceBetweenPlanets -= 0.1f;
                }
            }
            while (!correct);*/
        }




        public void CalculatePlanetNeighbours()
        {
            Vector3 cursor = new Vector3();
            int k = 0;
                
    
            for (int i = 0; i < planets.Length; i++)
            {
                Planet p = planets[i];

                /*
                //LEFT
                cursor.X = p.getPosition().X;
                cursor.Y = p.getPosition().Y;
                cursor.Z = p.getPosition().Z;
                k = 0;

                int planetCandidate = -1;
                Vector3 planetCandidadeCursor = new Vector3();

                while (planetCandidate == -1 && k < 1800)
                {
                    for (int j = 0; j < planets.Length; j++)
                    {
                        if (i == j || planets[j].planetRight != -1) continue;
                        
                        if (Vector3.Distance(cursor, planets[j].getPosition()) < Vector3.Distance(cursor, p.getPosition()))
                        {
                            //if (planetCandidate == -1 || Vector3.Distance(planetCandidadeCursor, p.getPosition()) > Vector3.Distance(cursor, p.getPosition()))
                            {
                                planetCandidate = j;
                                planetCandidadeCursor.X = cursor.X;
                                planetCandidadeCursor.Y = cursor.Y;
                                planetCandidadeCursor.Z = cursor.Z;
                            }
                            //p.planetLeft = j;
                            //planets[j].planetRight = i;
                        }
                    }
                    cursor.X -= 0.05f;
                    k++;
                }
                if (planetCandidate != -1)
                {
                    p.planetLeft = planetCandidate;
                    planets[planetCandidate].planetRight = i;

                }



                //TOP
                cursor.X = p.getPosition().X;
                cursor.Y = p.getPosition().Y;
                cursor.Z = p.getPosition().Z;
                k = 0;

                planetCandidate = -1;
                    
                while (planetCandidate == -1 && k < 800)
                {
                    for (int j = 0; j < planets.Length; j++)
                    {
                        if (i == j || planets[j].planetDown != -1) continue;
                        if (Vector3.Distance(cursor, planets[j].getPosition()) < Vector3.Distance(cursor, p.getPosition()))
                        {
                            planetCandidate = j;
                            //p.planetUp = j;
                            //planets[j].planetDown = i;
                        }
                    }
                    cursor.Y += 0.05f;
                    k++;
                }
                if (planetCandidate != -1)
                {
                    p.planetUp = planetCandidate;
                    planets[planetCandidate].planetDown = i;                 
                }
                */
                
                /*
                //while (k < 800)
                {
                    int planetCandidate = -1;
                    float planetCandidateFactor = 0.0f;
                    float angleMod = 0.52f;

                    for (int j = 0; j < planets.Length; j++)
                    {
                        if (i == j) continue;

                        if (planets[j].planetDown == -1 && planets[j].getPosition().Y > p.getPosition().Y)
                        {
                            if (planetCandidate == -1)
                            {
                                float offsetAngle = (float)(Math.PI / 2);
                                float facDistance = (100.0f - planets[j].distance(p)) / 100.0f;
                                float t1 = (float)(p.Angle(planets[j]));
                                t1 = offsetAngle - t1;
                                t1 = Math.Abs(t1);
                                t1 = ((offsetAngle) - t1) / (offsetAngle);
                                float facAngle = t1;//(float)( ((Math.PI / 2) - ()) / (Math.PI / 2));

                                float factor = 0.0f;
                                if (t1 > 0.25f)
                                {


                                    factor = facDistance + (facAngle * angleMod);
                                    planetCandidate = j;
                                    planetCandidateFactor = factor;
                                
                                }
                                
                                //planets[j].planetDown = i;
                            }
                            else if (planetCandidate != -1)
                            {
                                float offsetAngle = (float)(Math.PI / 2);
                                float facDistance = (100.0f - planets[j].distance(p)) / 100.0f;
                                float t1 = (float)( p.Angle(planets[j])  );
                                t1 = offsetAngle - t1;
                                t1 = Math.Abs(t1);
                                t1 = ((offsetAngle) - t1) / (offsetAngle);
                                float facAngle = t1;//(float)( ((Math.PI / 2) - ()) / (Math.PI / 2));

                                float factor = 0.0f;
                                //if (t1 > 0.25f)
                                {
                                    factor = facDistance + (facAngle*angleMod);
                                }
                                bool b = factor > planetCandidateFactor;
                                //bool b = Math.Abs(p.Angle(planets[j]) - offsetAngle) < Math.Abs(p.Angle(planets[planetCandidate]) - offsetAngle);




                                if (b)
                                

                                //if (Math.Abs(planets[j].getPosition().X - p.getPosition().X) < Math.Abs(planets[p.planetUp].getPosition().X - p.getPosition().X))
                                {
                                    planetCandidate = j;
                                    planetCandidateFactor = factor;
                                }
                            }
                        }
                    }
                    if (planetCandidate != -1)
                    {
                        p.planetUp = planetCandidate;
                        planets[planetCandidate].planetDown = i;
                    }
                    cursor.Y += 0.05f;
                    k++;
                }
                 */
                if (i % columns < columns-1)
                {
                    if (i + 1 < planets.Length)
                    {
                        planets[i].planetRight = i + 1;
                        planets[i + 1].planetLeft = i;
                    }
                }
                if (i / columns < rows-1)
                {
                    if (i + columns < planets.Length)
                    {
                        planets[i].planetUp = i + columns;
                        planets[i + columns].planetDown = i;
                    }
                }
            }
        }


        public void AddHomePlanet(Player _player)
        {
            int chosen;

            chosen = (int)(rnd.NextDouble() * planets.Length);

            while (planets[chosen].homePlanet)
            {
                chosen = (int)(rnd.NextDouble() * planets.Length);
            }

            _player.SetHomePlanet(planets[chosen], chosen);
            planets[chosen].SetHomePlanet(_player.side);
            
        }


        public void AddShipsToPlanet(Planet _planet, int _numberOfShips)
        {
            for (int i = 0; i < _numberOfShips; i++)
            {
                if (ships.Count >= GameVars.MAX_SHIPS_IN_UNIVERSE)
                {
                    break;
                }
                Ship s = new Ship(this);
                s.side = _planet.side;
                ships.Add(s);
                _planet.AddShip(s);
            }
        }

        public void DelShipsFromPlanet(Planet _planet, int _numberOfShips)
        {
            for (int i = 0; i < _numberOfShips; i++)
            {

                Ship s = _planet.GetShip();
                if (s != null)
                {
                    s.Destroy();
                }
                
            }
        }

        public void SendShip(Planet _origin, Planet _destination)
        {
            if (_origin != _destination)
            {
                Ship s = _origin.GetShip();
                if (s != null)
                {
                    s.SendTo(_destination);
                }
            }
        }


        public void Update(GameTime _gameTime)
        {
            universeTime = _gameTime.TotalGameTime;
            for (int i = 0; i < planets.Length; i++)
            {
                if (!GameStates.War.gameHalted) planets[i].Update(_gameTime);                
            }

            for (int i = 0; i < ships.Count; i++)
            {
                if (ships[i] != null)
                {
                    if (!GameStates.War.gameHalted) ships[i].Update(_gameTime);
                }
            }
        }


        public int GetRandomFreePlanet()
        {
            int tries = planets.Length * 5;
            int i = 0;
            bool freePlanetFound = false;
            int possiblePlanet = -1;

            while (i < tries)
            {
                possiblePlanet = rnd.Next(planets.Length);
                if (planets[possiblePlanet].side == 0)
                {
                    freePlanetFound = true;
                    //planetFocusedID = possiblePlanet;
                    break;
                }
                i++;
            }

            if (freePlanetFound)
            {
                return possiblePlanet;
            }
            else
            {
                return -1;
            }
        }


        public int GetWeakestPlanet(int _side)
        {
            int res = -1;
            int minShips = -1;

            for (int i = 0; i < planets.Length; i++)
            {
                if (planets[i].side == _side)
                {
                    if (planets[i].ships.Count < minShips || minShips == -1)
                    {
                        minShips = planets[i].ships.Count;
                        res = i;
                    }
                }
            }

            return res;
        }


        public int GetPlanetCount(int _side)
        {
            int res = 0;
            for (int i = 0; i < planets.Length; i++)
            {
                if (planets[i].side == _side)
                {
                    res++;
                }
            }
            return res;
        }


        public int GetShipCount(int _side)
        {
            int res = 0;
            for (int i = 0; i < planets.Length; i++)
            {
                if (planets[i].side == _side)
                {
                    res += planets[i].ships.Count;
                }
            }
            return res;
        }


        public void AddParticle(Particle p)
        {            
            particles.Add(p);
            
        }
        /*

        public void CalculatePlanetNeighbours()
        {
            float desfase = (float)Math.PI / 6;
            
            for (int i = 0; i < planets.Length; i++)
            {
                float radius;
                Planet p = planets[i];
                //LEFT
                radius = 0.5f;
                desfase = (float)Math.PI / 4;
                while (p.planetLeft == -1 && radius < 100.0f)
                {
                    for (int j = 0; j < planets.Length; j++)
                    {
                        if (i == j) continue;
                        float dx = planets[j].getPosition().X - p.getPosition().X;
                        float dy = planets[j].getPosition().Y - p.getPosition().Y;
                        //if (dy == 0) dy = 0.0001f;
                        float angle = (float)Math.Atan2(dy , dx);
                        while (angle < 0)
                        {
                            angle += (float)Math.PI * 2;
                        }
                        while (angle >= 2*Math.PI)
                        {
                            angle -= (float)Math.PI * 2;
                        }
                        if (angle > (Math.PI/2) + desfase && angle < ((3*Math.PI)/2) - desfase && p.distance(planets[j]) < radius)
                        {
                            p.planetLeft = j;
                            planets[j].planetRight = i;
                        }
                    }
                    desfase -= (float)Math.PI / 1800;
                    if (desfase < 0) desfase = 0;                    
                    radius += 0.25f;
                }
                //RIGHT
                radius = 0.5f;
                desfase = (float)Math.PI / 4;
                
                while (p.planetRight == -1 && radius < 100.0f)
                {
                    for (int j = 0; j < planets.Length; j++)
                    {
                        if (i == j) continue;
                        float dx = planets[j].getPosition().X - p.getPosition().X;
                        float dy = planets[j].getPosition().Y - p.getPosition().Y;
                        //if (dy == 0) dy = 0.0001f;
                        float angle = (float)Math.Atan2(dy , dx);
                        while (angle < 0)
                        {
                            angle += (float)Math.PI * 2;
                        }
                        while (angle >= 2 * Math.PI)
                        {
                            angle -= (float)Math.PI * 2;
                        }

                        if ((angle > ((3 * Math.PI) / 2)+desfase || angle < (Math.PI/2) - desfase) && p.distance(planets[j]) < radius)
                        {

                            p.planetRight = j;
                            planets[j].planetLeft = i;
                        }
                    }
                    desfase -= (float)Math.PI / 1800;
                    if (desfase < 0) desfase = 0;
                    radius += 0.25f;
                }


                //UP
                radius = 1.0f;
                while (p.planetUp == -1 && radius < 100.0f)
                {
                    for (int j = 0; j < planets.Length; j++)
                    {
                        if (i == j) continue;
                        if (p.getPosition().Y < planets[j].getPosition().Y && p.distance(planets[j]) < radius)
                        {
                            p.planetUp = j;
                            planets[j].planetDown = i;
                        }
                    }
                    radius += 1.0f;
                }
                //DOWN
                radius = 1.0f;
                while (p.planetDown == -1 && radius < 100.0f)
                {
                    for (int j = 0; j < planets.Length; j++)
                    {
                        if (i == j) continue;
                        if (planets[j].planetUp == -1 && p.getPosition().Y >= planets[j].getPosition().Y && p.distance(planets[j]) < radius)
                        {
                            p.planetDown = j;
                            planets[j].planetUp = i;
                        }
                    }
                    radius += 1.0f;
                }

            }
            

        }
        */

    }// end class
}

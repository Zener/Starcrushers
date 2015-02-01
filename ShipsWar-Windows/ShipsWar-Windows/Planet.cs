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
using Microsoft.Xna.Framework.Graphics;

namespace ShipsWar
{
    

    class Planet : Entity
    {
        public int textureID;
        private float radius;
        public int planetLeft, planetRight, planetUp, planetDown;
        public bool homePlanet;
        public int homePlanetSide;
        public int homePlanetShields;
        public List<Ship> ships = new List<Ship>();
        public double homePlanetGrow;
        public const int MAX_SHIPS_PER_PLANET = 100;
        public int type = 0;
        public float planetGrow;

        public const float GROW_MARK = 100.0f;

        public Planet(Universe _universe)
        {
            planetGrow = 0.0f;
            homePlanetGrow = 0.0f;
            homePlanet = false;
            universe = _universe;
            radius = 1.0f;
            planetLeft = planetRight = planetUp = planetDown = -1;
            side = 0;

            Random rnd = universe.rnd;

            //PLANET_TYPE_BUILD
            type = rnd.Next(3);
        }


        public void setRadius(float _radius)
        {
            radius = _radius;
        }


        public float getRadius()
        {
            return radius;
        }


        public void AddShip(Ship _s)
        {
            _s.setPosition(getPosition());
            _s.oldPos = getPosition();
            ships.Add(_s);
            _s.planetLanded = this;
        }


        public Ship GetShip()
        {
            if (ships.Count > 0)
            {
                Ship s = ships[0];
                ships.RemoveAt(0);
                return s;
            }

            return null;
        }


        public void Update(GameTime _gameTime)
        {
            if (homePlanet)
            {

                   if (side == homePlanetSide)
                   {

                        //Homeworld free of enemies
                       if (!(GameVars.tutorialMode && side == 2))
                       {
                           homePlanetGrow += getGrowRatio() * _gameTime.ElapsedGameTime.TotalMilliseconds;
                       }
                        while (homePlanetGrow >= GROW_MARK)
                        {
                            if (ships.Count < MAX_SHIPS_PER_PLANET)
                            {
                                universe.AddShipsToPlanet(this, 1);
                            }
                            homePlanetGrow -= GROW_MARK;                         
                        }
                    }
                    else
                    {
                        // Homeworld under enemy occupation
                        if (homePlanetShields > 0)
                        {
                            if (ships.Count > 50)
                            {
                                // Lots of enemy ships!!!
                                for (int i = 0; i < 50; i++)
                                {
                                    universe.DelShipsFromPlanet(this, 1);
                                }
                                //homePlanetShields--;
                                ShieldExplosion();
                                GameStates.War.ShieldDestroyed(this);
                                /*
                                if (Player.players[side - 1].playerType == Player.HUMAN_PLAYER)
                                {
                                    Input.Vibrate(Player.GetControllerBySide(side), 1.0f, 1.0f, 800);
                                }*/
                                //SoundManager.SoundPlay(SoundManager.MUSIC_STARBASEBLAST);
                            }
                            else
                            {
                                if ((GameVars.tutorialMode && homePlanetSide == 2))
                                {
                                    homePlanetGrow += 2.0f * _gameTime.ElapsedGameTime.TotalMilliseconds;
                                }
                                homePlanetGrow += 0.2f * _gameTime.ElapsedGameTime.TotalMilliseconds;
                                while (homePlanetGrow >= GROW_MARK)
                                {
                                    universe.DelShipsFromPlanet(this, 1);

                                    if (this.ships.Count <= 0)
                                    {
                                        //homePlanetShields--;
                                        side = homePlanetSide;
                                        universe.AddShipsToPlanet(this, 50);
                                        ShieldExplosion();
                                        GameStates.War.ShieldDestroyed(this);
                                
                                        /*
                                        if (Player.players[side - 1].playerType == Player.HUMAN_PLAYER)
                                        {
                                            Input.Vibrate(Player.GetControllerBySide(side), 1.0f, 1.0f, 800);
                                        }*/
                                        //SoundManager.SoundStop();
                                        //SoundManager.SoundPlay(SoundManager.MUSIC_STARBASEBLAST);
            
                                    }
                                    homePlanetGrow -= GROW_MARK;
                                }
                            }
                        }
                    }
                
            }
            else
            {
                if (this.side != 0)
                {
                    
                    if (!(GameVars.tutorialMode && side == 2))
                    {
                        planetGrow += 0.05f * (float)_gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    if ((GameVars.tutorialMode && side == 1))
                    {
                        planetGrow += 0.05f * (float)_gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    while (planetGrow >= GROW_MARK)
                    {
                        if (ships.Count < MAX_SHIPS_PER_PLANET)
                        {
                            universe.AddShipsToPlanet(this, 1);
                        }
                        planetGrow -= GROW_MARK;
                    }
                }
            }
        }


        public double getGrowRatio()
        {
            double glowRatio = 0.01;

            int planetsOwned = 0;

            for (int i = 0; i < universe.planets.Length; i++)
            {
                if (universe.planets[i].ships.Count > 0)
                {
                    
                    if (universe.planets[i].homePlanet && universe.planets[i].homePlanetSide == this.side)
                    {
                        planetsOwned++;
                        glowRatio += Math.Log10(universe.planets[i].ships.Count) * 0.03;
                    }


                    if ((!universe.planets[i].homePlanet) && (universe.planets[i].side == this.side))
                    {
                        planetsOwned++;
                        glowRatio += 0.02 * Math.Log10(universe.planets[i].ships.Count);
                    }
                }
            }



            return glowRatio;
        }

        public float Angle(Planet p)
        {
            float dx = p.getPosition().X - getPosition().X;
            float dy = p.getPosition().Y - getPosition().Y;
            float angle = (float)Math.Atan2(dy, dx);


            return angle;
        }

        public void SetHomePlanet(int _side)
        {
            position.Z = 0;
            homePlanetShields = 3;
            homePlanet = true;
            textureID = 4;
            homePlanetSide = _side;
            type = 3;
            setRadius(2.0f);
        }


        public void PlanetExplosion()
        {
            //ShieldExplosion();
            
            for (int i = 0; i < 32; i++)
            {
                float angle = (float)(Math.PI / 16.0f) * i;
                Particle p = new Particle(universe);
                p.SetParticle(Particle.PARTICLE_TYPE_EXPLOSION, 500, position, new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0)/50.0f, new Vector4(1.0f, 1.0f, 1.0f, 1.0f), "explosion", SpriteBlendMode.AlphaBlend);
                universe.particles.Add(p);
            }
        }


        public void ShieldExplosion()
        {
            for (int j = 2; j < 10; j++)
            {
                float angle2 = (float)(Math.PI / 4.0f) * j;// (universe.rnd.Next() % 8);
                for (int i = 0; i < 16; i++)
                {
                    float angle = (float)(Math.PI / 8.0f) * i;
                    Particle p = new Particle(universe);
                    p.SetParticle(Particle.PARTICLE_TYPE_EXPLOSION, 1500, position, new Vector3((float)(Math.Cos(angle) * Math.Cos(angle2)), (float)Math.Sin(angle), (float)(Math.Cos(angle) * Math.Sin(angle2))) / 17.0f, new Vector4(1.0f, 1.0f, 1.0f, 1.0f), "explosion", SpriteBlendMode.AlphaBlend);
                    universe.particles.Add(p);
                }
            }

            float[] c = Player.GetSideColor(homePlanetSide);
            
            for (int i = 0; i < 8; i++)
            {
                float angle = (float)(Math.PI / 4.0f) * i;
                Particle p = new Particle(universe);
                p.SetParticle(Particle.PARTICLE_TYPE_DEBRIS, 500, position, new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0) / 50.0f, new Vector4(c[0], c[1], c[2], 1.0f), "debris"+(universe.rnd.Next()%4), SpriteBlendMode.AlphaBlend);
                universe.particles.Add(p);
            }
        }
    }

}

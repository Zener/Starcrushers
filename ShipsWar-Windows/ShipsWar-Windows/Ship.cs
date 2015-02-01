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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;




namespace ShipsWar
{
    class Ship : Entity
    {
        public const int STATE_FLYING = 0;
        public const int STATE_LANDED = 1;

        public Planet   destination;
        Vector3         speed;
        float           speedMax = 0.005f;
        public Vector3  oldPos;
        Vector3         fThrust = new Vector3();

        public Planet   planetLanded;
        

        public Ship(Universe _universe)
        {
            universe = _universe;
            State = STATE_LANDED;
        }


        public void Update(GameTime _gameTime)
        {
            if (State == STATE_LANDED) return;
            if (destination == null) return;


            float oneOverMass = 0.001f;

            // Add forces
            Vector3 f = new Vector3();

            float l = fThrust.Length();

            //if (l == 0)
            {
                fThrust = destination.getPosition() - position;
                //fThrust.Y = destination.getPosition().Y - position.Y;
                //fThrust.Z = 0.0f;
                l = fThrust.Length();
                //if (l > 5) l = 5;
                if (l == 0)
                {
                    fThrust = Vector3.Zero;
                }
                else
                {
                    fThrust.Normalize();
                    //fThrust *= l;
                    fThrust *= 0.014f;
                }
                //fThrust *= 0.0001f;
            }


            // calculate possible collisions with other ships
            /*if (distance(destination) > 5.0f)
            {
                for (int i = 0; i < universe.ships.Count; i++)
                {
                    if (universe.ships[i] != this && universe.ships[i].State == STATE_FLYING)
                    {
                        float dist = distance(universe.ships[i]);
                        if (dist < 1.75f)
                        {
                            Vector3 rectification = position - universe.ships[i].getPosition();
                            float len = rectification.Length();
                            rectification.Normalize();
                            rectification *= 1.75f - len;
                            f += rectification * 0.00175f;
                            break;
                        }
                    }
                }
            }
            else
            {
                //oldPos = oldPos - ((getPosition()-oldPos)/2);
            }*/
            float max = 1.0f;
            /*if (fThrust.Length() > max)
            {
                fThrust.Normalize();
                fThrust *= max;
            }*/
                /*
                Vector3 fDrag = new Vector3();
                fDrag = fThrust;

                if (fDrag.Length() > 0.0f)
                {
                    fDrag.Normalize();

                    fDrag *= fThrust.LengthSquared();
                    fDrag *= 0.9f;
                    fDrag = -fDrag;
                }
                */
                //f += fDrag;

            f += fThrust;

            Vector3 fDrag = new Vector3();
            fDrag += speed;
            fDrag *= -70.00f;

            f += fDrag;

            if (f.Length() > max)
            {
                f.Normalize();
                f *= max;
            }


            // Integration
            Vector3 t = getPosition();
            Vector3 a = f * oneOverMass;
            Vector3 p = getPosition();
            float elapsedMilis = (float)_gameTime.ElapsedGameTime.TotalMilliseconds;
            p += position - oldPos + (a * elapsedMilis * elapsedMilis);
            
            setPosition(p);
            speed = a * elapsedMilis;
            /*
            if (destination.getPosition().X > getPosition().X) speed.X = speedMax;
            if (destination.getPosition().X < getPosition().X) speed.X = -speedMax;

            if (destination.getPosition().Y > getPosition().Y) speed.Y = speedMax;
            if (destination.getPosition().Y < getPosition().Y) speed.Y = -speedMax;

            position += (speed * _gameTime.ElapsedGameTime.Milliseconds);
            */

            //yaw calculation
            CalculateYaw(oldPos, p);

            /*
            yaw = yaw + (float)Math.PI / 2;
            if (yaw < 0)
            {
                yaw += (float)Math.PI * 2;
            }
            if (yaw >= Math.PI * 2)
            {
                yaw -= (float)Math.PI * 2;
            }*/

            oldPos = t;

            if (distance(destination) < destination.getRadius())
            {
                EnterPlanet(destination);
            }
        }


        public void EnterPlanet(Planet _target)
        {
            if (_target.side == side)
            {
                Land(_target);
            }
            else
            {
                if (_target.ships.Count == 0)
                {
                    
                    SoundManager.SoundPlay(SoundManager.MUSIC_PLANETBLAST);
                    _target.PlanetExplosion();
                    if (_target.side != 0)
                    {
                        float ratio = _target.getPosition().X / 100.0f;
                        if (Player.players[_target.side - 1].playerType == Player.HUMAN_PLAYER)
                        {
                            Input.Vibrate(Player.GetControllerBySide(_target.side), 1.0f - ratio, ratio, 300);
                        }
                    }
                    _target.side = side;
                    Land(_target);
                }
                else
                {
                    if (_target.side != 0)
                    {
                        float ratio = _target.getPosition().X / 100.0f;
                        if (Player.players[_target.side - 1].playerType == Player.HUMAN_PLAYER)
                        {
                            Input.Vibrate(Player.GetControllerBySide(_target.side), (1.0f - ratio)/2.0f, ratio / 2.0f, 100);
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        Particle p = new Particle(universe);
                        float[] c1 = Player.GetSideColor(side);
                        p.SetParticle(Particle.PARTICLE_TYPE_NONE, 800, position + new Vector3((float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble()) / 10.0f,
                            new Vector3((float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble()) / 100.0f
                            - (new Vector3((float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble()) / 100.0f),
                            new Vector4(c1[0]+0.5f, c1[1]+0.5f, c1[2]+0.5f, 0.4f), "smallexplosion", SpriteBlendMode.AlphaBlend);
                        universe.AddParticle(p);
                    }


                    for (int i = 0; i < 5; i++)
                    {
                        Particle p = new Particle(universe);
                        float[] c1 = Player.GetSideColor(side);
                        p.SetParticle(Particle.PARTICLE_TYPE_NONE, 800, position + new Vector3((float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble()) / 10.0f, 
                            new Vector3((float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble(), (float)universe.rnd.NextDouble()) / 100.0f
                            - (speed * (float)universe.rnd.NextDouble()/2.0f),
                            new Vector4(c1[0], c1[1], c1[2], 0.4f), "smallexplosion", SpriteBlendMode.AlphaBlend);
                        universe.AddParticle(p);
                    }

                    Particle pb = new Particle(universe);
                    pb.SetParticle(Particle.PARTICLE_TYPE_EXPLOSION, 500, position, new Vector3(), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), "explosion", SpriteBlendMode.AlphaBlend);
                    universe.AddParticle(pb);

                    // Remove one enemy ship
                    Ship s = _target.ships[0];
                    universe.ships.Remove(s);
                    _target.ships.RemoveAt(0);
                    // Remove this ship
                    universe.ships.Remove(this);                    
                }
            }
        }


        public void Destroy()
        {
            universe.ships.Remove(this);
            if (State == STATE_LANDED)
            {
                if (this.planetLanded != null)
                {
                    planetLanded.ships.Remove(this);
                }
            }
        }


        public void Land(Planet _target)
        {
            position = destination.getPosition();
            fThrust = Vector3.Zero;
            State = STATE_LANDED;
            planetLanded = _target;
            _target.AddShip(this);
        }


        public void SendTo(Planet _target)
        {
            SoundManager.SoundPlay(SoundManager.MUSIC_TAKEOFF);
            
            //planetLanded
            float randAngle = (float)(universe.rnd.NextDouble()*Math.PI*2);
            float randZ = (float)(universe.rnd.NextDouble() - universe.rnd.NextDouble()) * 0.5f;
            //float randY = (float)(universe.rnd.NextDouble() - universe.rnd.NextDouble()) * 0.5f;

            position.X += 0.5f * (float)Math.Cos(randAngle);
            position.Y += 0.5f * (float)Math.Sin(randAngle);
            position.Z += randZ;

            Vector3 step = position - planetLanded.getPosition();
            int maxtries = 5;
            int tries = 0;

            while (ShipCollision() && tries < maxtries)
            {
                position += step;
                //position.X += (float)(universe.rnd.NextDouble() - universe.rnd.NextDouble()) * 1.0f;
                //position.Y += (float)(universe.rnd.NextDouble() - universe.rnd.NextDouble()) * 1.0f;            
                tries++;
            }
            oldPos = position;
            speed = Vector3.Zero;
            
            destination = _target;
            State = STATE_FLYING;
        }

        public bool ShipCollision()
        {
            for (int i = 0; i < universe.ships.Count; i++)
            {
                if (universe.ships[i].side == this.side && universe.ships[i].State == STATE_FLYING)
                {
                    float dist = distance(universe.ships[i]);
                    if (dist < 1.75f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        ArrayList verticeslist = new ArrayList();
        VertexPositionNormalTexture[] verticesarray;

        public void DrawShip()
        {
            CreateShipVertexBuffer();

            GraphicsDevice device = Game1.main.graphics.GraphicsDevice;
            device.DrawUserPrimitives(PrimitiveType.TriangleList, verticesarray, 0, verticesarray.Length / 3);
            
        }


        protected void CreateShipVertexBuffer()
        {
            if (verticeslist.Count > 0) return;

            Vector3 v1, v2, v3;
            verticeslist.Clear();

            v1 = new Vector3(0.0f, 3.0f, 0.0f);
            v2 = new Vector3(2.0f, -3.0f, 0.0f);
            v3 = new Vector3(-2.0f, -3.0f, 0.0f);

            verticeslist.Add(new VertexPositionNormalTexture(v1, new Vector3(0, 0.7f, 0.7f), new Vector2()));
            verticeslist.Add(new VertexPositionNormalTexture(v2, new Vector3(0.7f, -0.7f, 0.4f), new Vector2()));
            verticeslist.Add(new VertexPositionNormalTexture(v3, new Vector3(-0.7f, -0.7f, 0.4f), new Vector2()));
            
            verticesarray = (VertexPositionNormalTexture[])verticeslist.ToArray(typeof(VertexPositionNormalTexture));
        }


        protected void CreateShipVertexBuffer1()
        {
            //if (verticeslist.Count > 0) return;

            Vector3 v1, v2, v3, v4, v5;
            
            verticeslist.Clear();

            v1 = new Vector3(0.0f, 3.0f, 0.0f);
            
            v2 = new Vector3(2.0f, -3.0f, 0.0f);
            v3 = new Vector3(-2.0f, -3.0f, 0.0f);

            v4 = new Vector3(0.0f, -4.0f, 1.0f);
            v5 = new Vector3(0.0f, -4.0f, -1.0f);

            Vector3[] points = { v1, v2, v3, v4, v5 };
            int[] faces = { 0, 3, 2, 
                                0, 1, 3, 
                                /*0, 4, 2, 
                                0, 1, 4*/ };

            //Vector3[] normals = { new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f) };
            Vector3[] normals = { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(), new Vector3(), new Vector3(), new Vector3() };
            
            for (int i = 0; i < faces.Length / 3; i++)
            {
                Vector3 firstvec = points[faces[(i * 3) + 1]] - points[faces[i * 3]];
                Vector3 secondvec = points[faces[i * 3]] - points[faces[(i * 3) + 2]];
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();
                normals[faces[i * 3]] += normal;
                normals[faces[(i * 3) + 1]] += normal;
                normals[faces[(i * 3) + 2]] += normal;
            }
            
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i].Normalize();
                /*float temp = normals[i].Y;
                normals[i].Y = normals[i].Z;
                normals[i].Z = temp;*/
            }
            
            for (int i = 0; i < faces.Length; i++)
            {
                verticeslist.Add(new VertexPositionNormalTexture(points[faces[i]], normals[faces[i]], new Vector2()));
            }

            verticesarray = (VertexPositionNormalTexture[])verticeslist.ToArray(typeof(VertexPositionNormalTexture));
            
        }



    }
}

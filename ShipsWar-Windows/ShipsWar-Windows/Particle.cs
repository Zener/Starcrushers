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
    class Particle : Entity
    {
        Vector3             speed;
        float               speedMax = 0.005f;
        public Vector3      oldPos;
        Vector3             fThrust = new Vector3();
        Vector3             destination;
        string              shape;
        SpriteBlendMode     drawMode;
        Color drawColor;
        public TimeSpan     startTime;
        public TimeSpan     timeToLive;
        public int          type;
        public const int    PARTICLE_TYPE_NONE = 0;
        public const int    PARTICLE_TYPE_EXPLOSION = 1;
        public const int    PARTICLE_TYPE_HOOP = 2;
        public const int    PARTICLE_TYPE_DEBRIS = 3;


        public Particle(Universe _universe)
        {
            universe = _universe;
            startTime = new TimeSpan();
        }


        public void SetParticle(int _type, int _timeToLive, Vector3 _position, Vector3 _speed, Vector4 _color, string _shape, SpriteBlendMode _mode)
        {
            type = _type;
            speed = _speed;
            position = _position;
            shape = _shape;
            drawMode = _mode;
            timeToLive = new TimeSpan(0, 0, 0, 0, _timeToLive);
            drawColor = new Color(_color);
        }


        public void Draw(Matrix view, Matrix projection)
        {
            GraphicsDevice device = Game1.main.graphics.GraphicsDevice;
            int screenHeight = device.PresentationParameters.BackBufferHeight;
            Vector2 pos = Util.TransformTo2D(position, view, projection);

            if (view == Matrix.Identity && projection == Matrix.Identity)
            {
                pos.X = position.X;
                pos.Y = position.Y;
            }
            Color renderColor;
            switch (type)
            {
                case PARTICLE_TYPE_HOOP:
                    {
                        float r = (float)(startTime.Milliseconds/2);
                        int ri = (int)r % 256;
                        renderColor = new Color(drawColor.R, drawColor.G, drawColor.B, (byte)(ri));
                        break;
                    }
                case PARTICLE_TYPE_EXPLOSION:
                    {
                        float r = (float)(startTime.TotalMilliseconds * 255.0f) / (float)timeToLive.TotalMilliseconds;
                        renderColor = new Color(drawColor.R, drawColor.G, drawColor.B, (byte)(255 - r));
                        break;
                    }
                case PARTICLE_TYPE_DEBRIS:
                    {
                        float r = (float)(startTime.TotalMilliseconds * 255) / (float)timeToLive.TotalMilliseconds;
                        renderColor = new Color(drawColor.R, drawColor.G, drawColor.B, (byte)(255 - r));
                        break;
                    }
                default:
                    renderColor = drawColor;
                    break;
            }

            switch (type)
            {
                case PARTICLE_TYPE_HOOP:
                    Util.DrawSprite(device, shape, pos, new Vector2(screenHeight / 7, screenHeight / 7)*Game1.gameScale, drawMode, renderColor, Util.SPRITE_ALIGN_CENTERED);                    
                    break;
                case PARTICLE_TYPE_DEBRIS:
                    Util.DrawSprite(device, shape, pos, new Vector2(screenHeight / 32.5f, screenHeight / 32.5f) * Game1.gameScale, drawMode, renderColor, Util.SPRITE_ALIGN_CENTERED, startTime.Milliseconds / 70.0f);
                    break;
                default:

                    Util.DrawSprite(device, shape, pos, new Vector2(Util.GetImageWidth(shape), Util.GetImageHeight(shape)) * Game1.gameScale, drawMode, renderColor, Util.SPRITE_ALIGN_CENTERED);
                    break;
            }   
        }


        public void Update(GameTime _gameTime)
        {
            startTime += _gameTime.ElapsedGameTime;

            position += speed * _gameTime.ElapsedGameTime.Milliseconds;

            if (startTime > timeToLive)
            {
                Destroy();
            }
        }
        /*
        public void Update(GameTime _gameTime)
        {
            

            float oneOverMass = 0.001f;

            // Add forces
            Vector3 f = new Vector3();

            float l = fThrust.Length();

            //if (l == 0)
            {
                fThrust = destination - position;
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
           
            float max = 1.0f;
           
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
            p += position - oldPos + (a * _gameTime.ElapsedGameTime.Milliseconds * _gameTime.ElapsedGameTime.Milliseconds);
            
            setPosition(p);
            speed = a * _gameTime.ElapsedGameTime.Milliseconds;
           
            //yaw calculation
            CalculateYaw(oldPos, p);

            
            oldPos = t;

            
        }
        */


        public void Destroy()
        {
            //universe.ships.Remove(this);
            if (universe != null)
            {
                int i = universe.particles.Count;
                universe.particles.Remove(this);
            }
        }


        /*
        public void SendTo(Planet _target)
        {
            //planetLanded
            float randX = (float)(universe.rnd.NextDouble() - universe.rnd.NextDouble()) * 0.5f;
            float randY = (float)(universe.rnd.NextDouble() - universe.rnd.NextDouble()) * 0.5f;

            position.X += randX;
            position.Y += randY;
            position.Z += randX;

            Vector3 step = position - planetLanded.getPosition();
            int maxtries = 15;
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
        }*/

        /*
        ArrayList verticeslist = new ArrayList();
        VertexPositionNormalTexture[] verticesarray;

        public void DrawShip()
        {

            GraphicsDevice device = Game1.main.graphics.GraphicsDevice;
            CreateShipVertexBuffer();

            device.VertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
            device.DrawUserPrimitives(PrimitiveType.TriangleList, verticesarray, 0, 1);

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
        */


    }
}

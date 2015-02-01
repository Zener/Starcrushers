using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using ShipsWar.Menus;


namespace ShipsWar
{
    class MenuBackgroundRender
    {
        List<Particle> particles;
        TimeSpan timeToChangeFocus = new TimeSpan();
        TimeSpan timeToParticle = new TimeSpan();
        TimeSpan timeToNextRafaga = new TimeSpan();
        int focusX, focusY;
        Random rnd = new Random();
        GraphicsDevice gdev;


        public MenuBackgroundRender()
        {
            particles = new List<Particle>();            
        }


        public void Update(GraphicsDevice _gdev, GameTime gameTime)
        {
            gdev = _gdev;
            timeToNextRafaga -= gameTime.ElapsedRealTime;
            timeToChangeFocus -= gameTime.ElapsedGameTime;
            timeToParticle -= gameTime.ElapsedGameTime;

            if (timeToNextRafaga.TotalMilliseconds < -1000)
            {
                timeToNextRafaga = new TimeSpan(0, 0, 0, 0, 250 + (rnd.Next() % 3000));
            }

            if (timeToNextRafaga.TotalMilliseconds < 0)
            {
                if (timeToChangeFocus.TotalMilliseconds < 0)
                {
                    timeToChangeFocus = new TimeSpan(0, 0, 0, 0, 100 + (rnd.Next() % 1000));
                    focusX = rnd.Next() % gdev.PresentationParameters.BackBufferWidth;
                    focusY = (gdev.PresentationParameters.BackBufferHeight / 5) + (rnd.Next() % (int)(gdev.PresentationParameters.BackBufferHeight - ((gdev.PresentationParameters.BackBufferHeight * 2.0f) / 5.0f)));
                }

                if (timeToParticle.TotalMilliseconds < 0)
                {
                    if (rnd.Next() - rnd.Next() > 0)
                    {
                        for (int i = 0; i < (rnd.Next() % 3) + 1; i++)
                        {
                            int pX = focusX + rnd.Next() % gdev.PresentationParameters.BackBufferWidth / 15;
                            int pY = focusY + rnd.Next() % gdev.PresentationParameters.BackBufferHeight / 9;
                            Particle p = new Particle(null);
                            float c = (float)rnd.NextDouble() / 2.0f;
                            p.SetParticle(Particle.PARTICLE_TYPE_EXPLOSION, 500, new Vector3(pX, pY, 0),
                                        new Vector3(), new Vector4(c, c, 0.0f, (float)rnd.NextDouble()), "explosion", SpriteBlendMode.Additive);


                            particles.Add(p);
                        }
                    }
                    timeToParticle = new TimeSpan(0, 0, 0, 0, 50 + (rnd.Next(100)));
                }
            }

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update(gameTime);
                if (particles[i].startTime > particles[i].timeToLive)
                {
                    particles.Remove(particles[i]);
                }
            }

        }


        public void Render()
        {

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw(Matrix.Identity, Matrix.Identity);
            }
        }


    }
}

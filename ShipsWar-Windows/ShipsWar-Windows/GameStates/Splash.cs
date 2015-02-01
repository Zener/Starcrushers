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
    class Splash: GameState
    {
        Texture2D bg;
        TimeSpan startGameTimer;


        public override bool IsFullScreen() { return true; }

        public Splash(Game1 g)
            : base(g)
        {
            //bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\splash");

            bg = new Texture2D(m_Game.graphics.GraphicsDevice, 10, 10, 1, TextureUsage.None, SurfaceFormat.Color);
            
               
            startGameTimer = new TimeSpan(0,0,1);
        }

        /// <summary>
        /// Allows the game state to run logic.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The next gamestate to transition to, this to stay, null to pop back to previous state. </returns>
        public override GameState Update(GameTime gameTime)
        {
            //if (Input.WasPressed(-1, Input.BACK))
            //    return null;
            //if (Input.WasPressed(-1, Input.HIT | Input.START))
            startGameTimer -= gameTime.ElapsedGameTime;
            if (startGameTimer.Ticks < 0)
            {
                return new GameStates.MainMenu(m_Game);
            }
            return this;
        }

        /// <summary>
        /// This is called when the game state should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice gdev = m_Game.graphics.GraphicsDevice;
            //Texture2D bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\splash");
            m_Game.batch.Begin(SpriteBlendMode.None);
            gdev.RenderState.DepthBufferWriteEnable = false;
            
            m_Game.batch.Draw(bg,
                                    new Rectangle(0, 0, gdev.PresentationParameters.BackBufferWidth, gdev.PresentationParameters.BackBufferHeight),Color.Black);
            m_Game.batch.End();
            
        }
    }
}

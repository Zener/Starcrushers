///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ShipsWar
{
    class GameState
    {
        protected Game1 m_Game;

        protected bool m_Finished = true; // true if we should be removed when requesting a state change

        public bool Finished
        {
            get
            {
                return m_Finished;
            }
        }

        public GameState(Game1 g)
        {
            m_Game = g;
        }

        public virtual bool IsFullScreen() { return false; }

        /// <summary>
        /// Allows the game state to run logic.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The next gamestate to transition to, this to stay, null to pop back to previous state. </returns>
        public virtual GameState Update(GameTime gameTime) { return null; }

        /// <summary>
        /// This is called when the game state should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Draw(GameTime gameTime) { }

        public virtual void Dispose() { }
    }
}

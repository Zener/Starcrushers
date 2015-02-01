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
using ShipsWar.Menus;


namespace ShipsWar.GameStates
{
    class SelectPlayers : GameState
    {
        Texture2D bg;
        Menu menu;
        int universeSize;
        MenuBackgroundRender mbr;

        public override bool IsFullScreen() { return true; }

        public SelectPlayers(Game1 g, int _universeSize)
            : base(g)
        {
            universeSize = _universeSize;
            //bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\MainMenu");
            menu = new Menu();
            menu.Init(0, 0, "Number of players", "Select", "Back");
            menu.AddOption(0, "Two Players", Menu.MENU_ACTION_NEXT);
            menu.AddOption(0, "Three Players", Menu.MENU_ACTION_NEXT);
            menu.AddOption(0, "Four Players", Menu.MENU_ACTION_NEXT);
            mbr = new MenuBackgroundRender();
        }

        /// <summary>
        /// Allows the game state to run logic.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The next gamestate to transition to, this to stay, null to pop back to previous state. </returns>
        public override GameState Update(GameTime gameTime)
        {
            mbr.Update(m_Game.graphics.GraphicsDevice, gameTime);

            return DoAction(menu.Update(gameTime));
        }


        public GameState DoAction(int action)
        {
            switch (action)
            {
                case 0:
                    if (menu.menuExited)
                    {
                        return new GameStates.SelectUniverseSize(m_Game);                    
                    }
                    menu.exitSequence = true;
                    break;

                case 1:
                    if (menu.menuExited)
                    {
                        return new GameStates.ConfigurePlayers(m_Game, menu.optionSelected + 2, universeSize);
                    }
                    menu.exitSequence = true;
                    
                    break;
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
            
            m_Game.DrawBackgroundImage();

            mbr.Render();
            // Logo
            int logoWidth = Util.GetImageWidth("Menus\\logo");
            int logoHeight = Util.GetImageHeight("Menus\\logo");

            float logoW = logoWidth * menu.scale * GameVars.destLogoScale;
            float logoH = logoHeight * menu.scale * GameVars.destLogoScale;

            Util.DrawSprite(gdev, "Menus\\logo", new Vector2(gdev.PresentationParameters.BackBufferWidth / 2, gdev.PresentationParameters.BackBufferHeight / 6), new Vector2(logoW, logoH), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_CENTERED);
               
            menu.Draw();
        }
    }
}

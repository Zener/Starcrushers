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
    class SelectUniverseSize: GameState
    {
        Texture2D bg;
        Menu menu;
        MenuBackgroundRender mbr;

        public override bool IsFullScreen() { return true; }

        public SelectUniverseSize(Game1 g)
            : base(g)
        {
            //bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\MainMenu");
            menu = new Menu();
            menu.Init(0, 0, "Universe Size Menu", "Select", "Back");
            menu.AddOption(0, "Tiny", Menu.MENU_ACTION_NEXT);
            menu.AddOption(0, "Small", Menu.MENU_ACTION_NEXT);
            menu.AddOption(0, "Medium", Menu.MENU_ACTION_NEXT);
            menu.AddOption(0, "Large", Menu.MENU_ACTION_NEXT);
            menu.AddOption(0, "Huge", Menu.MENU_ACTION_NEXT);
            mbr = new MenuBackgroundRender();
        }

        /// <summary>
        /// Allows the game state to run logic.
        /// </summary>backspace
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
                        return new GameStates.MainMenu(m_Game);                    
                    }
                    menu.exitSequence = true;
                    break;

                case 1:
                    if (menu.menuExited)
                    {
                        return new GameStates.SelectPlayers(m_Game, menu.optionSelected);
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

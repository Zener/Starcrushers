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
    class ConfigurePlayers : GameState
    {
        Texture2D bg;
        ConfigScreen menu;
        int universeSize;
        int numberOfPlayers;
        MenuBackgroundRender mbr;

        public override bool IsFullScreen() { return true; }

        public ConfigurePlayers(Game1 g, int _numberOfPlayers, int _universeSize)
            : base(g)
        {
            mbr = new MenuBackgroundRender();
            numberOfPlayers = _numberOfPlayers;
            universeSize = _universeSize;
            //bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\MainMenu");
            menu = new ConfigScreen();
            menu.Init(0, 0, "Configure Players", "Continue", "Back");
            menu.AddColumn(0, "Human", Menu.MENU_ACTION_NEXT);
            menu.AddColumn(0, "Easy CPU", Menu.MENU_ACTION_NEXT);
            menu.AddColumn(0, "Medium CPU", Menu.MENU_ACTION_NEXT);
            menu.AddColumn(0, "Hard CPU", Menu.MENU_ACTION_NEXT);

            for (int i = 0; i < _numberOfPlayers; i++)
            {
                menu.AddRow(0, "Player " + (i + 1), Menu.MENU_ACTION_NEXT);
            }
            
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
                        return new GameStates.SelectPlayers(m_Game, universeSize);                    
                    }
                    menu.exitSequence = true;
                    break;

                case 1:
                    if (menu.menuExited)
                    {
                        return new GameStates.War(m_Game, numberOfPlayers, menu.rowSelected, universeSize);
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
        
            Util.DrawSprite(gdev, "Menus\\logo", new Vector2(gdev.PresentationParameters.BackBufferWidth / 2, gdev.PresentationParameters.BackBufferHeight/6), new Vector2(logoW, logoH), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_CENTERED);
            
            menu.Draw();
        }
    }
}

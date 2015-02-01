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
    class Credits : GameState
    {
        Texture2D bg;
        TextScreen textScreen;
        int universeSize;
        MenuBackgroundRender mbr;

        public override bool IsFullScreen() { return true; }

        public Credits(Game1 g)
            : base(g)
        {
            int maxSize = m_Game.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2;
            maxSize = (int)((float)maxSize / Game1.menuScale);
            //bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\MainMenu");
            textScreen = new TextScreen();
            textScreen.Init(0, 0, "Credits", null, "Back");

            textScreen.AddText(0, "  ", maxSize);

            textScreen.AddText(0, "  ", maxSize);

            textScreen.AddText(0, "Starcrushers V1.1a", maxSize);

            textScreen.AddText(0, "  ", maxSize);

            textScreen.AddText(0, "Credits", maxSize);

            textScreen.AddText(0, "  ", maxSize);

            textScreen.AddText(0, "Design & Programming", maxSize);

            textScreen.AddText(1, "Carlos Peris", maxSize);

            textScreen.AddText(0, "  ", maxSize);

            textScreen.AddText(0, "Graphics", maxSize);

            textScreen.AddText(1, "Nadia Morales", maxSize);
            
            textScreen.AddText(0, "  ", maxSize);

            textScreen.AddText(0, "Music & SFX", maxSize);

            textScreen.AddText(1, "Jordi Pares", maxSize);

            textScreen.AddText(0, "  ", maxSize);

            textScreen.AddText(0, "Thanks to", maxSize);

            textScreen.AddText(1, "Judit Verdaguer", maxSize);
            textScreen.AddText(1, "Carolina Lopez", maxSize);
            textScreen.AddText(1, "Lorenzo Ibarria", maxSize);


            textScreen.AddText(0, "  ", maxSize);
            textScreen.AddText(0, "  ", maxSize);
            textScreen.AddText(0, "Developed for Microsoft XNA Game Studio Contest 2007", maxSize);
            //textScreen.AddText(0, "2007", maxSize);
            textScreen.AddText(0, "  ", maxSize);
            textScreen.AddText(0, "Visit http:// kung-foo.dhs.org/ zener/ starcrushers", maxSize);

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

            return DoAction(textScreen.Update(gameTime));
        }


        public GameState DoAction(int action)
        {
            switch (action)
            {
                case 0:
                    if (textScreen.menuExited)
                    {
                        return new GameStates.MainMenu(m_Game);                    
                    }
                    textScreen.exitSequence = true;
                    break;
                /*
                case 1:
                    if (menu.menuExited)
                    {
                        return new GameStates.War(m_Game, menu.optionSelected + 2, universeSize);
                    }
                    menu.exitSequence = true;
                    
                    break;*/
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

            float logoW = logoWidth * Game1.menuScale * GameVars.destLogoScale;
            float logoH = logoHeight * Game1.menuScale * GameVars.destLogoScale;

            Util.DrawSprite(gdev, "Menus\\logo", new Vector2(gdev.PresentationParameters.BackBufferWidth / 2, gdev.PresentationParameters.BackBufferHeight / 6), new Vector2(logoW, logoH), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_CENTERED);
            
            textScreen.Draw();
        }
    }
}

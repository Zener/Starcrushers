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
    class PauseMenu: GameState
    {
        private string m_File;
        public override bool IsFullScreen() { return false; }
        Menu menu;
        public static bool endGame;

        public PauseMenu(Game1 g, string file)
            : base(g)
        {
            endGame = false;
            m_File = file;
            //SoundManager.SoundStop();

            //SoundManager.SoundPlay(SoundManager.MUSIC_VICTORY);
            //SoundManager.SoundPlay(SoundManager.MUSIC_DEFEAT);
            menu = new Menu();
            menu.Init(0, 0, "Pause Menu", "Select", "Back");
            menu.AddOption(0, "Continue", Menu.MENU_ACTION_BACK);
            menu.AddOption(0, "Exit Game", Menu.MENU_ACTION_NEXT);            
        }

        /// <summary>
        /// Allows the game state to run logic.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The next gamestate to transition to, this to stay, null to pop back to previous state. </returns>
        public override GameState Update(GameTime gameTime)
        {
            /*
            if (Input.WasPressed(-1, Input.BACK | Input.HIT | Input.START))
                return null;
            return this;*/
            return DoAction(menu.Update(gameTime));

        }

                


        public GameState DoAction(int action)
        {
            switch (action)
            {
                case Menu.MENU_ACTION_NEXT:
                    endGame = true;
                    if (menu.menuExited)
                    {
                        return null;// new GameStates.MainMenu(m_Game);                    
                    }
                    menu.exitSequence = true;
                    
                    break;

                case Menu.MENU_ACTION_BACK:
                    
                    if (menu.menuExited)
                    {
                        return null;// new GameStates.SelectPlayers(m_Game, menu.optionSelected);
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
            
            GraphicsDevice device = m_Game.graphics.GraphicsDevice;
            int screenWidth = device.PresentationParameters.BackBufferWidth;
            int screenHeight = device.PresentationParameters.BackBufferHeight;
              
            //Util.DrawSprite(gdev, "Menus/labelboxalpha", new Vector2(), new Vector2(gdev.PresentationParameters.BackBufferWidth, gdev.PresentationParameters.BackBufferHeight), SpriteBlendMode.AlphaBlend, new Color(128, 128, 128), Util.SPRITE_ALIGN_TOP_LEFT);
            Util.DrawSprite(device, "Menus/labelboxwhite", new Vector2(screenWidth, screenHeight) * 0.025f, new Vector2(screenWidth, screenHeight) * 0.95f, SpriteBlendMode.AlphaBlend, new Color(32, 32, 90, 220), Util.SPRITE_ALIGN_TOP_LEFT);
                    
            /*
            Texture2D bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"Images\Menus\labelbox");
            m_Game.batch.Begin(SpriteBlendMode.AlphaBlend);
            gdev.RenderState.DepthBufferWriteEnable = false;
            m_Game.batch.Draw(bg,
                                    new Rectangle(0, 0, gdev.PresentationParameters.BackBufferWidth, gdev.PresentationParameters.BackBufferHeight),
                                    Color.White);
            m_Game.batch.End();
            */
            menu.Draw();

            // Logo
            int logoWidth = Util.GetImageWidth("Menus\\logo");
            int logoHeight = Util.GetImageHeight("Menus\\logo");

            float logoW = logoWidth * menu.scale * GameVars.destLogoScale;
            float logoH = logoHeight * menu.scale * GameVars.destLogoScale;

            Util.DrawSprite(device, "Menus\\logo", new Vector2(device.PresentationParameters.BackBufferWidth / 2, device.PresentationParameters.BackBufferHeight / 6), new Vector2(logoW, logoH), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_CENTERED);
            
        }
    }
}

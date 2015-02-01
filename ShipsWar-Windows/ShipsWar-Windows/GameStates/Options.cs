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
    class Options: GameState
    {
        Texture2D bg;
        Menu menu;
        MenuBackgroundRender mbr;

        public override bool IsFullScreen() { return true; }

        public Options(Game1 g)
            : base(g)
        {
            //bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\MainMenu");
            menu = new Menu();
            menu.Init(0, 0, "Options", "Change", "Back");
            
            List<String> shipOps = new List<String>();
            shipOps.Add("Yes");
            shipOps.Add("No");

            List<String> aspectRatioOps = new List<String>();
            aspectRatioOps.Add("Normal");
            aspectRatioOps.Add("16:9");
            //aspectRatioOps.Add("16:10");

            List<String> soundOps = new List<String>();
            soundOps.Add("Low");
            soundOps.Add("Medium");
            soundOps.Add("High");
            

            menu.AddOption(0, "See CPU Landed Ships", shipOps, GameVars.seeAIShips? 0:1);
            menu.AddOption(0, "Video Aspect Ratio", aspectRatioOps, GameVars.aspectRatio);

            

            int musicVolume = 1;
            if (GameVars.musicVolume < 0.5f) musicVolume = 0;
            if (GameVars.musicVolume > 0.7f) musicVolume = 2;


            int soundVolume = 1;
            if (GameVars.soundVolume < 0.5f) soundVolume = 0;
            if (GameVars.soundVolume > 0.7f) soundVolume = 2;

            menu.AddOption(0, "Music Volume", soundOps, musicVolume);
            menu.AddOption(0, "Sound Volume", soundOps, soundVolume);

#if !XBOX
            menu.AddOption(0, "FullScreen", shipOps, GameVars.fullScreenMode ? 0 : 1);

#endif

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
            GameVars.seeAIShips = menu.GetSubOption(0) == 0;
            
            int lastAspectRatio = GameVars.aspectRatio;
            GameVars.aspectRatio = menu.GetSubOption(1);
            if (GameVars.aspectRatio != lastAspectRatio)
            {
                if (GameVars.aspectRatio == 1)
                {
                    m_Game.SetUpXNADevice(true);
                }
                else
                {
                    m_Game.SetUpXNADevice(false);
                }
            }

            switch(menu.GetSubOption(2))
            {
                case 0:
                    GameVars.musicVolume = 0.33f;
                    break;
                case 1:
                    GameVars.musicVolume = 0.66f;
                    break;
                case 2:
                    GameVars.musicVolume = 1.0f;
                    break;
            }

            switch (menu.GetSubOption(3))
            {
                case 0:
                    GameVars.soundVolume = 0.33f;
                    break;
                case 1:
                    GameVars.soundVolume = 0.66f;
                    break;
                case 2:
                    GameVars.soundVolume = 1.0f;
                    break;
            }

            #if !XBOX
            bool lastFSM = GameVars.fullScreenMode;
            GameVars.fullScreenMode = menu.GetSubOption(4) == 0;
            if (GameVars.fullScreenMode != lastFSM)
            {
                if (GameVars.aspectRatio == 1)
                {
                    m_Game.SetUpXNADevice(true);
                }
                else
                {
                    m_Game.SetUpXNADevice(false);
                }            
            }
            #endif




            if (action >= 0)
            {
                SoundManager.Update();
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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShipsWar.Menus;

namespace ShipsWar.GameStates
{
    class PopUp: GameState
    {
        private string m_File;
        private string m_FilePlanet;
        public override bool IsFullScreen() { return false; }

        String popUpText;
        TextScreen textScreen;

        public PopUp(Game1 g, String _popUpText)
            : base(g)
        {
            popUpText = _popUpText;

            int maxSize = m_Game.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2;
            maxSize = (int)((float)maxSize / Game1.menuScale);
            //bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\MainMenu");
            
            textScreen = new TextScreen();

            textScreen.Init(0, 0, "Tutorial", "Continue", null);

            textScreen.AddText(0, "  ", maxSize);

            textScreen.AddText(0, "  ", maxSize);

            textScreen.AddText(1, popUpText, maxSize);

        /*
            SoundManager.SoundStop();

            if (win)
            {
                SoundManager.SoundPlay(SoundManager.MUSIC_VICTORY);
                m_File = @"Menus\victorytext";
                m_FilePlanet = @"Menus\victoryplanet";
            }
            else
            {
                SoundManager.SoundPlay(SoundManager.MUSIC_DEFEAT);
                m_File = @"Menus\defeattext";
                m_FilePlanet = @"Menus\defeatplanet";
            
            }
            winningPlayer = player;
        */
        }

        /// <summary>
        /// Allows the game state to run logic.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The next gamestate to transition to, this to stay, null to pop back to previous state. </returns>
        public override GameState Update(GameTime gameTime)
        {
            return DoAction(textScreen.Update(gameTime));
            /*if (Input.WasPressed(-1, Input.BACK | Input.HIT | Input.START))
                return null;
            return this;*/
        }

         public GameState DoAction(int action)
        {
            switch (action)
            {
                case 0:
                case 1:
                    if (textScreen.menuExited)
                    {
                        return null;                    
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

             // Logo
            int logoWidth = Util.GetImageWidth("Menus\\logo");
            int logoHeight = Util.GetImageHeight("Menus\\logo");

            float logoW = logoWidth * Game1.menuScale * GameVars.destLogoScale;
            float logoH = logoHeight * Game1.menuScale * GameVars.destLogoScale;

            Util.DrawSprite(gdev, "Menus\\logo", new Vector2(gdev.PresentationParameters.BackBufferWidth / 2, gdev.PresentationParameters.BackBufferHeight / 6), new Vector2(logoW, logoH), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_CENTERED);
            
            textScreen.Draw();

            /*
            float[][] sideColors = new float[8][];
            float[] c1 = { 0.0f, 1.0f, 0.0f, 1.0f };
            sideColors[0] = c1;
            float[] c2 = { 1.0f, 0.0f, 0.0f, 1.0f };
            sideColors[1] = c2;
            float[] c3 = { 0.0f, 0.0f, 1.0f, 1.0f };
            sideColors[2] = c3;
            float[] c4 = { 1.0f, 1.0f, 0.0f, 1.0f };
            sideColors[3] = c4;

            
            GraphicsDevice gdev = m_Game.graphics.GraphicsDevice;



            Util.DrawSprite(gdev, m_FilePlanet, new Vector2(gdev.PresentationParameters.BackBufferWidth / 2, gdev.PresentationParameters.BackBufferHeight / 2),
                new Vector2(gdev.PresentationParameters.BackBufferHeight / 2, gdev.PresentationParameters.BackBufferHeight / 2), 
                SpriteBlendMode.AlphaBlend, 
                new Color(new Vector4(sideColors[winningPlayer - 1][0], sideColors[winningPlayer - 1][1], sideColors[winningPlayer - 1][2], sideColors[winningPlayer - 1][3])), Util.SPRITE_ALIGN_CENTERED);


            int logoWidth = Util.GetImageWidth(m_File);
            int logoHeight = Util.GetImageHeight(m_File);

            float logoW = logoWidth * Game1.menuScale;
            float logoH = logoHeight * Game1.menuScale;
        

            Util.DrawSprite(gdev, m_File, new Vector2(gdev.PresentationParameters.BackBufferWidth / 2, gdev.PresentationParameters.BackBufferHeight / 2),
                new Vector2(logoW, logoH),
                SpriteBlendMode.AlphaBlend,
                Color.White, Util.SPRITE_ALIGN_CENTERED);
            
            DrawActionKeys();
             */ 
        }
        /*
        public void DrawActionKeys()
        {
            Vector2 actionPos;
            Color colorin, colorout;
            GraphicsDevice device = m_Game.graphics.GraphicsDevice;
            int screenWidth = device.PresentationParameters.BackBufferWidth;
            int screenHeight = device.PresentationParameters.BackBufferHeight;

            int marginSize = screenHeight / 32;
            int iconSize = screenHeight / 24;

            colorin = new Color(new Vector4(1, 1, 1, 1));
            colorout = new Color(new Vector4(0, 0, 0, 1));

            // Action Keys
            actionPos = new Vector2((screenWidth / 2), screenHeight - (marginSize * 2) - iconSize);

            if (GameHUD.spriteFont != null)
            {
                Util.DrawSprite(device, "Menus/a-button", actionPos, new Vector2(iconSize, iconSize), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_TOP_LEFT);
                actionPos.X += iconSize + marginSize;
                Util.DrawText(GameHUD.spriteFont, "Continue", actionPos, colorin, colorout, new Vector2(), Game1.menuScale, SpriteEffects.None);
            }
        }
        */
    }
}

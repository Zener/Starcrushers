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
    class Status: GameState
    {
        private string m_File;
        private string m_FilePlanet;
        public override bool IsFullScreen() { return false; }
        private int winningPlayer;

        public Status(Game1 g, int player, bool win): base(g)
        {
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
        
        }

        /// <summary>
        /// Allows the game state to run logic.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The next gamestate to transition to, this to stay, null to pop back to previous state. </returns>
        public override GameState Update(GameTime gameTime)
        {
            if (Input.WasPressed(-1, Input.BACK | Input.HIT | Input.START) || Input.mouseLeftRamp)
                return null;
            return this;
        }

        /// <summary>
        /// This is called when the game state should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
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
            
            /*
            Texture2D bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + m_File);

            m_Game.batch.Begin(SpriteBlendMode.AlphaBlend);
            gdev.RenderState.DepthBufferWriteEnable = false;
            m_Game.batch.Draw(bg,
                                    new Rectangle(0, 0, gdev.PresentationParameters.BackBufferHeight / 2, gdev.PresentationParameters.BackBufferHeight / 2),
                                    new Color(new Vector4(sideColors[winningPlayer - 1][0], sideColors[winningPlayer - 1][1], sideColors[winningPlayer - 1][2], sideColors[winningPlayer - 1][3])));
            m_Game.batch.End();
            */
            DrawActionKeys();
        }

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

    }
}

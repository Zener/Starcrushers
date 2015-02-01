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
    class Results: GameState
    {
        Texture2D bg;
        private int screenWidth, screenHeight;
        GraphicsDevice device;
        private SpriteFont spriteFont;
        float scale = 1.0f;

        public override bool IsFullScreen() { return true; }

        public Results(Game1 g)
            : base(g)
        {
            //bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\Menus\backspace");
            
            device = m_Game.graphics.GraphicsDevice;
            spriteFont = Game1.main.content.Load<SpriteFont>(Game1.main.DataPath + @"Fonts\Courier New");

            SoundManager.SoundStop();
            SoundManager.SoundPlay(SoundManager.MUSIC_STATISTICS);
            
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
            if (Input.WasPressed(-1, Input.HIT | Input.START) || Input.mouseLeftRamp)
                return new GameStates.MainMenu(m_Game);
            return this;
        }

        /// <summary>
        /// This is called when the game state should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {

            scale = Game1.menuScale;
            Color colorin, colorout;
            colorin = new Color(new Vector4(1, 1, 1, 1));
            colorout = new Color(new Vector4(0, 0, 0, 1));


            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

            m_Game.DrawBackgroundImage(@"Menus\base");
            /*
            bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\Menus\backspace");
            m_Game.batch.Begin(SpriteBlendMode.None);
            device.RenderState.DepthBufferWriteEnable = false;
            m_Game.batch.Draw(bg,
                                    new Rectangle(0, 0, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight),
                                    Color.White);
            m_Game.batch.End();
            */
            //game time
            Util.DrawText(spriteFont, "Total Time: " + Statistics.totalGameTime.Hours + ":" + Statistics.totalGameTime.Minutes+":"+Statistics.totalGameTime.Seconds, new Vector2((screenWidth / 5) * 1, (screenHeight / 4)), colorin, colorout, new Vector2(), scale, SpriteEffects.None);


            Vector2 pos = new Vector2((screenWidth / 5)*3, (screenHeight/4));
            // Points
            for(int i = 0; i < Statistics.nplayers;i++)
            {
                Util.DrawText(spriteFont, "Score Player "+i + ": "+(Statistics.points[i]*10) , pos, colorin, colorout, new Vector2(), scale, SpriteEffects.None);

                pos.Y += 45 * scale;
            }

            //Statistics
            Util.DrawSprite(device, "Menus/labelboxalpha", new Vector2(screenWidth / 16, screenHeight - (screenHeight / 3) - (screenHeight / 8)-8), new Vector2(screenWidth - (screenWidth / 8), (screenHeight / 3)+16), SpriteBlendMode.AlphaBlend, new Color(64, 64, 64, 255), Util.SPRITE_ALIGN_TOP_LEFT);


            float[][] sideColors = new float[8][];
            float[] c1 = { 0.0f, 1.0f, 0.0f, 1.0f };
            sideColors[0] = c1;
            float[] c2 = { 1.0f, 0.0f, 0.0f, 1.0f };
            sideColors[1] = c2;
            float[] c3 = { 0.0f, 0.0f, 1.0f, 1.0f };
            sideColors[2] = c3;
            float[] c4 = { 1.0f, 1.0f, 0.0f, 1.0f };
            sideColors[3] = c4;

            float step = (screenWidth - (screenWidth / 8)) / Statistics.planetDominion[0].Count;
            Vector2 size = new Vector2(step, 2);
            

            for (int i = 0; i < Statistics.nplayers; i++)
            {
                Color c = new Color(new Vector4(sideColors[i][0], sideColors[i][1], sideColors[i][2], sideColors[i][3]));
                pos.X = (screenWidth / 16);

                for (int j = 0; j < Statistics.planetDominion[i].Count; j++)
                {
                    float value = Statistics.planetDominion[i][j];
                    value = (value * (screenHeight / 3)) / Statistics.universeSize;

                    float nextvalue = 0;
                    if (j > 0)
                    {
                        nextvalue = Statistics.planetDominion[i][j-1];
                        nextvalue = (nextvalue * (screenHeight / 3)) / Statistics.universeSize;
                        if (nextvalue > value)
                        {
                            pos.Y = screenHeight - (screenHeight / 8) - nextvalue - (i*2) ;
                        }
                        else
                        {
                            pos.Y = screenHeight - (screenHeight / 8) - value - (i * 2);
                        }
                        Vector2 vsize = new Vector2(2, Math.Abs(nextvalue - value)+2);
                        Util.DrawSprite(device, "Menus/labelboxwhite", pos, vsize, SpriteBlendMode.None, c, Util.SPRITE_ALIGN_TOP_LEFT);
                    }
                    pos.Y = screenHeight - (screenHeight / 8) - value - (i * 2);
                
                    Util.DrawSprite(device, "Menus/labelboxwhite", pos, size, SpriteBlendMode.None, c, Util.SPRITE_ALIGN_TOP_LEFT);
                    pos.X += step;
                }
            }
            Util.DrawText(spriteFont, "Power Graph", new Vector2(screenWidth / 16, screenHeight - (screenHeight / 3) - (screenHeight / 8) - (screenHeight / 32)), colorin, colorout, new Vector2(), scale, SpriteEffects.None);

            int logoWidth = Util.GetImageWidth("Menus\\logo");
            int logoHeight = Util.GetImageHeight("Menus\\logo");

            float logoW = logoWidth * Game1.menuScale * GameVars.destLogoScale;
            float logoH = logoHeight * Game1.menuScale * GameVars.destLogoScale;

            Util.DrawSprite(device, "Menus\\logo", new Vector2(device.PresentationParameters.BackBufferWidth / 2, device.PresentationParameters.BackBufferHeight / 6), new Vector2(logoW, logoH), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_CENTERED);
            

            DrawActionKeys();
        }


        public void DrawActionKeys()
        {
            Vector2 actionPos;
            Color colorin, colorout;
            int marginSize = screenHeight / 32;
            int iconSize = screenHeight / 24;

            colorin = new Color(new Vector4(1, 1, 1, 1));
            colorout = new Color(new Vector4(0, 0, 0, 1));

            // Action Keys
            actionPos = new Vector2(screenWidth/2, screenHeight - (marginSize * 2) - iconSize);


            Util.DrawSprite(device, "Menus/a-button", actionPos, new Vector2(iconSize, iconSize), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_TOP_LEFT);
            actionPos.X += iconSize + marginSize;
            Util.DrawText(spriteFont, "Continue", actionPos, colorin, colorout, new Vector2(), scale, SpriteEffects.None);
        }
    }
}

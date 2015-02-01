///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////



using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;


namespace ShipsWar
{
    class GameHUD
    {
        static int playerCount;
        static int screenWidth;
        static int screenHeight;
        static GraphicsDevice device;
        static Vector4[] canvas;
        static int iconSize;
        static int marginSize;
        static int cursorSize;
        
        static Player[] players;
        static Universe universe;
        public static SpriteFont spriteFont;

        public static void Start(Player[] _players, Universe _universe)
        {
            players = _players;
            universe = _universe;
            playerCount = _players.Length;
            device = Game1.main.graphics.GraphicsDevice;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            
            canvas = new Vector4[playerCount];

            int hudWidth = screenWidth / 20;
            int hudHeight = screenHeight / ((playerCount / 2) + (playerCount % 2));

            for (int i = 0; i < playerCount; i++)
            {
                canvas[i].W = hudWidth;
                canvas[i].Z = hudHeight;             
            }

            int hudUnit = hudWidth / ((1 * 4) + 1);
            iconSize = hudUnit * 3;
            marginSize = hudUnit;


            switch (playerCount)
            {
                case 1:
                    canvas[0].X = marginSize * 3;
                    canvas[0].Y = 0;
                    break;
                case 2:
                    canvas[0].X = (marginSize * 3);
                    canvas[0].Y = 0;
                    canvas[1].X = screenWidth - hudWidth - (marginSize * 3);
                    canvas[1].Y = 0;
                    break;
                case 3:
                    canvas[0].X = (marginSize * 3);
                    canvas[0].Y = 0;
                    canvas[1].X = (marginSize * 3);
                    canvas[1].Y = screenHeight / 2;
                    canvas[2].X = screenWidth - hudWidth - (marginSize * 3);
                    canvas[2].Y = 0;
                    break;
        
                case 4:
                    canvas[0].X = (marginSize * 3);
                    canvas[0].Y = 0;
                    canvas[1].X = (marginSize * 3);
                    canvas[1].Y = screenHeight / 2;
                    canvas[2].X = screenWidth - hudWidth - (marginSize * 3);
                    canvas[2].Y = 0;
                    canvas[3].X = screenWidth - hudWidth - (marginSize * 3);
                    canvas[3].Y = screenHeight / 2;
                    break;
        
            }

            
            cursorSize = screenHeight / 10;

            spriteFont = Game1.main.content.Load<SpriteFont>(Game1.main.DataPath + @"Fonts\Courier New");

        }



        


        public static void Draw(Matrix view, Matrix  projection)
        {
            SpriteBatch spriteBatch = Game1.main.batch;
            //spriteBatch.Begin();

            // Number of ships on planet
            for (int i = 0; i < universe.planets.Length; i++)
            {
                if (universe.planets[i].side != 0)
                {
                    if (!GameVars.seeAIShips && players[universe.planets[i].side - 1].playerType == Player.COMP_PLAYER) continue;
                
                    string ships = ""+universe.planets[i].ships.Count;
                    Vector2 FontOrigin = spriteFont.MeasureString(ships) / 2;
                    Vector2 pos = Util.TransformTo2D(universe.planets[i].getPosition(), view, projection);
                    //spriteBatch.DrawString(spriteFont, ""+ships, Util.TransformTo2D(universe.planets[i].getPosition(), view, projection), Color.White);
                    float fontScale = 0.75f * Game1.gameScale;
                    if (fontScale < 0.4f) fontScale = 0.4f;
                    #if XBOX
                    fontScale *= 1.25f;
                    #endif
                    Util.DrawText(spriteFont, ships, pos, Color.White, Color.Black, FontOrigin, fontScale, SpriteEffects.None);
                }
            }
            //spriteBatch.End();


            int numControllable = 0;
            for (int i = 0; i < playerCount; i++)
            {
                int x = (int)canvas[i].X;
                int y = (int)canvas[i].Y;
                int h = (int)canvas[i].Z;

                Util.DrawSprite(device, "Hud/planetHud", new Vector2(marginSize + x, y + h - (marginSize*4) - iconSize), new Vector2(iconSize, iconSize), SpriteBlendMode.None, Color.White, Util.SPRITE_ALIGN_TOP_LEFT);
              
                float[] playerColorF = players[i].GetSideColor();
                Vector4 playerColorV = new Vector4(playerColorF[0], playerColorF[1], playerColorF[2], playerColorF[3]);
                Color playerColor = new Color(playerColorV);

                float usableHeight = h - (marginSize * 10) - iconSize;

                float barHeight = players[i].GetOwnedPercentage() * usableHeight;
                Util.DrawSprite(device, "Hud/bar", new Vector2(marginSize + x + iconSize / 4, y + h - (marginSize * 5) - iconSize - barHeight), new Vector2(iconSize / 2, barHeight), SpriteBlendMode.Additive, playerColor, Util.SPRITE_ALIGN_TOP_LEFT);

           

                //Cursor
                if (players[i].playerType == Player.HUMAN_PLAYER)
                {
                    if (players[i].planetFocusedID != -1)
                    {

                        Vector2 cursorPos = Util.TransformTo2D(universe.planets[players[i].planetFocusedID].getPosition(), view, projection);
                        float despValue = (screenHeight / 512.0f);
                        if (i % 2 == 0) despValue = -despValue;
                        if (i >= 2) despValue = despValue * 2;
                        cursorPos.X += despValue;
                        cursorPos.Y += despValue;
                        
                        float fadeValue = (float)universe.universeTime.TotalSeconds*5;
                        fadeValue = (float)Math.Sin(fadeValue);
                        Color c = new Color(/*playerColorV +*/ (new Vector4(1.0f, 1.0f, 1.0f, 0.7f) * (float)(fadeValue)));

                        Util.DrawSprite(device, "Hud/cursor", cursorPos, new Vector2(cursorSize, cursorSize), SpriteBlendMode.Additive, playerColor, Util.SPRITE_ALIGN_CENTERED);
                        //TODO (que se vea bien)
                        //, (float)(universe.universeTime.TotalMilliseconds / 1000.0f)
                        Util.DrawSprite(device, "Hud/cursor", cursorPos, new Vector2(cursorSize, cursorSize), SpriteBlendMode.AlphaBlend, c, Util.SPRITE_ALIGN_CENTERED);
                     
                        Util.DrawSprite(device, "Hud/overcursor", cursorPos, new Vector2(cursorSize, cursorSize), SpriteBlendMode.AlphaBlend, playerColor, Util.SPRITE_ALIGN_CENTERED);

                        

                        //Util.DrawSprite(device, "Hud/cursor", cursorPos, new Vector2(cursorSize, cursorSize), SpriteBlendMode.Additive, playerColor, Util.SPRITE_ALIGN_CENTERED);
                    }
                    #if !XBOX
                    if (numControllable == 0)
                    {
                        Vector2 mousePos = new Vector2(Input.mouseX, Input.mouseY);
                        Util.DrawSprite(device, "Hud/overcursor", mousePos, new Vector2(cursorSize, cursorSize), SpriteBlendMode.AlphaBlend, playerColor, Util.SPRITE_ALIGN_CENTERED);
                    }
                    #endif
                    numControllable++;
                }
            }

                
        }


        public static void DrawRemainingTimeToStart(String _text, Color _colorin, Color _colorout, float scale)
        {
            String file = @"Menus\getready";
            int logoWidth = Util.GetImageWidth(file);
            int logoHeight = Util.GetImageHeight(file);

            float logoW = logoWidth * Game1.menuScale;
            float logoH = logoHeight * Game1.menuScale;


            Util.DrawSprite(device, file, new Vector2(device.PresentationParameters.BackBufferWidth / 2, device.PresentationParameters.BackBufferHeight / 2),
                new Vector2(logoW, logoH),
                SpriteBlendMode.AlphaBlend,
                Color.White, Util.SPRITE_ALIGN_CENTERED);
/*            
            Vector2 pos = new Vector2(screenWidth / 2, screenHeight / 2);
            Vector2 fontOrigin = spriteFont.MeasureString(_text) / 2;

            Util.DrawText(spriteFont, _text, pos, _colorin, _colorout, fontOrigin, scale, SpriteEffects.None);
 */
        }
    }
}

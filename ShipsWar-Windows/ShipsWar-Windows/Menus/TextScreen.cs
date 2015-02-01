///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;



namespace ShipsWar.Menus
{
    public class TextScreen
    {
        class TextItem
        {
            public int type;
            public String text;
            public int action;
            public float scale;
        };

        public float scale = 1.0f;
        private SpriteFont spriteFont;
        private List<TextItem> items;
        private int screenWidth, screenHeight;
        GraphicsDevice device;
        public double firstLine;
        public const int MENU_ACTION_NONE = -1;
        public const int MENU_ACTION_BACK = 0;
        public const int MENU_ACTION_NEXT = 1;
        String titleText;
        String buttonAText;
        String buttonBText;
        public bool exitSequence;
        public bool menuExited;
        int lastOption;
        TimeSpan timeSpan;
        TimeSpan exitSpan;


        public void Init(int _x, int _y, String _tt, String _t1, String _t2)
        {
            scale = Game1.menuScale;
            
            items = new List<TextItem>();
            device = Game1.main.graphics.GraphicsDevice;
            spriteFont = Game1.main.content.Load<SpriteFont>(Game1.main.DataPath + @"Fonts\Courier New");
            firstLine = 0.0f;
            titleText = _tt;
            buttonAText = _t1;
            buttonBText = _t2;
            exitSequence = false;
        }


        public void AddText(int _type, String _text, int _size)
        {

            List<String> cutText = SplitText(_text, _size);
            for (int i = 0; i < cutText.Count; i++)
            {
                InternalAddText(_type, cutText[i]);
            }
        }    
        
        private void InternalAddText(int _type, String _text)
        {
            TextItem mi = new TextItem();
            mi.type = _type;
            mi.text = _text;
            mi.scale = 0.4f;
            //mi.action = _action;
            items.Add(mi);
        }


        public int Update(GameTime gt)
        {
            if (exitSequence)
            {
                exitSpan = gt.TotalGameTime - timeSpan;
              
                if (exitSpan > new TimeSpan(0,0,0,0,500))
                {
                    menuExited = true;
                }
                return lastOption;
            }
            timeSpan = gt.TotalGameTime;

            int res = -1;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].scale < 1.0f)
                {
                    items[i].scale += 0.001f * (float)gt.ElapsedGameTime.TotalMilliseconds;
                }
                if (items[i].type == 0)
                {
                    if (items[i].scale < 1.0f)
                        items[i].scale += 0.001f * (float)gt.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    if (items[i].scale > 1.0f)
                        items[i].scale -= 0.001f * (float)gt.ElapsedGameTime.TotalMilliseconds;
                }
            }
            /*
            if (Input.WasPressed(-1, Input.DOWN))
            {
                firstLine++;
            }
            if (Input.WasPressed(-1, Input.UP))
            {
                firstLine--;
            }
            */

            if (firstLine < items.Count - 5)
            {
                firstLine += (gt.ElapsedGameTime.TotalMilliseconds * 0.0005f);
            }
            if (Input.WasPressed(-1, Input.BACK | Input.HIT2 | Input.HIT | Input.START) || Input.mouseRightPressed || Input.mouseLeftRamp)
            {
                res = MENU_ACTION_BACK;
            }
            /*if (Input.WasPressed(-1, Input.HIT | Input.START))
            {
                res = items[optionSelected].action;
            }*/
            lastOption = res;
            return res;
        }



        public List<String> SplitText(String inString, int lineWidth)
        {
            List<String> output;
            int start = 0;
            int len = 1;
            String line;

            output = new List<String>();
            line = inString.Substring(start, 1);               

            while (start + len < inString.Length)
            {
                
                line = inString.Substring(start, len);
                while (start + len < inString.Length && spriteFont.MeasureString(line).X < lineWidth)
                {
                    len++;
                    line = inString.Substring(start, len);            
                }
                //check for work spacing
                bool ok = false;
                if (start + len < inString.Length - 1)
                {
                    int endLine = len;
                    while (!ok && endLine > 0)
                    {
                        String st = inString.Substring(start + endLine, 1);
                        if (st.CompareTo(" ") != 0)
                        {
                            endLine--;
                        }
                        else ok = true;
                    }
                    len = endLine+1;
                }
                /*
                if (spriteFont.MeasureString(line).X >= lineWidth)
                    len--;
                */
                output.Add(inString.Substring(start, len));
                start = start + len;
                len = 1;

            }
            return output;
            //start = 
        }

        public void Draw()
        {
            scale = Game1.menuScale;
            Color colorin, colorout;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

            //SpriteBatch spriteBatch = Game1.main.batch;
            Vector2 titlePos = new Vector2(screenWidth/8, screenHeight/3);
            colorin = new Color(new Vector4(1, 1, 1, 1));
            colorout = new Color(new Vector4(0, 0, 0, 1));
            Vector2 titleFontOrigin = new Vector2();
            Vector2 titleBoxPos = new Vector2(0, screenHeight / 3);
            Util.DrawSprite(device, "Menus/labelboxalpha", titleBoxPos, new Vector2(screenWidth / 3, 40 * scale), SpriteBlendMode.AlphaBlend, new Color(128, 128, 128), Util.SPRITE_ALIGN_TOP_LEFT);

            Util.DrawText(spriteFont, titleText, titlePos, colorin, colorout, titleFontOrigin, scale, SpriteEffects.None);


            // Label background
            Vector2 boxPos = new Vector2((screenWidth / 8) * 1.5f, screenHeight / 2);
            Vector2 boxSize = new Vector2((screenWidth / 8) * 5, (screenHeight / 2) - (screenHeight / 8));
            //boxPos.Y -= FontOrigin.Y * itemScale;
            //boxPos.X -= boxSize.X / 2;

            //boxPos.X += offsetX;
            Util.DrawSprite(device, "Menus/labelboxalpha", boxPos, boxSize, SpriteBlendMode.AlphaBlend, new Color(new Vector4(0.1f, 0.1f, 0.3f, 0.9f)), Util.SPRITE_ALIGN_TOP_LEFT);



            //float modY = (float)(firstLine - ((int)firstLine));

            float posY = (screenHeight / 2) - (float)(scale * firstLine * 45 * items[0].scale);// modY;
            
            for (int i = 0; i < items.Count; i++)
            {
                int offsetX = 0;
                if (exitSequence)
                {
                    offsetX = (int)((exitSpan.TotalMilliseconds * exitSpan.TotalMilliseconds * screenWidth) / 100000.0f);

                    if (i % 2 == 0)
                    {
                        offsetX = -offsetX;
                    }
                }

                float itemScale = scale * items[i].scale;
                float alpha = 1.0f;
  
                if (posY < (screenHeight/2) + (screenHeight / 12))
                {
                    float aY = posY - (screenHeight / 2);
                    alpha = aY / (screenHeight / 12);
                }

                if (posY > (screenHeight) - (screenHeight / 8) - (screenHeight / 16))
                {
                    float aY = screenHeight - posY - (screenHeight / 8);
                    alpha = aY / (screenHeight / 16);
                }

                if (alpha > 0.0f)
                {


                    if (items[i].type == 0)
                    {
                        colorin = new Color(new Vector4(1, 1, 1, alpha));
                        colorout = new Color(new Vector4(0, 0, 0, alpha));
                        //itemScale *= 1.2f;
                    }
                    else
                    {
                        colorin = new Color(new Vector4(0.9f, 0.9f, 0.9f, alpha));
                        colorout = new Color(new Vector4(0, 0, 0, alpha));
                    }

                    Vector2 pos = new Vector2(screenWidth / 4, posY);
                    Vector2 FontOrigin = spriteFont.MeasureString(items[i].text) / 2;
                    FontOrigin.X = 0;

                    pos.X += offsetX;

                    
                    Util.DrawText(spriteFont, items[i].text, pos, colorin, colorout, FontOrigin, itemScale, SpriteEffects.None);
                }
                posY += (45) * itemScale;
            }


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
             
            if (buttonAText != null)
            {

                Util.DrawSprite(device, "Menus/a-button", actionPos, new Vector2(iconSize, iconSize), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_TOP_LEFT);
                actionPos.X += iconSize + marginSize;
                Util.DrawText(spriteFont, buttonAText, actionPos, colorin, colorout, new Vector2(), scale, SpriteEffects.None);
            }
            if (buttonBText != null)
            {
                actionPos.X = screenWidth - (marginSize * 2);
                actionPos.X -= screenWidth / 4;
                Util.DrawSprite(device, "Menus/b-button", actionPos, new Vector2(iconSize, iconSize), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_TOP_LEFT);
                actionPos.X += iconSize + marginSize;

                Util.DrawText(spriteFont, buttonBText, actionPos, colorin, colorout, new Vector2(), scale, SpriteEffects.None);
            }
        }
    }
}

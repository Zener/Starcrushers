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
    public class Menu
    {
        class MenuItem
        {
            public int type;
            public String text;
            public int action;
            public float scale;
            public List<String> ops;
            public int subObtion;
            public Vector4 colBox;
        };

        public float scale = 1.0f;
        private SpriteFont spriteFont;
        private List<MenuItem> items;
        private int screenWidth, screenHeight;
        GraphicsDevice device;
        public int optionSelected;
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
            items = new List<MenuItem>();
            device = Game1.main.graphics.GraphicsDevice;
            spriteFont = Game1.main.content.Load<SpriteFont>(Game1.main.DataPath + @"Fonts\Courier New");
            optionSelected = 0;
            titleText = _tt;
            buttonAText = _t1;
            buttonBText = _t2;
            exitSequence = false;
            scale = Game1.menuScale;
            
        }


        public void AddOption(int _type, String _text, int _action)
        {
            MenuItem mi = new MenuItem();
            mi.type = _type;
            mi.text = _text;
            mi.scale = 0.4f;
            mi.action = _action;
            mi.colBox = new Vector4();
            items.Add(mi);
        }


        public void AddOption(int _type, String _text, List<String> _ops, int _default)
        {
            MenuItem mi = new MenuItem();
            mi.type = _type;
            mi.text = _text;
            mi.scale = 0.4f;
            mi.action = -1;
            mi.ops = _ops;
            mi.subObtion = _default;
            mi.colBox = new Vector4();
            items.Add(mi);
        }


        public int GetSubOption(int _item)
        {
            return items[_item].subObtion;
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
                if (i == optionSelected)
                {
                    if (items[i].scale < 1.2f)
                        items[i].scale += 0.001f * (float)gt.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    if (items[i].scale > 1.0f)
                        items[i].scale -= 0.001f * (float)gt.ElapsedGameTime.TotalMilliseconds;
                }
            }

            if (Input.WasPressed(-1, Input.DOWN))
            {
                if (optionSelected < items.Count - 1)
                {
                    optionSelected++;
                    SoundManager.SoundPlay(SoundManager.MUSIC_OPTION);
                }
            }
            if (Input.WasPressed(-1, Input.UP))
            {
                if (optionSelected > 0)
                {
                    optionSelected--;
                    SoundManager.SoundPlay(SoundManager.MUSIC_OPTION);
                }
            }

            if (Input.WasPressed(-1, Input.BACK | Input.HIT2))
            {
                SoundManager.SoundPlay(SoundManager.MUSIC_ACCEPT);                
                res = MENU_ACTION_BACK;
                //startState.DoAction(1);
                //return null;
            }
            if (Input.WasPressed(-1, Input.HIT | Input.START))
            {

                SoundManager.SoundPlay(SoundManager.MUSIC_ACCEPT);
                if (items[optionSelected].action != -1)
                {
                    res = items[optionSelected].action;
                }
                else
                {
                    items[optionSelected].subObtion = (items[optionSelected].subObtion + 1) % items[optionSelected].ops.Count;
                    res = 3;
                }
                //startState.DoAction(2);
                //return new GameStates.War(m_Game);
            }

            // Mouse handling
            for (int i = 0; i < items.Count; i++)
            {
                Vector4 colBox = items[i].colBox;
                if (Input.mouseY >= colBox.Y && Input.mouseY < colBox.Y + colBox.W
                    && Input.mouseX >= colBox.X && Input.mouseX < colBox.X + colBox.Z)
                {
                    if (i != optionSelected)
                    {
                        optionSelected = i;
                        SoundManager.SoundPlay(SoundManager.MUSIC_OPTION);
                    }
                    if (Input.mouseLeftRamp)
                    {
                        SoundManager.SoundPlay(SoundManager.MUSIC_ACCEPT);
                        if (items[optionSelected].action != -1)
                        {
                            res = items[optionSelected].action;
                        }
                        else
                        {
                            items[optionSelected].subObtion = (items[optionSelected].subObtion + 1) % items[optionSelected].ops.Count;
                            res = 3;
                        }
                    }
                }
            }
            if (Input.mouseRightRamp)
            {
                SoundManager.SoundPlay(SoundManager.MUSIC_ACCEPT);                
                res = MENU_ACTION_BACK;
            }
            // End mouse handling
            lastOption = res;
            return res;
        }


        public void Draw()
        {
            scale = Game1.menuScale;
             
            Color colorin, colorout, coloroption;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

            //SpriteBatch spriteBatch = Game1.main.batch;
            Vector2 titlePos = new Vector2(screenWidth/8, screenHeight/3);
            colorin = new Color(new Vector4(1, 1, 1, 1));
            colorout = new Color(new Vector4(0, 0, 0, 1));
            Vector2 titleFontOrigin = new Vector2();
            Vector2 titleBoxPos = new Vector2(0, screenHeight / 3);
            Util.DrawSprite(device, "Menus/labelboxalpha", titleBoxPos, new Vector2(screenWidth / 2.5f, 40 * scale), SpriteBlendMode.AlphaBlend, new Color(128, 128, 128), Util.SPRITE_ALIGN_TOP_LEFT);

            Util.DrawText(spriteFont, titleText, titlePos, colorin, colorout, titleFontOrigin, scale, SpriteEffects.None);



            float posY = screenHeight / 2;

            for (int i = 0; i < items.Count; i++)
            {
                int offsetX = 0;
                if (exitSequence)
                {
                    offsetX = (int)((exitSpan.TotalMilliseconds * exitSpan.TotalMilliseconds * screenWidth) / 100000.0f);

                    if (i % 2 == 0) offsetX = -offsetX;
                }

                float itemScale = items[i].scale * Game1.menuScale;
                  

                
                if (i == optionSelected)
                {
                    colorin = new Color(new Vector4(1, 1, 1, 1));
                    colorout = new Color(new Vector4(0, 0, 0, 1));
                    coloroption =  new Color(200,200,220);
                    //itemScale *= 1.2f;
                }
                else
                {
                    colorin = new Color(new Vector4(0.8f, 0.8f, 0.8f, 0.8f));
                    colorout = new Color(new Vector4(0, 0, 0, 1));
                    coloroption = new Color(128,128,128);
                }

                Vector2 pos = new Vector2(screenWidth / 2, posY);
                Vector2 FontOrigin = spriteFont.MeasureString(items[i].text) / 2;
                

                // Label background
                Vector2 boxPos = new Vector2(screenWidth / 2, posY);
                Vector2 boxSize = new Vector2(200, 40) * itemScale;
                boxPos.Y -= FontOrigin.Y*itemScale;
                boxPos.X -= boxSize.X / 2;
                
                boxPos.X += offsetX;
                pos.X += offsetX;

                if (items[i].action == -1)
                {
                    boxPos.X -= screenWidth / 4;
                    pos.X -= screenWidth / 6;
                    boxSize.X += screenWidth / 6;
                }

                items[i].colBox.X = boxPos.X;
                items[i].colBox.Y = boxPos.Y;
                items[i].colBox.Z = boxSize.X;
                items[i].colBox.W = boxSize.Y;



                Util.DrawSprite(device, "Menus/labelboxalpha", boxPos, boxSize, SpriteBlendMode.AlphaBlend, coloroption, Util.SPRITE_ALIGN_TOP_LEFT);
               
                Util.DrawText(spriteFont, items[i].text, pos, colorin, colorout, FontOrigin, itemScale, SpriteEffects.None);

                if (items[i].action == -1)
                {
                    boxSize.X -= screenWidth / 5;
                    boxPos.X += screenWidth / 4;
                    String opText = items[i].ops[items[i].subObtion];

                    Util.DrawSprite(device, "Menus/labelboxalpha", boxPos + new Vector2(screenWidth / 3, 0), boxSize, SpriteBlendMode.AlphaBlend, coloroption, Util.SPRITE_ALIGN_TOP_LEFT);

                    Util.DrawText(spriteFont, opText, boxPos + new Vector2(screenWidth/3, 0), colorin, colorout, new Vector2(), itemScale, SpriteEffects.None);

                }

                posY += (45) * itemScale;
            }


            DrawActionKeys();

            #if !XBOX
            // Draw Cursor
            int cursorSize = screenHeight / 10;
            Vector2 mousePos = new Vector2(Input.mouseX, Input.mouseY);
            Util.DrawSprite(device, "Hud/overcursor", mousePos, new Vector2(cursorSize, cursorSize), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_CENTERED);
            #endif
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
            actionPos = new Vector2((marginSize * 4), screenHeight - (marginSize * 2) - iconSize);
             
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

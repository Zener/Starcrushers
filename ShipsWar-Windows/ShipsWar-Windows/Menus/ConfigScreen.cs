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
    public class ConfigScreen
    {
        class MenuItem
        {
            public int type;
            public String text;
            public int action;
            public float scale;
            public Vector4 colBox;
        };

        public float scale = 1.0f;
        private SpriteFont spriteFont;
        private List<MenuItem> columns, rows;
        private int screenWidth, screenHeight;
        GraphicsDevice device;
        public int optionSelected;
        public List<int> rowSelected;
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
            columns = new List<MenuItem>();
            rows = new List<MenuItem>();
            rowSelected = new List<int>();
            device = Game1.main.graphics.GraphicsDevice;
            spriteFont = Game1.main.content.Load<SpriteFont>(Game1.main.DataPath + @"Fonts\Courier New");
            optionSelected = 0;
            titleText = _tt;
            buttonAText = _t1;
            buttonBText = _t2;
            exitSequence = false;
            scale = Game1.menuScale;
            
        }


        public void AddColumn(int _type, String _text, int _action)
        {
            MenuItem mi = new MenuItem();
            mi.type = _type;
            mi.text = _text;
            mi.scale = 1.0f;
            mi.action = _action;
            mi.colBox = new Vector4();
            columns.Add(mi);
        }


        public void AddRow(int _type, String _text, int _action)
        {
            MenuItem mi = new MenuItem();
            mi.type = _type;
            mi.text = _text;
            mi.scale = 1.0f;
            mi.action = _action;
            mi.colBox = new Vector4();
            rows.Add(mi);
            rowSelected.Add(_type);
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

            if (Input.WasPressed(-1, Input.DOWN))
            {
                if (optionSelected < rows.Count - 1)
                {
                    optionSelected++;
                }
            }
            if (Input.WasPressed(-1, Input.UP))
            {
                if (optionSelected > 0)
                {
                    optionSelected--;
                }
            }

            if (Input.WasPressed(-1, Input.LEFT))
            {
                if (rowSelected[optionSelected] > 0)
                {
                    rowSelected[optionSelected]--;
                }
                
            }
            if (Input.WasPressed(-1, Input.RIGHT))
            {
                if (rowSelected[optionSelected] < columns.Count - 1)
                {
                    rowSelected[optionSelected]++;
                }

            }


            if (Input.WasPressed(-1, Input.BACK | Input.HIT2))
            {
                res = MENU_ACTION_BACK;
                //startState.DoAction(1);
                //return null;
            }
            if (Input.WasPressed(-1, Input.HIT | Input.START))
            {
                res = rows[optionSelected].action;
                //startState.DoAction(2);
                //return new GameStates.War(m_Game);
            }

            // Mouse Handling
            bool correctPress = false; 

            for (int i = 0; i < rows.Count; i++)
            {
                //for (int j = 0; j < columns.Count; j++)
                {

                    Vector4 colBoxRow = rows[i].colBox;
                    if (Input.mouseY >= colBoxRow.Y && Input.mouseY < colBoxRow.Y + colBoxRow.W)
                        //&& Input.mouseX >= colBox.X && Input.mouseX < colBox.X + colBox.Z)
                    {
                        
                        optionSelected = i;
                        correctPress = true;

                        if (Input.mouseLeftRamp)
                        {
                            SoundManager.SoundPlay(SoundManager.MUSIC_OPTION);
                            rowSelected[i] = (1 + rowSelected[i]) % (columns.Count);
                        }
                    }
                }
            }
            if (!correctPress && Input.mouseLeftRamp)
            {
                SoundManager.SoundPlay(SoundManager.MUSIC_ACCEPT);
                res = rows[optionSelected].action;
            }
            if (Input.mouseRightRamp)
            {
                SoundManager.SoundPlay(SoundManager.MUSIC_ACCEPT);
                res = MENU_ACTION_BACK;
            }
            // End Mouse Handling


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


            float posX = screenWidth / 4;

            float posY = screenHeight / 2;

            colorin = new Color(new Vector4(1, 1, 1, 1));
            colorout = new Color(new Vector4(0, 0, 0, 1));
            float itemScale = rows[0].scale * 0.8f * Game1.menuScale;



//            Util.DrawSprite(device, "Menus/labelboxalpha", new Vector2(posX, posY + (Game1.menuScale * optionSelected * 45 * 0.8f)), new Vector2(screenWidth - (screenWidth / 8), 45 * scale * 0.8f), SpriteBlendMode.AlphaBlend, new Color(128, 128, 128), Util.SPRITE_ALIGN_TOP_LEFT);

            

            for (int i = -1; i < rows.Count; i++)
            {
                for (int j = -1; j < columns.Count; j++)
                {
                    bool drawSquare = false;
                    String text = null;

                    if (i >= 0)
                    {
                        if (j >= 0)
                        {
                            if (rowSelected[i] == j)
                            {
                                drawSquare = true;

                            }
                        }
                        else
                        {
                            // Row titles
                            text = rows[i].text;
                        }
                    }
                    else
                    {
                        // First Row
                        if (j >= 0)
                        {
                            // Column Titles
                            text = columns[j].text;

                            
                        }

                    }

                    if (text != null)
                    {
                        Vector2 pos = new Vector2(posX + ((screenWidth / 6) *j) , posY + (45 * itemScale * i));
                        Vector2 FontOrigin = new Vector2(0, 0);// spriteFont.MeasureString(text) / 2;
                
                        Util.DrawText(spriteFont, text, pos, colorin, colorout, FontOrigin, itemScale, SpriteEffects.None);
                    }
                    if (drawSquare)
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

                        Vector2 pos = new Vector2(posX + ((screenWidth / 6) * j), posY + (45 * itemScale * i));
                        Vector2 size = new Vector2((screenWidth / 6), (45 * itemScale));
                        //Vector2 FontOrigin = new Vector2(0, 0);// spriteFont.MeasureString(text) / 2;
                        Color c = new Color(new Vector4(sideColors[i][0],sideColors[i][1], sideColors[i][2], sideColors[i][3]));
                        SpriteBlendMode mode = SpriteBlendMode.Additive;
                        if (optionSelected == i)
                        {
                            mode = SpriteBlendMode.None;
                            coloroption = new Color(200, 200, 220);
                        }
                        else
                        {
                            coloroption = new Color(128,128,128);
                        }
                        Util.DrawSprite(device, "Menus/labelboxalpha", new Vector2(posX, posY + (Game1.menuScale * i * 45 * 0.8f)), new Vector2(size.X*4, 45 * scale * 0.8f), SpriteBlendMode.AlphaBlend, coloroption, Util.SPRITE_ALIGN_TOP_LEFT);

                        Util.DrawSprite(device, "Menus/labelboxwhite", pos, size, mode, c, Util.SPRITE_ALIGN_TOP_LEFT);
                        if (i == optionSelected)
                        {
                            if (rowSelected[optionSelected] > 0)
                            {
                                Util.DrawSprite(device, "Menus/left-arrow", pos - new Vector2(size.X / 4, 0), new Vector2(size.X / 4, size.Y), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_TOP_LEFT);
                            }
                            if (rowSelected[optionSelected] < columns.Count - 1)
                            {
                                Util.DrawSprite(device, "Menus/right-arrow", pos + new Vector2(size.X, 0), new Vector2(size.X / 4, size.Y), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_TOP_LEFT);
                            }
                        }

                        //if (i >= 0)
                        {
                            rows[i].colBox.Y = pos.Y;
                            rows[i].colBox.W = size.Y;
                        }

                    }
                }
                
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

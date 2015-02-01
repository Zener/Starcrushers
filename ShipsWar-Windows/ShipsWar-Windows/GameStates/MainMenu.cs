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
using Microsoft.Xna.Framework.Audio;
using ShipsWar.Menus;


namespace ShipsWar.GameStates
{
    class MainMenu: GameState
    {
        Texture2D bg;
        Menu menu;
        MenuBackgroundRender mbr;
        float logoScale;
        
        //float destLogoScale = 0.5f;

        public override bool IsFullScreen() { return true; }

        public MainMenu(Game1 g): base(g)
        {
            bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"Images\Menus\backspace");
            //bg = m_Game.content.Load<Texture2D>("backspace");
            //bg = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"images\MainMenu");
            menu = new Menu();
            menu.Init(0, 0, "Main Menu", "Accept", "Quit");
            menu.AddOption(0, "Start Game", Menu.MENU_ACTION_NEXT);
            menu.AddOption(0, "Tutorial", Menu.MENU_ACTION_NEXT);            
            menu.AddOption(0, "Options", 3);
            menu.AddOption(0, "Credits", 2);
            menu.AddOption(0, "Quit", Menu.MENU_ACTION_BACK);

            SoundManager.Update();

            SoundManager.SoundStop();
            
            SoundManager.SoundPlay(SoundManager.MUSIC_MAINMENU);
            //soundBank.PlayCue("31855_HardPCM_Chip015");


            logoScale = GameVars.destLogoScale * 10; ;
            mbr = new MenuBackgroundRender();
        }


        /// <summary>
        /// Allows the game state to run logic.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The next gamestate to transition to, this to stay, null to pop back to previous state. </returns>
        public override GameState Update(GameTime gameTime)
        {
            GraphicsDevice gdev = m_Game.graphics.GraphicsDevice;

            if (logoScale == GameVars.destLogoScale)
            {
                mbr.Update(gdev, gameTime);
            }
            
            
            if (logoScale > GameVars.destLogoScale)
            {
                logoScale -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.008f;
            }
            if (logoScale <= GameVars.destLogoScale)
            {
                logoScale = GameVars.destLogoScale;
                return DoAction(menu.Update(gameTime));
            }
            if (Input.WasPressed(-1, Input.HIT | Input.START))
            {
                logoScale = GameVars.destLogoScale;
             
            }
            return this;
            
        }


        public GameState DoAction(int action)
        {
            switch (action)
            {
                case 0:
                    if (menu.menuExited)
                    {
                        return null;
                    }
                    menu.exitSequence = true;
                    break;

                case Menu.MENU_ACTION_NEXT:
                    if (menu.menuExited)
                    {
                        if (menu.optionSelected == 0)
                        {
                            GameVars.tutorialMode = false;
                            return new GameStates.SelectUniverseSize(m_Game);
                        }
                        else
                        {
                            GameVars.tutorialMode = true;
                            List<int> tutPlayers = new List<int>();
                            tutPlayers.Add(0);
                            tutPlayers.Add(4);
                            return new GameStates.War(m_Game, 2, tutPlayers, 0);
                        }
                        
                    }
                    menu.exitSequence = true;
                    break;

                case 2:
                    if (menu.menuExited)
                    {
                        return new GameStates.Credits(m_Game);
                    }
                    menu.exitSequence = true;
                    break;

                case 3:
                    if (menu.menuExited)
                    {
                        return new GameStates.Options(m_Game);
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
            
            GraphicsDevice gdev = m_Game.graphics.GraphicsDevice;
            /*Texture2D */
            float imageWidth = Util.GetImageWidth(@"Menus\backspace");
            float imageHeight = Util.GetImageHeight(@"Menus\backspace");
            float ratioW = gdev.PresentationParameters.BackBufferWidth / imageWidth;
            float ratioH = gdev.PresentationParameters.BackBufferHeight / imageHeight;
            float ratio;
            if (ratioW < ratioH)
            {
                ratio = ratioH;
            }
            else
            {
                ratio = ratioW;
            }

            Util.DrawSprite(gdev, @"Menus\backspace", 
                new Vector2(gdev.PresentationParameters.BackBufferWidth / 2, gdev.PresentationParameters.BackBufferHeight / 2),
                new Vector2(imageWidth*ratio, imageHeight*ratio), SpriteBlendMode.None, new Color(new Vector4(1.0f, 1.0f, 1.0f, 0.5f) * (1.0f / (logoScale * logoScale))), Util.SPRITE_ALIGN_CENTERED);

            mbr.Render();
            

             
            // Logo
            int logoWidth = Util.GetImageWidth("Menus\\logo");
            int logoHeight = Util.GetImageHeight("Menus\\logo");
            float logoW = logoWidth * menu.scale * logoScale;
            float logoH = logoHeight * menu.scale * logoScale;
            
            SpriteBlendMode sbm = SpriteBlendMode.Additive;
            if (logoScale == GameVars.destLogoScale)
            {
                sbm = SpriteBlendMode.AlphaBlend;
            }
            Util.DrawSprite(gdev, "Menus\\logo", new Vector2(gdev.PresentationParameters.BackBufferWidth / 2, gdev.PresentationParameters.BackBufferHeight/6), new Vector2(logoW, logoH), sbm, Color.White, Util.SPRITE_ALIGN_CENTERED);

            if (logoScale == GameVars.destLogoScale)
            {
                menu.Draw();
            }
        }
    }
}

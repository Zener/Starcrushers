///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////


#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;
//using ZFramework;
#endregion



namespace ShipsWar
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public ContentManager content;
        GraphicsDevice device;
        public string DataPath = @"Content\res\";
        List<GameState> States = new List<GameState>();
        public SpriteBatch batch;
        public static Game1 main;
        public static float menuScale;
        public static float gameScale;
        

        public Game1()
        {
            main = this;
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
            menuScale = 1.0f;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            States.Add(new GameStates.MainMenu(this));

            base.Initialize();
        }


        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                SetUpXNADevice(false);
            }
           
        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent == true)
            {
                content.Unload();
            }
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
            // Allows the default game to exit on Xbox 360 and Windows
            
            if (States.Count == 0)
            {
                this.Exit();
                return;
            }

            GameState curSt = States[States.Count - 1];
            GameState newSt = curSt.Update(gameTime);
            if (newSt == null)
            {
                States.RemoveAt(States.Count - 1);
            }
            else if (newSt != curSt)
            {
                if (curSt.Finished)
                {
                    States.RemoveAt(States.Count - 1);
                }
                States.Add(newSt);
            }

            base.Update(gameTime);
        }


        
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

           int i = States.Count - 1;
            for (; i > 0 && !States[i].IsFullScreen(); --i)
                ;
            if (i >= 0)
            {
                for (; i < States.Count; ++i)
                {
                    States[i].Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }


        public void DrawBackgroundImage()
        {
            DrawBackgroundImage(@"Menus\backspace");
        }


        public void DrawBackgroundImage(String img)
        {
            GraphicsDevice gdev = graphics.GraphicsDevice;
            /*Texture2D */
            float imageWidth = Util.GetImageWidth(img);
            float imageHeight = Util.GetImageHeight(img);
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

            Util.DrawSprite(gdev, img,
                new Vector2(gdev.PresentationParameters.BackBufferWidth / 2, gdev.PresentationParameters.BackBufferHeight / 2),
                new Vector2(imageWidth * ratio, imageHeight * ratio), SpriteBlendMode.None, Color.White
                , Util.SPRITE_ALIGN_CENTERED);

        }

        

        public void SetUpXNADevice(bool supportWideScreen)
        {
            bool fullscreen = GameVars.fullScreenMode;
           
            batch = new SpriteBatch(graphics.GraphicsDevice);
         
            device = graphics.GraphicsDevice;

            device.RenderState.CullMode = CullMode.CullClockwiseFace;           
            
            #if !XBOX
            if (!supportWideScreen || !InitGraphicsMode(1440, 900, fullscreen))
            #endif
            if (!supportWideScreen || !InitGraphicsMode(1280, 720, fullscreen))
            {
                if (!InitGraphicsMode(1024, 768, fullscreen))
                {
                    if (!InitGraphicsMode(800, 600, fullscreen))
                    {
                        if (!InitGraphicsMode(640, 480, fullscreen))
                        {
                            this.Exit();
                        }
                    }
                }
            }
            
            menuScale = device.PresentationParameters.BackBufferHeight / 720.0f;
            gameScale = (device.PresentationParameters.BackBufferWidth / 1280.0f)*0.95f;


            //graphics.ApplyChanges();
            Window.Title = "Starcrushers";
        }



        
private bool InitGraphicsMode(int iWidth, int iHeight, bool bFullScreen)
{
  // If we aren't using a full screen mode, the height and width of the window can
  // be set to anything equal to or smaller than the actual screen size.
  if (bFullScreen == false)
  {
    if ((iWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
    && (iHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
    {
      graphics.PreferredBackBufferWidth = iWidth;
      graphics.PreferredBackBufferHeight = iHeight;
      graphics.IsFullScreen = bFullScreen;
      graphics.ApplyChanges();
      return true;
    }
  }
  else
  {
    // If we are using full screen mode, we should check to make sure that the display
    // adapter can handle the video mode we are trying to set. To do this, we will
    // iterate thorugh the display modes supported by the adapter and check them against
    // the mode we want to set.
    foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
    {   
        
      // Check the width and height of each mode against the passed values
      if ((dm.Width == iWidth) && (dm.Height == iHeight))
      {
        // The mode is supported, so set the buffer formats, apply changes and return
        graphics.PreferredBackBufferWidth = iWidth;
        graphics.PreferredBackBufferHeight = iHeight;
        graphics.IsFullScreen = bFullScreen;
        graphics.ApplyChanges();
        return true;
      }
    }
  }
  return false;
}

    }
}
///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;



namespace ShipsWar
{
    class Util
    {
        #region Constants

        public  const int SPRITE_ALIGN_TOP_LEFT = 0;
        public  const int SPRITE_ALIGN_CENTERED = 1;

        #endregion


        public static int GetImageWidth(String img)
        {
            int w, h;
            Texture2D image = Game1.main.content.Load<Texture2D>(Game1.main.DataPath + @"images\" + img);
            w = image.Width;
            return w;
        }

        public static int GetImageHeight(String img)
        {
            int w, h;
            Texture2D image = Game1.main.content.Load<Texture2D>(Game1.main.DataPath + @"Images\" + img);
            h = image.Height;
            return h;
        }


        public static void DrawSprite(GraphicsDevice device, String img, Vector2 pos, Vector2 size, SpriteBlendMode blend, Color col, int align)
        {
            int w = 0, h = 0;
            Texture2D image = Game1.main.content.Load<Texture2D>(Game1.main.DataPath + @"Images\" + img);
            if (size != null)
            {
                w = (int)size.X;
                h = (int)size.Y;
            }
            if (size == null || w == -1)
            {
                w = image.Width;
            }

            if (size == null || h == -1)
            {
                h = image.Height;
            }

            if (align == SPRITE_ALIGN_CENTERED)
            {
                pos.X -= (w / 2);
                pos.Y -= (h / 2);
            }


            Game1.main.batch.Begin(blend);
            device.RenderState.DepthBufferWriteEnable = false;
            Game1.main.batch.Draw(image,
                                    new Rectangle((int)pos.X, (int)pos.Y, w, h),
                                    col);

            Game1.main.batch.End();
        }




        public static void DrawSprite(GraphicsDevice device, String img, Vector2 pos, Vector2 size, SpriteBlendMode blend, Color col, int align, float rotation)
        {
            int w = 0, h = 0;
            Texture2D image = Game1.main.content.Load<Texture2D>(Game1.main.DataPath + @"Images\" + img);
            if (size != null)
            {
                w = (int)size.X;
                h = (int)size.Y;
            }
            if (size == null || w == -1)
            {
                w = image.Width;
            }

            if (size == null || h == -1)
            {
                h = image.Height;
            }

            if (align == SPRITE_ALIGN_CENTERED)
            {
                pos.X -= (w / 2);
                pos.Y -= (h / 2);
            }


            Game1.main.batch.Begin(blend);
            device.RenderState.DepthBufferWriteEnable = false;
            /*Game1.main.batch.Draw(image,
                                    new Rectangle((int)pos.X, (int)pos.Y, w, h),
                                    col);*/
            Game1.main.batch.Draw(image, new Rectangle((int)pos.X + (w/2), (int)pos.Y + (h/2), w, h), new Rectangle(0, 0, w, h), col, rotation, new Vector2(w/2, h/2), SpriteEffects.None,0);

            Game1.main.batch.End();
        }



        public static Vector2 TransformTo2D(Vector3 res, Matrix view, Matrix projection)
        {
            GraphicsDevice device = Game1.main.graphics.GraphicsDevice;

            Matrix matrix = view;
            res = Vector3.Transform(res, matrix);

            float x = res.X;
            float y = res.Y;
            float z = -res.Z;

            //Matrix pp = Matrix.Invert(projection);

            int px = (int)((device.PresentationParameters.BackBufferWidth / 2.0) + device.PresentationParameters.BackBufferWidth * x * projection.M11 / (z * 2));
            int py = (int)((device.PresentationParameters.BackBufferHeight / 2.0) - device.PresentationParameters.BackBufferHeight * y * projection.M22 / (z * 2)); //inversed: y should be "up", while in screen coordinates it points down

            return new Vector2(px, py);
        }

        public static void DrawText(SpriteFont spriteFont, String text, Vector2 pos, Color colorin, Color colorout, Vector2 fontOrigin, float scale, SpriteEffects se)
        {
            SpriteBatch spriteBatch = Game1.main.batch;
            spriteBatch.Begin();
            
            spriteBatch.DrawString(spriteFont, text, pos - (new Vector2(0, 2) * scale), colorout, 0.0f, fontOrigin, scale, se, 0);
            spriteBatch.DrawString(spriteFont, text, pos + (new Vector2(0, 2) * scale), colorout, 0.0f, fontOrigin, scale, se, 0);
            spriteBatch.DrawString(spriteFont, text, pos - (new Vector2(2, 0) * scale), colorout, 0.0f, fontOrigin, scale, se, 0);
            spriteBatch.DrawString(spriteFont, text, pos + (new Vector2(2, 0) * scale), colorout, 0.0f, fontOrigin, scale, se, 0);
            
            spriteBatch.DrawString(spriteFont, text, pos, colorin, 0.0f, fontOrigin, scale, se, 0);
            
            spriteBatch.End();    
            
        }
    }
}

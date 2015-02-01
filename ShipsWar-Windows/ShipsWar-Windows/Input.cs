///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShipsWar
{
    static class Input
    {
        static public readonly int NUM_CONTROLLERS = 4;
        static public readonly int LEFT  = 1;
        static public readonly int RIGHT = 2;
        static public readonly int UP = 4;
        static public readonly int DOWN = 8;
        static public readonly int HIT = 16;
        static public readonly int BACK = 32;
        static public readonly int START = 64;
        static public readonly int HIT2 = 128;
         public const float epsilon = 0.2f;
        
        static int[] ramp = new int[NUM_CONTROLLERS];
        static int[] buttons = new int[NUM_CONTROLLERS];
        static TimeSpan[] vibrateTime = new TimeSpan[NUM_CONTROLLERS];

        #if !XBOX 
        static public MouseState mPreviousMouseState;
        #endif
        static public bool mouseLeftRamp;
        static public bool mouseRightRamp;
        static public int mouseX, mouseY;
        static public bool mouseLeftPressed;
        static public bool mouseRightPressed;
        


        static public void Update(GameTime gameTime)
        {
            for (int i = 0; i < NUM_CONTROLLERS; i++)
            {
                if (vibrateTime[i].Milliseconds > 0)
                {
                    vibrateTime[i] -= gameTime.ElapsedGameTime;
                    
                    if (vibrateTime[i].Milliseconds <= 0)
                    {
                        StopVibra(i);
                    }                    
                }
            }
            #if !XBOX
            mouseLeftRamp = false;
            mouseRightRamp = false;
            #endif

            int c = 0;
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed
                || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.2f
                || Keyboard.GetState().IsKeyDown(Keys.Enter)
                )
                c |= HIT;

            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed
                || GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.2f
                || Keyboard.GetState().IsKeyDown(Keys.RightControl))
                c |= HIT2;
            
            if (/*GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed
                ||*/ 
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                c |= BACK;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                c |= START;
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Left))
                c |= LEFT;
            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Right))
                c |= RIGHT;
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Up))
                c |= UP;
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Down))
                c |= DOWN;

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -epsilon)
            {
                c |= LEFT;
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > epsilon)
            {
                c |= RIGHT;
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -epsilon)
            {
                c |= DOWN;
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > epsilon)
            {
                c |= UP;
            }
            ramp[0] = c ^ buttons[0];
            buttons[0] = c;


            c = 0;
            if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed

                || Keyboard.GetState().IsKeyDown(Keys.LeftAlt)
                )
                c |= HIT;
            if (GamePad.GetState(PlayerIndex.Two).Buttons.B == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                c |= HIT2;
            
            if (/*GamePad.GetState(PlayerIndex.Two).Buttons.B == ButtonState.Pressed
                ||*/ GamePad.GetState(PlayerIndex.Two).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                c |= BACK;
            if (GamePad.GetState(PlayerIndex.Two).Buttons.Start == ButtonState.Pressed)
                c |= START;
            if (GamePad.GetState(PlayerIndex.Two).DPad.Left == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.A))
                c |= LEFT;
            if (GamePad.GetState(PlayerIndex.Two).DPad.Right == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.D))
                c |= RIGHT;
            if (GamePad.GetState(PlayerIndex.Two).DPad.Up == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.W))
                c |= UP;
            if (GamePad.GetState(PlayerIndex.Two).DPad.Down == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.S))
                c |= DOWN;
            ramp[1] = c ^ buttons[1];
            buttons[1] = c;


            c = 0;
            if (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed)
                c |= HIT;
            if (GamePad.GetState(PlayerIndex.Three).Buttons.B == ButtonState.Pressed)
                c |= HIT2;
            if (GamePad.GetState(PlayerIndex.Three).Buttons.Back == ButtonState.Pressed)
                c |= BACK;
            if (GamePad.GetState(PlayerIndex.Three).Buttons.Start == ButtonState.Pressed)
                c |= START;
            if (GamePad.GetState(PlayerIndex.Three).DPad.Left == ButtonState.Pressed)
                c |= LEFT;
            if (GamePad.GetState(PlayerIndex.Three).DPad.Right == ButtonState.Pressed)
                c |= RIGHT;
            if (GamePad.GetState(PlayerIndex.Three).DPad.Up == ButtonState.Pressed)
                c |= UP;
            if (GamePad.GetState(PlayerIndex.Three).DPad.Down == ButtonState.Pressed)
                c |= DOWN;
            ramp[2] = c ^ buttons[2];
            buttons[2] = c;

            c = 0;
            if (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed)
                c |= HIT;
            if (GamePad.GetState(PlayerIndex.Four).Buttons.B == ButtonState.Pressed)
                c |= HIT2;
            if (GamePad.GetState(PlayerIndex.Four).Buttons.Back == ButtonState.Pressed)
                c |= BACK;
            if (GamePad.GetState(PlayerIndex.Four).Buttons.Start == ButtonState.Pressed)
                c |= START;
            if (GamePad.GetState(PlayerIndex.Four).DPad.Left == ButtonState.Pressed)
                c |= LEFT;
            if (GamePad.GetState(PlayerIndex.Four).DPad.Right == ButtonState.Pressed)
                c |= RIGHT;
            if (GamePad.GetState(PlayerIndex.Four).DPad.Up == ButtonState.Pressed)
                c |= UP;
            if (GamePad.GetState(PlayerIndex.Four).DPad.Down == ButtonState.Pressed)
                c |= DOWN;
            ramp[3] = c ^ buttons[3];
            buttons[3] = c;

            #if !XBOX
            //Get the current state of the Mouse
            MouseState aMouse = Mouse.GetState();



            mouseLeftPressed = (aMouse.LeftButton == ButtonState.Pressed);
            mouseRightPressed = (aMouse.RightButton == ButtonState.Pressed);
           
            
            //If the user has just clicked the Left mouse button, then set the start location for the Selection box

            if (aMouse.LeftButton == ButtonState.Pressed && mPreviousMouseState.LeftButton == ButtonState.Released)
            {

                //Set the starting location for the selection box to the current location

                //where the Left button was initially clicked.

                //mSelectionBox = new Rectangle(aMouse.X, aMouse.Y, 0, 0);
                mouseLeftRamp = true;
                
            }

            if (aMouse.RightButton == ButtonState.Pressed && mPreviousMouseState.RightButton == ButtonState.Released)
            {
                mouseRightRamp = true;
            }

            mouseX = aMouse.X;
            mouseY = aMouse.Y;

            //If the user has released the left mouse button, then reset the selection square
            /*
            if (aMouse.LeftButton == ButtonState.Released)
            {

                //Reset the selection square to no position with no height and width

                //mSelectionBox = new Rectangle(-1, -1, 0, 0);

            }*/



            //Store the previous mouse state

            mPreviousMouseState = aMouse;
            #endif
        }

        static public void Vibrate(int _player, float _left, float _right, int milis)
        {

            PlayerIndex []pi = {PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four};

            GamePad.SetVibration(pi[_player], _left, _right);

            vibrateTime[_player] = new TimeSpan(0,0,0,0,milis);
        }

        static public void StopVibra(int _player)
        {
            PlayerIndex[] pi = { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };

            GamePad.SetVibration(pi[_player], 0, 0);
        }


        // Ramp-based checks
        static public bool WasPressed(int _nPlayer, int b)
        {

            if (_nPlayer <= 0)
            {
                return ((ramp[0] & buttons[0]) & b) != 0;
            }
            else
            {
                return ((ramp[_nPlayer] & buttons[_nPlayer]) & b) != 0;
            }
        }


        static public bool WasReleased(int _nPlayer, int b)
        {
            if (_nPlayer <= 0)
            {
                return ((ramp[0] & ~buttons[0]) & b) != 0;
            }
            else
            {
                return ((ramp[_nPlayer] & ~buttons[_nPlayer]) & b) != 0;
            }
        }


        // State-based checks
        static public bool IsPressed(int _nPlayer, int b)
        {
            if (_nPlayer <= 0)
            {
                return (buttons[0] & b) != 0;
            }
            else
            {
                return (buttons[_nPlayer] & b) != 0;
            }
        }


        static public bool IsReleased(int _nPlayer, int b)
        {
            if (_nPlayer <= 0)
            {
                return (~buttons[0] & b) != 0;
            }
            else
            {
                return (~buttons[_nPlayer] & b) != 0;
            }
        }
    }
}

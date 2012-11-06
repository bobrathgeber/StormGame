using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace StormGame
{
    class PlayerControllerState
    {
        IDictionary<Keys, TimeSpan> lastDown = new Dictionary<Keys, TimeSpan>();
        IDictionary<Keys, TimeSpan> lastUp = new Dictionary<Keys, TimeSpan>();
        TimeSpan doubleTapThresh =  new TimeSpan(0, 0, 0, 0, 200);
        PlayerControllerState oldState;
        KeyboardState ks;

        public bool IsDoubleTap(Keys key)
        {
            if(ks.IsKeyDown(key))
            {
                // this may be a double tap, calculate
                // - assume that the last key up MUST follow a key down. If the player presses the key after the lifting
                // - in less time than the threshold it must be a double tap.
                if (Globals.GameTime.TotalGameTime -oldState.lastUp[key] <= doubleTapThresh)
                    return true;
            }
            return false;            
        }

        public PlayerControllerState Update()
        {
            // ALWAYS CHECK DOUBLE TAP FIRST
            ks = Keyboard.GetState();

            foreach(Keys k in Enum.GetValues(typeof(Keys)))
            {
                if(ks.IsKeyDown(k))
                {
                    lastDown[k] = Globals.GameTime.TotalGameTime;
                }
                if(ks.IsKeyUp(k))
                {
                    lastUp[k] = Globals.GameTime.TotalGameTime;
                }
               
            }
            oldState = this;
            return this;
        }
    }
}
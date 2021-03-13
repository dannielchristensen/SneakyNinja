using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SneakyNinja.Screens
{
    public class InputState
    {
        public KeyboardState CurrentKeyboardState;
        public KeyboardState _lastKeyboardState;

        public InputState()
        {
            CurrentKeyboardState = new KeyboardState();

            _lastKeyboardState = new KeyboardState();

        }

        public void Update()
        {
            _lastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }
        public bool IsKeyPressed(Keys key) { 


                return CurrentKeyboardState.IsKeyDown(key);

        }
        public bool IsNewKeyPress(Keys key)
        {


                return (CurrentKeyboardState.IsKeyDown(key) &&
                        _lastKeyboardState.IsKeyUp(key));

        }
    }
}

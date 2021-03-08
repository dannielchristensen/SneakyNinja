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
        private KeyboardState _lastKeyboardState;

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

    }
}

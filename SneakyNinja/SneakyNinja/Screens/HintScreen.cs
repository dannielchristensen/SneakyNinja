using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace SneakyNinja.Screens
{
    public class HintScreen : GameScreen
    {
        private SneakyNinjas game;
        public HintScreen(SneakyNinjas game)
        {
            this.game = game;
        }
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input.CurrentKeyboardState.IsKeyDown(Keys.Enter) && input._lastKeyboardState.IsKeyUp(Keys.Enter))
            {
                MainMenuScreen.Load(ScreenManager, this.game);

            }
        }
        public static void Load(ScreenManager screenManager, SneakyNinjas game)
        {
            foreach (var screen in screenManager.GetScreens())
                screen.ExitScreen();
            var h = new HintScreen(game);
            screenManager.AddScreen(h);

        }
        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            spriteBatch.DrawString(ScreenManager.Font, $"The eyes are all seeing and can see through walls.", new Vector2(100, 100), Color.Black);
            spriteBatch.DrawString(ScreenManager.Font, $"WASD to move.", new Vector2(100, 160), Color.Black);
            spriteBatch.DrawString(ScreenManager.Font, $"Find the Scroll and return to the portal.", new Vector2(100, 190), Color.Black);
            spriteBatch.DrawString(ScreenManager.Font, $"Be careful. The scroll may be pressure plated.", new Vector2(100, 220), Color.Black);
            spriteBatch.DrawString(ScreenManager.Font, $"Press enter to return to the main menu.", new Vector2(100, 250), Color.Black);



            spriteBatch.End();

        }
    }
}

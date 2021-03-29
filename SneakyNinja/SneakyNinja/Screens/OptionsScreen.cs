using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using Microsoft.Xna.Framework;


namespace SneakyNinja.Screens
{


    public class OptionsScreen : MenuScreen
    {
        private MenuEntry VolumeUp;
        private MenuEntry VolumeDown;
        private MenuEntry BackToMainMenu;
        private SneakyNinjas game;
        public float Volume = 100;
        public OptionsScreen(SneakyNinjas game) : base("Options", game)
        {
            this.game = game;
            VolumeUp = new MenuEntry("Increase Volume");
            VolumeDown = new MenuEntry("Decrease Volume");
            BackToMainMenu = new MenuEntry("Return to Main Menu");
            Volume = MediaPlayer.Volume;
            VolumeUp.Selected += IncreaseVolume;
            VolumeDown.Selected += DecreaseVolume;
            BackToMainMenu.Selected += ReturnToMainMenu;
            MenuEntries.Add(VolumeUp);
            MenuEntries.Add(VolumeDown);
            MenuEntries.Add(BackToMainMenu);


        }
        public static void Load(ScreenManager screenManager, SneakyNinjas game)
        {
            foreach (var screen in screenManager.GetScreens())
                screen.ExitScreen();
            var h = new OptionsScreen(game);
            screenManager.AddScreen(h);
        }
        public void ReturnToMainMenu(Object sender, EventArgs e)
        {
            MainMenuScreen.Load(ScreenManager, game);
        }
        public void DecreaseVolume(Object sender, EventArgs e)
        {
            Volume-=.05f;
            if(Volume < 0)
            {
                Volume = 0;
            }
            MediaPlayer.Volume = Volume;

        }
        public void IncreaseVolume(Object sender, EventArgs e)
        {
            Volume += .05f;
            if (Volume > 1)
            {
                Volume = 1;
            }
            MediaPlayer.Volume = Volume;

        }
        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            base.DrawNonSpriteBatch(gameTime);
            if(Volume == 0)
            {
                spriteBatch.DrawString(ScreenManager.Font, $"Current Volume: 0", new Vector2(25, 200), Color.Black);

            }
            else
                spriteBatch.DrawString(ScreenManager.Font, $"Current Volume: {Volume*100:###}", new Vector2(25, 200), Color.Black);
            spriteBatch.End();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace SneakyNinja.Screens
{
    public class EndScreen : GameScreen
    {
        private SneakyNinjas game;
        private Room room;
        private PlayerSprite player;
        private SpriteFont bangers;
        private ContentManager _content;


        private EndScreen(SneakyNinjas game, PlayerSprite player)
        {
            this.game = game;
            this.player = player;

        }
        public static void Load(ScreenManager screenManager, SneakyNinjas game, PlayerSprite player)
        {
            foreach (var screen in screenManager.GetScreens())
                screen.ExitScreen();
            var EndScreen = new EndScreen(game, player);
            screenManager.AddScreen(EndScreen);

        }
        public override void Activate()
        {
            base.Activate();
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            if (player == null)
                player = new PlayerSprite(game, new Vector2(0, 0), new Vector2(96, 96));
            player.LoadContent();
            bangers = game.Content.Load<SpriteFont>("bangers");

        }
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
            if (input.CurrentKeyboardState.IsKeyDown(Keys.S))
            {
                player = new PlayerSprite(game, new Vector2(0, 0), new Vector2(96, 96));
                ScreenManager.Rooms = new Room[2,2];
                TopLeftCornerRoomScreen.Load(ScreenManager, game, player);

            }
            else if (input.CurrentKeyboardState.IsKeyDown(Keys.Enter))
            {
                player = new PlayerSprite(game, new Vector2(0, 0), new Vector2(96, 96));

                TopLeftCornerRoomScreen.Load(ScreenManager, game, player);

            } else if (input.CurrentKeyboardState.IsKeyDown(Keys.M))
            {
                player.HasScroll = false;
                player.GameOver = false;
                player.PlayerWins = false;
                // load menu screen
                MainMenuScreen.Load(ScreenManager, game);

            }
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            if (player.PlayerWins)
            {
                spriteBatch.DrawString(bangers, $"YOU WIN. YOU GOT THE SCROLL WITHOUT GETTING CAUGHT.", new Vector2(25, 5), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS ENTER TO CONTINUE WITH THE SAME MAP.", new Vector2(25, 35), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS S TO SPAWN A NEW MAP.", new Vector2(25, 65), Color.Black);
                spriteBatch.DrawString(bangers, $"RUN TIME: {player.TotalTime:######.##}.", new Vector2(25, 95), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS M TO GO TO THE MENU.", new Vector2(25, 125), Color.Black);

            }
            else
            {
                spriteBatch.DrawString(bangers, $"YOU WERE CAUGHT. GAME OVER.", new Vector2(25, 5), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS ENTER TO CONTINUE WITH THE SAME MAP.", new Vector2(25, 35), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS S TO SPAWN A NEW MAP.", new Vector2(25, 65), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS M TO GO TO THE MENU.", new Vector2(25, 95), Color.Black);

            }
            spriteBatch.End();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;



namespace SneakyNinja.Screens
{
    public class TopRightCornerRoomScreen:GameScreen
    {
        private ContentManager _content;
        private EyeSprite eye;
        private SneakyNinjas game;
        private Room room;
        private Texture2D wallTexture;
        private PlayerSprite player;
        public Room Room => room;
        public EyeSprite Eye => eye;

        private TopRightCornerRoomScreen(SneakyNinjas game, PlayerSprite player)
        {
            this.game = game;
            this.player = player;



        }
        public override void Activate()
        {
            base.Activate();
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            room = ScreenManager.Rooms[(int)player.Coord.X, (int)player.Coord.Y];
            if (room == null)
            {
                room = new Room(game, RoomType.TopRight, ScreenManager);
                room.LoadContent();
                ScreenManager.Rooms[(int)player.Coord.X, (int)player.Coord.Y] = room;

            }
            eye = new EyeSprite(this.room, game);
            eye.LoadContent();
            wallTexture = game.Content.Load<Texture2D>("dungeon_wall_32_r");
        }
        public static void Load(ScreenManager screenManager, SneakyNinjas game, PlayerSprite player)
        {
            foreach (var screen in screenManager.GetScreens())
                screen.ExitScreen();
            var StandardCornerRoomScreen = new TopRightCornerRoomScreen(game, player);
            screenManager.AddScreen(StandardCornerRoomScreen);

        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            player.Update(gameTime, room.Walls, null);
            eye.Update(gameTime);
            if (player.Bounds.CollidesWith(room.door_y))
            {
                player.Coord.X = 1;
                player.Coord.Y = 1;
                player.Position = new Vector2(player.Position.X, 64);

                BottomRightCornerRoomScreen.Load(ScreenManager, game, player);
            }
            else if (player.Bounds.CollidesWith(room.door_x))
            {
                player.Coord.Y = 0;
                player.Coord.X = 0;
                player.Position = new Vector2(game.GraphicsDevice.Viewport.Width - 96, player.Position.Y);
                TopLeftCornerRoomScreen.Load(ScreenManager, game, player);
            }

            if (!player.Detected && Eye.Vision.CollidesWith(player.Bounds))
            {

                player.Detected = true;
            }
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (player.Detected)
            {
                player.DetectedTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (player.DetectedTimer <= 0)
                {
                    player.GameOver = true;
                }
            }
            if (player.GameOver)
            {
                ExitScreen();
            }
        }
        public override void Draw(GameTime gameTime)
        {

            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            player.Draw(spriteBatch);

            room.Draw(spriteBatch);
            eye.Draw(spriteBatch);
            DrawTopRightCornerRoom(spriteBatch);
            spriteBatch.End();
        }

        private void DrawTopRightCornerRoom(SpriteBatch sb)
        {
            int count = 0;
            for (int i = 32; i < game.GraphicsDevice.Viewport.Width; i += 32)
            {
                count++;
                sb.Draw(wallTexture, new Vector2(i, 32), Color.White);
                sb.Draw(wallTexture, new Vector2(game.GraphicsDevice.Viewport.Width - 32, i), Color.White);

                if (count < 11 || count > 13)
                    sb.Draw(wallTexture, new Vector2(i, game.GraphicsDevice.Viewport.Height - 32), Color.White);
                if (count < 7 || count > 9)
                    sb.Draw(wallTexture, new Vector2(0, i), Color.White);

            }
        }

        
    }
}


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;



namespace SneakyNinja.Screens
{
    public class BottomLeftCornerRoomScreen : GameScreen
    {
        private ContentManager _content;
        private EyeSprite eye;
        private SneakyNinjas game;
        private Room room;
        private Texture2D wallTexture;
        private PlayerSprite player;
        public Room Room => room;
        public EyeSprite Eye => eye;

        private BottomLeftCornerRoomScreen(SneakyNinjas game, PlayerSprite player)
        {
            this.game = game;
            this.player = player;



        }
        public override void Activate()
        {
            base.Activate();
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            room = new Room(game, RoomType.BottomLeft, ScreenManager);
            room.LoadContent();

            eye = new EyeSprite(this.room, game);
            eye.LoadContent();
            wallTexture = game.Content.Load<Texture2D>("dungeon_wall_32_r");
        }
        public static void Load(ScreenManager screenManager, SneakyNinjas game, PlayerSprite player)
        {
            var StandardCornerRoomScreen = new BottomLeftCornerRoomScreen(game, player);
            screenManager.AddScreen(StandardCornerRoomScreen);

        }
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            player.Update(gameTime, room.Walls, null);
            eye.Update(gameTime);
            if (player.Bounds.CollidesWith(room.door_x))
            {
                player.Coord.Y = 1;
                player.Position = new Vector2(64, player.Position.Y);

                BottomRightCornerRoomScreen.Load(ScreenManager, game, player);
            }
            else if (player.Bounds.CollidesWith(room.door_y))
            {
                player.Coord.X = 0;
                player.Position = new Vector2(player.Position.X, game.GraphicsDevice.Viewport.Height - 96);
                TopLeftCornerRoomScreen.Load(ScreenManager, game, player);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            player.Draw(spriteBatch);

            room.Draw(spriteBatch);
            eye.Draw(spriteBatch);
            DrawBottomLeftCornerRoom(spriteBatch);
            spriteBatch.End();
        }

        private void DrawBottomLeftCornerRoom(SpriteBatch sb)
        {
            int count = 0;
            for (int i = 32; i < game.GraphicsDevice.Viewport.Width; i += 32)
            {
                count++;
                sb.Draw(wallTexture, new Vector2(0, i), Color.White);
                sb.Draw(wallTexture, new Vector2(i, game.GraphicsDevice.Viewport.Height - 32), Color.White);
                if (count < 11 || count > 13)
                    sb.Draw(wallTexture, new Vector2(i, 32), Color.White);
                if (count < 7 || count > 9)
                    sb.Draw(wallTexture, new Vector2(game.GraphicsDevice.Viewport.Width - 32, i), Color.White);

            }
        }
    }
}

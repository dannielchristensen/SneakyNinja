using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace SneakyNinja.Screens
{
    public class BottomRightCornerRoomScreen : GameScreen
    {
        private ContentManager _content;
        private SneakyNinjas game;
        private Room room;
        private Texture2D wallTexture;
        private ScrollSprite scroll;
        private PlayerSprite player;

        public ScrollSprite Scroll
        {
            get
            {
                return scroll;
            }
            set
            {
                scroll = value;
            }
        }
        


        public Room Room => room;
        public BottomRightCornerRoomScreen(SneakyNinjas game, PlayerSprite player)
        {
            this.game = game;
            this.player = player;


        }
        public override void Activate()
        {
            base.Activate();
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            room = new Room(game, RoomType.BottomRight, ScreenManager);
            room.LoadContent();

            wallTexture = game.Content.Load<Texture2D>("dungeon_wall_32_r");
            scroll = new ScrollSprite(this.room, game);
            scroll.LoadContent();
        }
        public static void Load(ScreenManager screenManager, SneakyNinjas game, PlayerSprite player)
        {
            var StandardCornerRoomScreen = new BottomRightCornerRoomScreen(game, player);
            screenManager.AddScreen(StandardCornerRoomScreen);

        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            player.Update(gameTime, room.Walls, null);
            if (player.Bounds.CollidesWith(room.door_y))
            {
                player.Coord.X = 0;
                player.Position = new Vector2(player.Position.X, game.GraphicsDevice.Viewport.Height - 96);

                TopRightCornerRoomScreen.Load(ScreenManager, game, player);
            }
            else if (player.Bounds.CollidesWith(room.door_x))
            {
                player.Coord.Y = 0;
                player.Position = new Vector2(game.GraphicsDevice.Viewport.Width - 96, player.Position.Y);
                BottomLeftCornerRoomScreen.Load(ScreenManager, game, player);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            player.Draw(spriteBatch);

            room.Draw(spriteBatch);
            scroll.Draw(spriteBatch);
            DrawBottomRightCornerRoom(spriteBatch);
            spriteBatch.End();
        }
        private void DrawBottomRightCornerRoom(SpriteBatch sb)
        {
            int count = 0;
            for (int i = 32; i < game.GraphicsDevice.Viewport.Width; i += 32)
            {
                count++;
                sb.Draw(wallTexture, new Vector2(game.GraphicsDevice.Viewport.Width - 32, i), Color.White);
                sb.Draw(wallTexture, new Vector2(i, game.GraphicsDevice.Viewport.Height - 32), Color.White);

                if (count < 11 || count > 13)
                    sb.Draw(wallTexture, new Vector2(i, 32), Color.White);
                if (count < 7 || count > 9)
                    sb.Draw(wallTexture, new Vector2(0, i), Color.White);

            }
        }
    }
}

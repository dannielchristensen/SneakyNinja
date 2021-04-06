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
        private bool shaking;
        private float shakeTime;
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
            room = ScreenManager.Rooms[(int)player.Coord.X, (int)player.Coord.Y];
            if (room == null)
            {
                room = new Room(game, RoomType.BottomRight, ScreenManager);
                room.LoadContent();
                ScreenManager.Rooms[(int)player.Coord.X, (int)player.Coord.Y] = room;

            }

            wallTexture = game.Content.Load<Texture2D>("dungeon_wall_32_r");
            scroll = new ScrollSprite(this.room, game);
            scroll.LoadContent();
        }
        public static void Load(ScreenManager screenManager, SneakyNinjas game, PlayerSprite player)
        {
            foreach (var screen in screenManager.GetScreens())
                screen.ExitScreen();
            var StandardCornerRoomScreen = new BottomRightCornerRoomScreen(game, player);
            screenManager.AddScreen(StandardCornerRoomScreen);

        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            // make sure to check top door for bottom right room; player went right through door without switching rooms
            player.Update(gameTime, room.Walls, null);
            if (player.Bounds.CollidesWith(room.door_y))
            {
                player.Coord.X = 0;
                player.Coord.Y = 1;
                player.Position = new Vector2(player.Position.X, game.GraphicsDevice.Viewport.Height - 96);
                scroll.RemoveComponents();

                TopRightCornerRoomScreen.Load(ScreenManager, game, player);
            }
            else if (player.Bounds.CollidesWith(room.door_x))
            {
                player.Coord.Y = 0;
                player.Coord.X = 1;
                player.Position = new Vector2(game.GraphicsDevice.Viewport.Width - 96, player.Position.Y);
                //ExitScreen();
                scroll.RemoveComponents();

                BottomLeftCornerRoomScreen.Load(ScreenManager, game, player);
            }
            if (player.Bounds.CollidesWith(Scroll.Bounds))
            {
                player.HasScroll = true;
                shaking = true;

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
                scroll.RemoveComponents();
                EndScreen.Load(ScreenManager, game, player);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            Matrix shakeTransform = Matrix.Identity;
            if (shaking)
            {
                shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                shakeTransform = Matrix.CreateTranslation(10 * MathF.Sin(shakeTime), 10 * MathF.Cos(shakeTime), 0);
                if (shakeTime > 3000) shaking = false;
            }
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin(transformMatrix:shakeTransform);
            player.Draw(spriteBatch);

            room.Draw(spriteBatch);
            if(!player.HasScroll)
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

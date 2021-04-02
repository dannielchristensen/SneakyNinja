using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SneakyNinja.Particles;


using SneakyNinja.Collisions;

namespace SneakyNinja.Screens
{
    public class TopLeftCornerRoomScreen : GameScreen, IParticleEmitter
    {
        private ContentManager _content;
        private SneakyNinjas game;
        private Room room;
        private Texture2D wallTexture;
        private PlayerSprite player;
        public ExitSprite Exit;
        DustParticleSystem dust;
        public Vector2 Position { get; private set;  }
        public Vector2 Velocity { get; private set;  }
        

        public Room Room => room;
        public TopLeftCornerRoomScreen(SneakyNinjas game)
        {
            this.game = game;
        }
        private TopLeftCornerRoomScreen(SneakyNinjas game, PlayerSprite player)
        {
            this.game = game;
            this.player = player;

        }
        public static void Load(ScreenManager screenManager, SneakyNinjas game, PlayerSprite player)
        {
            foreach (var screen in screenManager.GetScreens())
                screen.ExitScreen();
            var StandardCornerRoomScreen = new TopLeftCornerRoomScreen(game, player);
            screenManager.AddScreen(StandardCornerRoomScreen);

        }
        public override void Activate()
        {
            base.Activate();
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            if (player == null)
                player = new PlayerSprite(game, new Vector2(0,0), new Vector2(96, 96));
            player.LoadContent();
            wallTexture = _content.Load<Texture2D>("dungeon_wall_32_r");
            room = ScreenManager.Rooms[(int)player.Coord.X, (int)player.Coord.Y];
            if (room == null)
            {
                room = new Room(game, RoomType.TopLeft, ScreenManager);
                room.LoadContent();
                ScreenManager.Rooms[(int)player.Coord.X, (int)player.Coord.Y] = room;

            }
            if (Exit == null)
            {
                Exit = new ExitSprite(game, new Vector2(96, 96));
                Exit.LoadContent();
                Vector2 ExitPos = new Vector2(96, 96);
            }
            dust = new DustParticleSystem(this.game, this);
            this.game.Components.Add(dust);


        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            player.Update(gameTime, room.Walls, null);
            if (player.Bounds.CollidesWith(room.door_y))
            {
                player.Coord.X = 1;
                player.Coord.Y = 0;
                
                player.Position = new Vector2(player.Position.X, 64);
                this.game.Components.Remove(dust);

                BottomLeftCornerRoomScreen.Load(ScreenManager, game, player);
            }
            else if (player.Bounds.CollidesWith(room.door_x))
            {
                player.Coord.Y = 1;
                player.Coord.X = 0;
                this.game.Components.Remove(dust);

                player.Position = new Vector2(64, player.Position.Y);
                TopRightCornerRoomScreen.Load(ScreenManager, game, player);
            }else if (player.HasScroll && Exit.Bounds.CollidesWith(player.Bounds))
            {
                player.PlayerWins = true;
                this.game.Components.Remove(dust);

                EndScreen.Load(ScreenManager, game, player);

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
                this.game.Components.Remove(dust);

                EndScreen.Load(ScreenManager, game, player);
            }
            Random r = new Random();
            this.Position = Exit.Position + new Vector2(16, 16);
            Velocity = new Vector2(r.Next(-5, 6), r.Next(-5, 6));
        }
        public override void  Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            room.Draw(spriteBatch);
            Exit.Draw(spriteBatch);
            DrawTopLeftCornerRoom(spriteBatch);
            spriteBatch.End();

        }
        private void DrawTopLeftCornerRoom(SpriteBatch sb)
        {
            int count = 0;
            for (int i = 32; i < game.GraphicsDevice.Viewport.Width; i += 32)
            {
                count++;
                sb.Draw(wallTexture, new Vector2(i, 32), Color.White);
                sb.Draw(wallTexture, new Vector2(0, i), Color.White);
                if (count < 11 || count > 13)
                    sb.Draw(wallTexture, new Vector2(i, game.GraphicsDevice.Viewport.Height - 32), Color.White);
                if (count < 7 || count > 9)
                    sb.Draw(wallTexture, new Vector2(game.GraphicsDevice.Viewport.Width - 32, i), Color.White);
            }
        }
    }
    public class ExitSprite
    {
        public BoundingCircle Bounds;
        public Vector2 Position;
        private Texture2D texture;
        private SneakyNinjas game;
        public ExitSprite(SneakyNinjas game, Vector2 pos)
        {
            this.game = game;
            Position = pos;
            Bounds = new BoundingCircle(pos + new Vector2(16, 16), 16);
        }
        public void LoadContent()
        {
            this.texture = game.Content.Load<Texture2D>("hole");
        }
        public void Draw(SpriteBatch spriteBatch)
        {


            spriteBatch.Draw(texture, Position, Color.White);
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

        }
    }
}

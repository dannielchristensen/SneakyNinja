using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SneakyNinja.Collisions;

namespace SneakyNinja
{
    public class PlayerSprite
    {
        public Vector2 Coord = new Vector2(0, 0);
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                bounds.X = position.X + 16;
                bounds.Y = position.Y + 16;
            }
        }
        private Vector2 position;
        private bool flipped = false;
        private SpriteFont bangers;
        public double TotalTime = 0;
        public bool PlayerWins = false;
        public bool HasScroll = false;
        private KeyboardState currentState;
        private ScrollSprite scroll;

        private KeyboardState priorState;
        private Texture2D texture;
        private SneakyNinjas game;
        private BoundingRectangle bounds;
        public bool Detected = false;
        public double DetectedTimer = 30;
        public bool GameOver = false;
        public BoundingRectangle Bounds => bounds;

        public PlayerSprite(SneakyNinjas game, Vector2 coord, Vector2 position)
        {
            this.game = game;
            Coord = coord;
            Position = position;
            scroll = new ScrollSprite(game);
            scroll.Position = new Vector2(5, 5);
            bounds = new BoundingRectangle(new Vector2(position.X+16, position.Y+32), 32, 32);
        }

        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("ninja");
            bangers = game.Content.Load<SpriteFont>("bangers");
            scroll.LoadContent();
            
        }

        public void Update(GameTime gameTime, WallSprite[] walls, EyeSprite eye)
        {
            priorState = currentState;
            currentState = Keyboard.GetState();
            TotalTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (currentState.IsKeyDown(Keys.Left) ||
                   currentState.IsKeyDown(Keys.A))
            {
                checkWalls(eye, walls, new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0));
                flipped = true;

            }

            if (currentState.IsKeyDown(Keys.Right) ||
                currentState.IsKeyDown(Keys.D))
            {
                checkWalls(eye, walls, new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0));
                flipped = false;

            }

            if (currentState.IsKeyDown(Keys.Up) ||
                currentState.IsKeyDown(Keys.W))
            {
                checkWalls(eye, walls, new Vector2(0, -100 * (float)gameTime.ElapsedGameTime.TotalSeconds));
                

            }

            if (currentState.IsKeyDown(Keys.Down) ||
                currentState.IsKeyDown(Keys.S))
            {
                checkWalls(eye, walls, new Vector2(0, 100 * (float)gameTime.ElapsedGameTime.TotalSeconds));

            }

            if(position.X < 32 && !(Coord.Y == 1 && position.Y > 6.5 * 32 && position.Y < (8.5 * 32)))
            {
                position.X = 32;
            }
            else if(position.X > game.GraphicsDevice.Viewport.Width - 80 && !(Coord.Y == 0 && position.Y > 6.5 * 32 && position.Y < (8.5 * 32)))
            {
                position.X = game.GraphicsDevice.Viewport.Width - 80;
            }


            if (position.Y < 64 && !(Coord.X == 1 && position.X > 10.5 * 32 && position.X < (13.5 * 32)))
            {
                position.Y = 64;
            }
            else if (position.Y > game.GraphicsDevice.Viewport.Height - 96 && !(Coord.X == 0 && position.X > 10.5 * 32 && position.X < (13 * 32)))
            {
                position.Y = game.GraphicsDevice.Viewport.Height - 96;
            }
            bounds.X = this.Position.X+16;
            bounds.Y = this.Position.Y+16;
        }
        public void checkWalls(EyeSprite eye, WallSprite[] walls, Vector2 added_pos)
        {
            Position += added_pos;
            if (eye!= null && eye.Bounds.CollidesWith(bounds))
            {
                Position -= added_pos;
                return;
            }

            foreach (WallSprite w in walls)
            {
                if (bounds.CollidesWith(w.Bounds))
                {
                    Position -= added_pos;
                    break;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0);
            if (Detected)
                spriteBatch.DrawString(bangers, $"YOU HAVE BEEN DETECTED. TIME REMAINING: {DetectedTimer:##.##}", new Vector2(25, 5), Color.Maroon);
            else if (TotalTime < 20)
            {
                spriteBatch.DrawString(bangers, $"WASD to move. Retrieve the Scroll and don't get caught.", new Vector2(25, 2), Color.Maroon);

            }

            if (HasScroll)
            {
                scroll.Draw(spriteBatch);
            }
        }
    }
}

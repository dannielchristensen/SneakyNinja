﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SneakyNinja.Collisions;

namespace SneakyNinja
{
    public class Player
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
        private KeyboardState currentState;
        private KeyboardState priorState;
        private Texture2D texture;
        private SneakyNinjas game;
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        public Player(SneakyNinjas game, Vector2 coord, Vector2 position)
        {
            this.game = game;
            Coord = coord;
            Position = position;
            bounds = new BoundingRectangle(new Vector2(position.X+16, position.Y+32), 32, 32);
        }

        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("ninja");
        }

        public void Update(GameTime gameTime, Wall[] walls, EyeSpawn eye)
        {
            priorState = currentState;
            currentState = Keyboard.GetState();
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
        public void checkWalls(EyeSpawn eye, Wall[] walls, Vector2 added_pos)
        {
            Position += added_pos;
            if (eye!= null && eye.Bounds.CollidesWith(bounds))
            {
                Position -= added_pos;
                return;
            }

            foreach (Wall w in walls)
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
        }
    }
}

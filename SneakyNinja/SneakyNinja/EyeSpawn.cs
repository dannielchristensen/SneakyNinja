using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SneakyNinja.Collisions;

namespace SneakyNinja
{
    public enum Direction
    {
        Right,
        Up,
        Left,
        Down
    }
    public class EyeSpawn
    {
        private Room room;
        private int directionAnimation = 1;
        private double animationTimer;
        private SneakyNinjas game;
        private Vector2 position;
        public Vector2 Position;
        private BoundingRectangle vision;
        public BoundingRectangle Vision => vision;
        public BoundingCircle Bounds => bounds;
        private BoundingCircle bounds;
        private Texture2D texture;


        public EyeSpawn(Room r, SneakyNinjas game)
        {
            spawn();
            for (int i = 0; i < r.Walls.Length; i++)
            {
                if (bounds.CollidesWith(r.Walls[i].Bounds) || ( r.Scroll != null && bounds.CollidesWith(r.Scroll.Bounds)))
                {
                    spawn();
                    i = 0;
                }
            }
            room = r;
            this.game = game;

        }
        private void spawn()
        {
            Random rand = new Random();
            Vector2 pos = new Vector2(rand.Next(5, 21)*32, rand.Next(4, 11)*32);
            position = pos;
            bounds = new BoundingCircle(pos + new Vector2(32, 32), 32);

        }

        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("eye_changing");
        }
        public void Update(GameTime gameTime)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(animationTimer > 2.0)
            {
                animationTimer -= 2;
                directionAnimation++;
                if (directionAnimation > 4)
                    directionAnimation = 1;
            }


        }
        public void Draw(SpriteBatch spriteBatch)
        {

            var source = new Rectangle();
            switch (directionAnimation)
            {
                case 1:
                    source = new Rectangle(0, 0, 64, 64);
                    vision = new BoundingRectangle(position.X+64,position.Y, game.GraphicsDevice.Viewport.Width,64);
                    break;
                case 2:
                    source = new Rectangle(64, 0, 64, 64);
                    vision = new BoundingRectangle(position.X, 0, 64, position.Y);
                    break;
                case 3:
                    source = new Rectangle(0, 64, 64, 64);
                    vision = new BoundingRectangle(0, position.Y, position.X, 64);
                    break;
                case 4:
                    source = new Rectangle(64, 64, 64, 64);
                    vision = new BoundingRectangle(position.X, position.Y+64, 64, game.GraphicsDevice.Viewport.Height);
                    break;
                default:
                    source = new Rectangle(0, 0, 64, 64);
                    break;
            }
            spriteBatch.Draw(texture, position, source, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }

        public bool CheckWall(Vector2 wallPosition, Vector2 playerPosition)
        {
            bool isWallCoveringPlayer = false;
            switch (directionAnimation)
            {
                // right
                case 1:
                    if (position.X < wallPosition.X && wallPosition.X < playerPosition.X && (playerPosition.Y + 64 > wallPosition.Y && playerPosition.Y + 12 < wallPosition.Y +32))
                        isWallCoveringPlayer = true;
                    break;
                // up
                case 2:
                    if (position.Y > wallPosition.Y && wallPosition.Y > playerPosition.Y && (playerPosition.X + 64 > wallPosition.X && playerPosition.X + 12 < wallPosition.X + 32))
                        isWallCoveringPlayer = true;
                    break;
                // left
                case 3:
                    if (position.X > wallPosition.X && wallPosition.X > playerPosition.X && (playerPosition.Y+64 > wallPosition.Y && playerPosition.Y+12 < wallPosition.Y + 32))
                        isWallCoveringPlayer = true;
                    break;
                // down
                case 4:
                    if (position.Y < wallPosition.Y && wallPosition.Y < playerPosition.Y && (playerPosition.X + 64 > wallPosition.X && playerPosition.X + 12 < wallPosition.X + 32))
                        isWallCoveringPlayer = true;
                    break;
                default:
                    break;


            }
            return isWallCoveringPlayer;
        }
            
    }
}

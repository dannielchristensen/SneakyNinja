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
    public class EyeSprite
    {
        private Room room;
        private int directionAnimation = 1;
        private double animationTimer;
        private SneakyNinjas game;
        private Vector2 position;
        public Vector2 Position;
        private BoundingRectangle vision;
        private BoundingRectangle[] vision_directions = new BoundingRectangle[4];
        public BoundingRectangle Vision => vision;
        public BoundingCircle Bounds => bounds;
        private BoundingCircle bounds;
        private Texture2D texture;


        public EyeSprite(Room r, SneakyNinjas game)
        {
            room = r;
            this.game = game;

            spawn();

        }
        private void spawn()
        {
            Random rand = new Random();
            Vector2 pos = new Vector2(rand.Next(5, 21)*32, rand.Next(4, 11)*32);
            position = pos;
            bounds = new BoundingCircle(pos + new Vector2(32, 32), 32);

            vision = new BoundingRectangle(position.X + 64, position.Y, game.GraphicsDevice.Viewport.Width, 64);
            vision_directions[0] = vision;
            vision = new BoundingRectangle(position.X, 0, 64, position.Y);
            vision_directions[1] = vision;
            vision = new BoundingRectangle(0, position.Y, position.X, 64);
            vision_directions[2] = vision;
            vision = new BoundingRectangle(position.X, position.Y + 64, 64, game.GraphicsDevice.Viewport.Height);
            vision_directions[3] = vision;

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
                    vision = vision_directions[0];
                    break;
                case 2:
                    source = new Rectangle(64, 0, 64, 64);
                    vision = vision_directions[1];
                    break;
                case 3:
                    source = new Rectangle(0, 64, 64, 64);
                    vision = vision_directions[2];
                    break;
                case 4:
                    source = new Rectangle(64, 64, 64, 64);
                    vision = vision_directions[3];
                    break;
                default:
                    source = new Rectangle(0, 0, 64, 64);
                    vision = vision_directions[0];
                    break;
            }
            spriteBatch.Draw(texture, position, source, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }

       
            
    }
}

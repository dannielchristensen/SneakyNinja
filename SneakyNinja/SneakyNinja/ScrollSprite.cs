using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SneakyNinja.Collisions;

namespace SneakyNinja
{
    public class ScrollSprite
    {
        private Room room;
        private SneakyNinjas game;
        private BoundingRectangle bounds;
        
        private Vector2 position;
        private Texture2D texture;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {

                bounds.X = value.X;
                bounds.Y = value.Y;
                position = value;

            }
        }
        public BoundingRectangle Bounds => bounds;
        public ScrollSprite(SneakyNinjas game)
        {
            this.game = game;
            position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
            bounds = new BoundingRectangle(Position, 32, 32);
        }
        public ScrollSprite(Room r, SneakyNinjas g)
        {
            room = r;
            game = g;
            position = new Vector2(game.GraphicsDevice.Viewport.Width/2, game.GraphicsDevice.Viewport.Height/2);
            bounds = new BoundingRectangle(Position, 32, 32);
        }

        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("scroll");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }

        public void Reset()
        {
            position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
            bounds = new BoundingRectangle(Position, 32, 32);
        }


    }
}

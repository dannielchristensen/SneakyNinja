using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SneakyNinja.Collisions;
using SneakyNinja.Particles;

namespace SneakyNinja
{
    public class ScrollSprite
    {
        private Room room;
        private SneakyNinjas game;
        private BoundingRectangle bounds;
        
        private Vector2 position;
        private Texture2D texture;
        SmokeParticleSystem _smoke;

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
        public void RemoveComponents()
        {
            this.game.Components.Remove(_smoke);

        }
        public ScrollSprite(Room r, SneakyNinjas g)
        {
            room = r;
            game = g;
            position = new Vector2(game.GraphicsDevice.Viewport.Width/2, game.GraphicsDevice.Viewport.Height/2);
            bounds = new BoundingRectangle(Position, 32, 32);
            _smoke = new SmokeParticleSystem(this.game, 25);
            this.game.Components.Add(_smoke);
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

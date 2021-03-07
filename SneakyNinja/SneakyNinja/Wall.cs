using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SneakyNinja.Collisions;

namespace SneakyNinja
{
    public class Wall
    {
        private Random r = new Random();
        private Vector2 position;
        private BoundingRectangle bounds;
        public Texture2D texture;
        private SneakyNinjas game;

        public BoundingRectangle Bounds => bounds;
        public Vector2 Position => position;
        public Wall(SneakyNinjas game)
        {
            position = new Vector2(r.Next(5, 21)*32, r.Next(4, 12)*32);
            bounds = new BoundingRectangle(Position.X, Position.Y, 30, 30);
            this.game = game;

        }
        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("dungeon_wall_32_r");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(texture, new Vector2(bounds.X, bounds.Y), Color.Red);

        }
    }
}

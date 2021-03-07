using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SneakyNinja.Collisions;

namespace SneakyNinja
{
    public class Exit
    {
        public BoundingCircle Bounds;
        public Vector2 Position;
        public Texture2D Texture;
    }
    public enum RoomType
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3
    }
    public class Room
    {
        private Wall[] walls;
        private Vector2 coord;
        private RoomType roomType;
        private Texture2D wallTexture;
        private SneakyNinjas game;
        private EyeSpawn eye;
        private Scroll scroll;
        public Exit Exit;

        public Wall[] Walls => walls;
        public Scroll Scroll
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
        public BoundingRectangle door_x;
        public BoundingRectangle door_y;

        public EyeSpawn Eye => eye;
        public Room(SneakyNinjas game, RoomType type)
        {
            if(type == RoomType.BottomRight)
            {
                scroll = new Scroll(this, game);
            }
            this.game = game;
            roomType = type;
            walls = new Wall[10];
            for (int i = 0; i<walls.Length; i++)
            {
                walls[i] = new Wall(this.game);
                while (type == RoomType.BottomRight && walls[i].Bounds.CollidesWith(scroll.Bounds))
                {
                    walls[i] = new Wall(this.game);
                }
            }
            switch (type)
            {
                case RoomType.TopLeft:
                    coord = new Vector2(0, 0);
                    door_x = new BoundingRectangle(game.GraphicsDevice.Viewport.Width - 22, 7 * 32, 32, 3*32);
                    door_y = new BoundingRectangle(11 * 32, game.GraphicsDevice.Viewport.Height - 22,  3 * 32, 32);
                    Exit = new Exit();
                    break;
                case RoomType.TopRight:
                    coord = new Vector2(2, 0);
                    door_x = new BoundingRectangle(-10, 7 * 32, 32, 3 * 32);
                    door_y = new BoundingRectangle(11 * 32, game.GraphicsDevice.Viewport.Height - 22, 3 * 32, 32);
                    eye = new EyeSpawn(this, game);

                    break;
                case RoomType.BottomLeft:
                    coord = new Vector2(0, 2);
                    door_x = new BoundingRectangle( game.GraphicsDevice.Viewport.Width - 22, 7 * 32, 32, 3*32);
                    door_y = new BoundingRectangle(11 * 32, -10, 3 * 32, 32);
                    eye = new EyeSpawn(this, game);

                    break;
                case RoomType.BottomRight:
                    coord = new Vector2(2, 2);
                    door_x = new BoundingRectangle(-10, 7 * 32, 32, 3 * 32);
                    door_y = new BoundingRectangle(11 * 32, -10, 3 * 32, 32);
                    eye = new EyeSpawn(this, game);
                    

                    break;
            }
        }
        public void LoadContent()
        {
            wallTexture = game.Content.Load<Texture2D>("dungeon_wall_32_r");
            foreach(Wall w in walls)
            {
                w.LoadContent();
            }
            if(roomType != RoomType.TopLeft)
                eye.LoadContent();
            if (scroll != null)
                scroll.LoadContent();
            if(Exit != null)
                Exit.Texture = game.Content.Load<Texture2D>("hole");


        }
        public void Update(GameTime gameTime)
        {
            if (roomType != RoomType.TopLeft)
                eye.Update(gameTime);
        }
        public bool equals(Room r)
        {
            return r.roomType == this.roomType;
        }

        private void DrawTopLeftCornerRoom(SpriteBatch sb)
        {
            int count = 0;
            for(int i = 32; i< game.GraphicsDevice.Viewport.Width; i += 32)
            {
                count++;
                sb.Draw(wallTexture, new Vector2(i, 32), Color.White);
                sb.Draw(wallTexture, new Vector2(0, i), Color.White);
                if(count < 11 || count > 13)
                    sb.Draw(wallTexture, new Vector2(i, game.GraphicsDevice.Viewport.Height-32), Color.White);
                if(count < 7 || count > 9)
                    sb.Draw(wallTexture, new Vector2(game.GraphicsDevice.Viewport.Width - 32, i), Color.White);
            }
        }
        private void DrawTopRightCornerRoom(SpriteBatch sb)
        {
            int count = 0;
            for (int i = 32; i < game.GraphicsDevice.Viewport.Width; i += 32)
            {
                count++;
                sb.Draw(wallTexture, new Vector2(i, 32), Color.White);
                sb.Draw(wallTexture, new Vector2(game.GraphicsDevice.Viewport.Width - 32, i), Color.White);

                if (count < 11 || count > 13)
                    sb.Draw(wallTexture, new Vector2(i, game.GraphicsDevice.Viewport.Height - 32), Color.White);
                if (count < 7 || count > 9)
                    sb.Draw(wallTexture, new Vector2(0, i), Color.White);

            }
        }

        private void DrawBottomLeftCornerRoom(SpriteBatch sb)
        {
            int count = 0;
            for (int i = 32; i < game.GraphicsDevice.Viewport.Width; i += 32)
            {
                count++;
                sb.Draw(wallTexture, new Vector2(0, i), Color.White);
                sb.Draw(wallTexture, new Vector2(i, game.GraphicsDevice.Viewport.Height - 32), Color.White);
                if (count < 11 || count > 13)
                    sb.Draw(wallTexture, new Vector2(i, 32), Color.White);
                if (count < 7 || count > 9)
                    sb.Draw(wallTexture, new Vector2(game.GraphicsDevice.Viewport.Width - 32, i), Color.White);

            }
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
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (this.roomType)
            {
                case RoomType.TopLeft:
                    DrawTopLeftCornerRoom(spriteBatch);
                    break;
                case RoomType.TopRight:
                    DrawTopRightCornerRoom(spriteBatch);
                    break;
                case RoomType.BottomLeft:
                    DrawBottomLeftCornerRoom(spriteBatch);
                    break;
                case RoomType.BottomRight:
                    DrawBottomRightCornerRoom(spriteBatch);
                    break;
            }
            foreach (Wall w in walls)
            {
                w.Draw(spriteBatch);
            }
            if (roomType != RoomType.TopLeft)
                eye.Draw(spriteBatch);
            if (scroll!= null)
                scroll.Draw(spriteBatch);
            if(Exit != null)
                spriteBatch.Draw(Exit.Texture, Exit.Position, Color.White);

        }
    }
}

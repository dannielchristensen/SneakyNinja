using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SneakyNinja.Collisions;
using SneakyNinja.Screens;


namespace SneakyNinja
{

    public enum RoomType
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3
    }
    public class Room
    {
        private WallSprite[] walls;
        private Vector2 coord;
        private RoomType roomType;
        private Texture2D wallTexture;
        private SneakyNinjas game;

        public WallSprite[] Walls => walls;
        public RoomType RoomType => roomType;
        public BoundingRectangle door_x;
        public BoundingRectangle door_y;

        public Room(SneakyNinjas game, RoomType type, ScreenManager ScreenManager)
        {
            this.game = game;
            roomType = type;
            walls = new WallSprite[10];
            for (int i = 0; i<walls.Length; i++)
            {
                walls[i] = new WallSprite(this.game);

            }
            switch (type)
            {
                case RoomType.TopLeft:
                    coord = new Vector2(0, 0);
                    door_x = new BoundingRectangle(ScreenManager.Game.GraphicsDevice.Viewport.Width - 22, 7 * 32, 32, 3*32);
                    door_y = new BoundingRectangle(11 * 32, game.GraphicsDevice.Viewport.Height - 22,  3 * 32, 32);
                    break;
                case RoomType.TopRight:
                    coord = new Vector2(2, 0);
                    door_x = new BoundingRectangle(-10, 7 * 32, 32, 3 * 32);
                    door_y = new BoundingRectangle(11 * 32, ScreenManager.Game.GraphicsDevice.Viewport.Height - 22, 3 * 32, 32);

                    break;
                case RoomType.BottomLeft:
                    coord = new Vector2(0, 2);
                    door_x = new BoundingRectangle(ScreenManager.Game.GraphicsDevice.Viewport.Width - 22, 7 * 32, 32, 3*32);
                    door_y = new BoundingRectangle(11 * 32, -10, 3 * 32, 32);

                    break;
                case RoomType.BottomRight:
                    coord = new Vector2(2, 2);
                    door_x = new BoundingRectangle(-10, 7 * 32, 32, 3 * 32);
                    door_y = new BoundingRectangle(11 * 32, -10, 3 * 32, 32);
                    

                    break;
            }
        }
        public void LoadContent()
        {
            wallTexture = game.Content.Load<Texture2D>("dungeon_wall_32_r");
            foreach(WallSprite w in walls)
            {
                w.LoadContent();
            }
        }
        public bool equals(Room r)
        {
            return r.roomType == this.roomType;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (WallSprite w in walls)
            {
                w.Draw(spriteBatch);
            }
        }
    }
}

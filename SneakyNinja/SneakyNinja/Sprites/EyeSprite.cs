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
    public enum Direction
    {
        Right,
        Up,
        Left,
        Down
    }
    public class EyeSprite:IParticleEmitter
    {
        private Room room;
        private int directionAnimation = 1;
        public int DirectionAnimation => directionAnimation;
        private double animationTimer;
        private SneakyNinjas game;
        private Vector2 position;
        public Vector2 Position => position+new Vector2(32,32);
        private BoundingRectangle vision;
        private BoundingRectangle[] vision_directions = new BoundingRectangle[4];
        public BoundingRectangle Vision => vision;
        public BoundingCircle Bounds => bounds;

        public Vector2 Velocity { get; set; } = new Vector2(100,100);
        public double TimeLeftAnimationTimer;
        private BoundingCircle bounds;
        private Texture2D texture;
        private VisionParticleSystem _visionParticle;
        private Rectangle source = new Rectangle();

        public EyeSprite(Room r, SneakyNinjas game)
        {
            room = r;
            this.game = game;

            spawn();
            _visionParticle = new VisionParticleSystem(this.game, this);
            this.game.Components.Add(_visionParticle);
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
            vision = vision_directions[0];
            source = new Rectangle(0, 0, 64, 64);


        }

        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("eye_changing");
        }
        public void Update(GameTime gameTime)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            TimeLeftAnimationTimer = 2.0 - animationTimer;
            if(animationTimer >= 2.0)
            {
                animationTimer -= 2;
                directionAnimation++;
                if (directionAnimation > 4)
                    directionAnimation = 1;
            }

        }
        public void RemoveComponents()
        {
            this.game.Components.Remove(_visionParticle);

        }
        public bool CheckRay(Vector2 PlayerPosition, WallSprite[] walls)
        {
            Vector2 origin = this.bounds.Center;
            double x = PlayerPosition.X - origin.X;
            double y = origin.Y - PlayerPosition.Y;
            double h =  Math.Sqrt(x * x + y * y);
            double m;
            float convert =  180 / (float) Math.PI;
            float angle;
            switch (directionAnimation)
            {
                // right
                case 1:
                    if (PlayerPosition.X < origin.X)
                        return false;
                    m = (double) y / x;
                    angle = (float) Math.Atan(m) * convert;
                    if(angle > 30 || angle < -30)
                    {
                        return false;
                    }
                    break;
                case 2:
                    // up
                    if (PlayerPosition.Y > origin.Y)
                        return false;
                    m = (double)x / h;
                    angle = (float) Math.Acos(m) * convert;
                    if (angle > 120 || angle < 60)
                        return false;
                    break;
                case 3:
                    // left
                    if (PlayerPosition.X > origin.X)
                        return false;
                    m = (double)y / x;
                    angle = (float)Math.Atan(m) * convert;
                    if (angle > 30 || angle < -30)
                    {
                        return false;
                    }
                    break;
                case 4:
                    // down

                    if (PlayerPosition.Y < origin.Y)
                        return false;
                    m = (double)x / h;
                    angle = (float)Math.Acos(m) * convert;
                    if (angle > 120 || angle < 60)
                        return false;
                    break;
                default:

                    break;
            }
            Ray r = new Ray(new Vector3(origin, 0), new Vector3(PlayerPosition, 3));
            foreach (WallSprite w in walls)
            {
                BoundingBox b = new BoundingBox(new Vector3(w.Position, 0), new Vector3(w.Position.X + 32, w.Position.Y + 32, 0));
                float? w_dist = r.Intersects(b);
                float playerDist = (float)Math.Sqrt(Math.Pow(PlayerPosition.X - this.Position.X, 2) + Math.Pow(PlayerPosition.Y - this.Position.Y, 2));
                if (w_dist != null && w_dist < playerDist)
                {
                    return false;
                }
            }
            return true;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 PlayerPosition)
        {

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

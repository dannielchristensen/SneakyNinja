using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Media;
using SneakyNinja.Collisions;

namespace SneakyNinja
{


    public class SneakyNinjas : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private Room[,] map = new Room[2,2];
        private Player player;
        private bool hasScroll = false;
        private bool detected = false;
        private double detectedTimer = 20;
        private SpriteFont bangers;
        public Vector2 userRoomPosition;
        private bool gameOver = false;
        private double totalTime = 0;
        private bool playerWins = false;
        private double fastestTime;
        private Song backgroundMusic;


        public SneakyNinjas()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            map[0,0] = new Room(this, (RoomType)0);
            map[0,1] = new Room(this, (RoomType)1);
            map[1,0] = new Room(this, (RoomType)2);
            map[1,1] = new Room(this, (RoomType)3);

            userRoomPosition = new Vector2(0, 0);
            player = new Player(this, userRoomPosition, new Vector2(96, 96));
            map[0, 0].Exit.Position = player.Position;
            map[0, 0].Exit.Bounds = new BoundingCircle(map[0, 0].Exit.Position + new Vector2(16, 16), 16);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map[0, 0].LoadContent();
            map[0, 1].LoadContent();
            map[1, 0].LoadContent();
            map[1, 1].LoadContent();
            bangers = Content.Load<SpriteFont>("bangers");
            player.LoadContent();
            backgroundMusic = Content.Load<Song>("NinjaWarrior");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeys = Keyboard.GetState();
            if (currentKeys.IsKeyDown(Keys.Escape))
                Exit();
            if(gameOver && currentKeys.IsKeyDown(Keys.Enter))
            {
                map[0, 0].Scroll = null;
                map[0, 1].Scroll = null;
                map[1, 0].Scroll = null;
                userRoomPosition = new Vector2(0, 0);
                player = new Player(this, userRoomPosition, new Vector2(64, 64));
                map[1, 1].Scroll.Reset();
                detected = false;
                detectedTimer = 20;
                map[0, 0].Exit.Position = player.Position;
                map[0, 0].Exit.Bounds = new BoundingCircle(map[0, 0].Exit.Position + new Vector2(16, 16), 16);
                totalTime = 0;
                gameOver = false;
                hasScroll = false;
                playerWins = false;

                this.LoadContent();
                return;

            }
            else if (gameOver && currentKeys.IsKeyDown(Keys.S))
            {
                totalTime = 0;
                fastestTime = 0;
                playerWins = false;
                userRoomPosition = new Vector2(0, 0);
                player = new Player(this, userRoomPosition, new Vector2(64, 64));
                map[0, 0] = new Room(this, (RoomType)0);
                map[0, 1] = new Room(this, (RoomType)1);
                map[1, 0] = new Room(this, (RoomType)2);
                map[1, 1] = new Room(this, (RoomType)3);
                userRoomPosition = new Vector2(0, 0);
                map[0, 0].Exit.Position = player.Position;
                map[0, 0].Exit.Bounds = new BoundingCircle(map[0, 0].Exit.Position + new Vector2(16, 16), 16);
                detected = false;
                detectedTimer = 20;
                gameOver = false;
                hasScroll = false;

                this.LoadContent();
                return;

            }
            else if(gameOver)
            {
                return;
            }
            totalTime += gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 old_pos = player.Position;
            Room cur = map[(int)userRoomPosition.X, (int)userRoomPosition.Y];

            player.Update(gameTime, cur.Walls, cur.Eye);

            // TODO: Add your update logic here

            if (player.Bounds.CollidesWith(cur.door_x))
            {
                if(userRoomPosition.Y == 0)
                {
                    userRoomPosition.Y = 1;
                    player.Coord.Y = 1;
                    player.Position = new Vector2(64, player.Position.Y);
                }
                else
                {
                    userRoomPosition.Y = 0;
                    player.Coord.Y = 0;
                    player.Position = new Vector2(this.GraphicsDevice.Viewport.Width - 96, player.Position.Y);

                }

            }
            else if(player.Bounds.CollidesWith(cur.door_y)) {
                if (userRoomPosition.X == 0)
                {
                    userRoomPosition.X = 1;
                    player.Coord.X = 1;
                    player.Position = new Vector2(player.Position.X, 64);

                }
                else
                {
                    userRoomPosition.X = 0;
                    player.Coord.X = 0;
                    player.Position = new Vector2(player.Position.X, this.GraphicsDevice.Viewport.Height - 96);

                }
            }

            if(cur.Eye != null && cur.Eye.Vision.CollidesWith(player.Bounds))
            {
                bool isHidden = false;
                foreach(Wall w in cur.Walls)
                {
                    if (cur.Eye.Vision.CollidesWith(w.Bounds))
                    {
                        isHidden = (cur.Eye.CheckWall(w.Position, player.Position)) || isHidden;
                        
                    }
                }
                if(!detected)
                    detected = !isHidden;
            }
            if (detected)
            {
                detectedTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if(detectedTimer <= 0)
                {
                    gameOver = true;
                }
            }
            if(cur.Scroll!= null && player.Bounds.CollidesWith(cur.Scroll.Bounds))
            {
                hasScroll = true;
                cur.Scroll.Position = new Vector2(5, 5);
                foreach(Room r in map)
                {
                    r.Scroll = cur.Scroll;
                }
            }

            cur.Update(gameTime);
            if(hasScroll && cur.Exit != null && cur.Exit.Bounds.CollidesWith(player.Bounds))
            {
                playerWins = true;
                gameOver = true;
                if (totalTime < fastestTime)
                    fastestTime = totalTime;
            }
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();

            // TODO: Add your drawing code here

            //var rect = new Rectangle((int)cur.door_x.X, (int)cur.door_x.Y, (int)cur.door_x.Width, (int)cur.door_x.Height);
            //spriteBatch.Draw(cur.Walls[0].texture, rect, Color.Blue); 
            if(playerWins && gameOver)
            {
                spriteBatch.DrawString(bangers, $"YOU WIN. YOU GOT THE SCROLL WITHOUT GETTING CAUGHT.", new Vector2(25, 5), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS ENTER TO CONTINUE WITH THE SAME MAP.", new Vector2(25, 35), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS S TO SPAWN A NEW MAP.", new Vector2(25, 65), Color.Black);
                spriteBatch.DrawString(bangers, $"RUN TIME: {totalTime:######.##}.", new Vector2(25, 95), Color.Black);
                if(fastestTime != 0)
                {
                    spriteBatch.DrawString(bangers, $"FASTEST RUN TIME ON THIS SEED TIME: {fastestTime:######.##}.", new Vector2(25, 125), Color.Black);

                }
                spriteBatch.End();

                base.Draw(gameTime);
                return;

            }
            else if (gameOver)
            {
                spriteBatch.DrawString(bangers, $"YOU WERE CAUGHT. GAME OVER.", new Vector2(25, 5), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS ENTER TO CONTINUE WITH THE SAME MAP.", new Vector2(25, 35), Color.Black);
                spriteBatch.DrawString(bangers, $"PRESS S TO SPAWN A NEW MAP.", new Vector2(25, 65), Color.Black);

                spriteBatch.End();

                base.Draw(gameTime);
                return;

            }
            else if (detected)
            {
                spriteBatch.DrawString(bangers, $"YOU HAVE BEEN DETECTED. TIME REMAINING: {detectedTimer:##.##}", new Vector2(25, 5), Color.Maroon);

            }
            else if(totalTime < 20)
            {
                spriteBatch.DrawString(bangers, $"WASD to move. Retrieve the Scroll and don't get caught.", new Vector2(25, 2), Color.Maroon);

            }
            Room cur = map[(int)userRoomPosition.X, (int)userRoomPosition.Y];
            cur.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

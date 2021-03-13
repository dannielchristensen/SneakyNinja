using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using SneakyNinja.Collisions;
using SneakyNinja.Screens;

namespace SneakyNinja
{


    public class SneakyNinjas : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        //private Room[,] map = new Room[2,2];

        private Song backgroundMusic;

        private readonly ScreenManager _screenManager;

        public SneakyNinjas()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);
            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);
            AddInitialScreens();
        }
        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new MainMenuScreen(this));

        }

        protected override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundMusic = Content.Load<Song>("NinjaWarrior");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

        }

        protected override void Update(GameTime gameTime)
        {
            
                    
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Gray);
           
            base.Draw(gameTime);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SneakyNinja
{
    public class ParticleSystem : DrawableGameComponent
    {
        public const int AlphaBlendDrawOrder = 100;
        public const int AdditiveBlendDrawOrder = 200;
        protected static SpriteBatch spriteBatch;
        protected static ContentManager contentManager;
        Particle[] particles;
        Queue<Particle> freeParticles;
        Texture2D texture;
        Vector2 origin;
        public int FreeParticleCount => freeParticles.Count;
        protected BlendState blendState = BlendState.AlphaBlend;
        protected string textureFilename;
        protected int minNumParticles;
        protected int maxNumParticles;
        public ParticleSystem(Game game, int maxParticles) : base(game)
        {
            particles = new Particle[maxParticles];
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle();
            }
            freeParticles = new Queue<Particle>(particles);
            InitializeConstants();
        }
        protected virtual void InitializeConstants() { }
        protected virtual void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where);
        }
        protected virtual void UpdateParticle(Particle particle, float dt)
        {
            particle.Velocity += particle.Acceleration * dt;
            particle.Position += particle.Velocity * dt;

            particle.AngularVelocity += particle.AngularAcceleration * dt;
            particle.Rotation += particle.AngularVelocity * dt;

            particle.TimeSinceStart += dt;
        }

        protected override void LoadContent()
        {

            if (contentManager == null) contentManager = new ContentManager(Game.Services, "Content");
            if (spriteBatch == null) spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            if (string.IsNullOrEmpty(textureFilename))
            {
                string message = "textureFilename wasn't set properly.";
                throw new InvalidOperationException(message);
            }
            texture = contentManager.Load<Texture2D>(textureFilename);
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Particle p in particles)
            {

                if (p.Active)
                {
                    UpdateParticle(p, dt);
                    if (!p.Active)
                    {
                        freeParticles.Enqueue(p);
                    }
                }
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(blendState: blendState);

            foreach (Particle p in particles)
            {
                if (!p.Active)
                    continue;

                spriteBatch.Draw(texture, p.Position, null, p.Color,
                    p.Rotation, origin, 1, SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void AddParticles(Vector2 where)
        {

            int numParticles = RandomHelper.Next(minNumParticles, maxNumParticles);

            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                Particle p = freeParticles.Dequeue();
                InitializeParticle(ref p, where);
            }
        }

        protected void AddParticles(Rectangle where)
        {
            // the number of particles we want for this effect is a random number
            // somewhere between the two constants specified by the subclasses.
            int numParticles =
                RandomHelper.Next(minNumParticles, maxNumParticles);

            // create that many particles, if you can.
            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                // grab a particle from the freeParticles queue, and Initialize it.
                Particle p = freeParticles.Dequeue();
                InitializeParticle(ref p, RandomHelper.RandomPosition(where));
            }
        }
    }
}

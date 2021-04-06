using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SneakyNinja.Particles { 
    public class SmokeParticleSystem : ParticleSystem
    {
        Game game;
        public SmokeParticleSystem(Game game, int maxExplosions) : base(game, maxExplosions * 5)
        {
            this.game = game;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "smoke";
            minNumParticles = 0;
            maxNumParticles = 2;
            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }
        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40, 200);
            var lifetime = RandomHelper.NextFloat(.5f, 1f);
            var acceleration = -velocity / lifetime;

            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);
            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);
            p.Initialize(where, velocity, acceleration, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale:1);
        }

        protected override void UpdateParticle(Particle particle, float dt)
        {
            base.UpdateParticle(particle, dt);
            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;
            //float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
            particle.Color = Color.DarkSlateGray;
            particle.Scale = .1f + .25f * normalizedLifetime;

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            AddParticles(new Vector2(this.game.GraphicsDevice.Viewport.Width / 2, this.game.GraphicsDevice.Viewport.Height / 2));

        }

    }
}

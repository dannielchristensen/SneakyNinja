using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SneakyNinja.Particles
{
    public class DustParticleSystem : ParticleSystem
    {

        IParticleEmitter _emitter;

        public DustParticleSystem(Game game, IParticleEmitter emitter) : base(game, 2000) 
        {
            _emitter = emitter;
        }
        protected override void InitializeConstants()
        {
            textureFilename = "particle";

            minNumParticles = 0;
            maxNumParticles = 3;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }
        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            Random r = new Random();

            base.InitializeParticle(ref p, where);
            p.Velocity = _emitter.Velocity;
            p.Acceleration = new Vector2(r.Next(-100,100), r.Next(-50,50));
            p.Scale = RandomHelper.NextFloat(0.1f, 0.5f);
            p.Lifetime = RandomHelper.NextFloat(0.1f, 1.0f);
            p.Acceleration.X = -p.Velocity.X / p.Lifetime;
            p.Color = Color.Purple;
           
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            AddParticles(_emitter.Position);
        }
    }

}

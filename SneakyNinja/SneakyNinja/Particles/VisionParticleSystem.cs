using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SneakyNinja.Particles
{
    public class VisionParticleSystem:ParticleSystem
    {
        IParticleEmitter _emitter;

        public VisionParticleSystem(Game game, IParticleEmitter emitter) : base(game, 4000)
        {
            _emitter = emitter;
        }
        protected override void InitializeConstants()
        {
            textureFilename = "vision_single";
            minNumParticles = 0;
            maxNumParticles = 2;
        }
        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var acceleration = Vector2.UnitY * 400;
            var velocity = new Vector2(0, 0);
            var rotation = 0.0;
            var lifetime = 2.0;
            if (_emitter is EyeSprite e){
                lifetime = e.TimeLeftAnimationTimer;
                switch (e.DirectionAnimation)
                {
                    case 1:
                        acceleration = new Vector2(25, 0);
                        velocity = new Vector2(200, 0);

                        break;
                    case 2:
                        // up
                        rotation = 3*Math.PI / 2.0;
                        velocity = new Vector2(0, -200);
                        acceleration = new Vector2(0, -25);

                        break;
                    case 3:
                        // left
                        rotation = Math.PI;
                        acceleration = new Vector2(-25, 0);
                        velocity = new Vector2(-200, 0);

                        break;
                    case 4:
                        // down
                        rotation = Math.PI/2;
                        acceleration = new Vector2(0, 25);
                        velocity = new Vector2(0, 200);

                        break;
                    default:
                        // assume right, so do nothing
                        break;
                }

            }
            p.Initialize(where, velocity, acceleration, Color.White, rotation: (float)rotation, lifetime: (float)lifetime);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var position = _emitter.Position;
            if (_emitter is EyeSprite e)
            {
                switch (e.DirectionAnimation)
                {
                    case 1:
                        position += new Vector2(32, 0);

                        break;
                    case 2:
                        // up
                        position += new Vector2(0, -32);


                        break;
                    case 3:
                        // left
                        position += new Vector2(-32, 0);


                        break;
                    case 4:
                        // down
                        position += new Vector2(0, 32);


                        break;
                    default:
                        // assume right, so do nothing
                        break;
                }

            }
            AddParticles(position);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class ShipExplosion : IParticle
    {
        private readonly List<IParticle> _shipParticles = new List<IParticle>();
        public EventHandler AnimationFinished;
        public ShipExplosion(Vector2 position)
        {
            IsParticleActive = true;
            Origin = position;
            var random = new Random();
            var particleCount = 200;
            for (int i = 0; i < particleCount; i++)
            {
                var vector = new Vector2(0, -1);
                vector = SetAngle(vector, random.Next(0, 360));
                vector.Normalize();
                var explosionParticle = new ShipExplosionParticle(ContentProvider.ExhaustParticles[6], position, vector, (float)random.NextDouble());
                _shipParticles.Add(explosionParticle);
            }
        }

        private Vector2 SetAngle(Vector2 vector, float angle)
        {
            var length = vector.Length();
            vector.X = (float)Math.Cos(angle) * length;
            vector.Y = (float)Math.Sin(angle) * length;
            return vector;
        }

        public Vector2 Origin { get; set; }
        public bool IsParticleActive { get; set; }
        public void Update(GameTime gametime)
        {
            foreach (var shipParticle in _shipParticles)
            {
                shipParticle.Update(gametime);
            }

            if (_shipParticles.All(particle => !particle.IsParticleActive))
            {
                AnimationFinished?.Invoke(this,null);
                IsParticleActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var shipParticle in _shipParticles)
            {
                shipParticle.Draw(spriteBatch);
            }
        }
    }
}
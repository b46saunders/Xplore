using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class Lazer : Sprite, IParticle
    {
        public Vector2 Origin { get; set; }
        public bool IsActive { get; set; }
        private float Speed = 16f;
        private Vector2 _velocityVector;
        private readonly float _maxDistance;
        protected Vector2 Velocity = Vector2.Zero;

        public Lazer(Texture2D texture, Vector2 position, Vector2 directionVector,float maxDistance) : base(texture, position)
        {
            _maxDistance = maxDistance;
            Origin = position;
            rotation = (float)directionVector.GetRotationFromVector();
            _velocityVector = new Vector2(directionVector.X, directionVector.Y);
            _velocityVector.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            if ((position - Origin).Length() > _maxDistance)
            {
                IsActive = false;
            }
            else
            {
                Velocity = _velocityVector*Speed;
            }

            position = position + Velocity;
            base.Update(gameTime);
        }
    }
}
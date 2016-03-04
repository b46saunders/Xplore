using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class Lazer : Sprite, IParticle
    {
        private Vector2 velocityVector;
        private float _maxDistance;
        public Lazer(Texture2D texture, Vector2 position, Vector2 directionVector,float maxDistance) : base(texture, position)
        {
            _maxDistance = maxDistance;
            Origin = position;
            rotation = (float)directionVector.GetRotationFromVector();
            velocityVector = new Vector2(directionVector.X, directionVector.Y);
            velocityVector.Normalize();
        }

        public Vector2 Origin { get; set; }
        public bool IsActive { get; set; }

        private float Speed = 4f;

        public override void Update(GameTime gameTime)
        {
            if ((position - Origin).Length() > _maxDistance)
            {
                IsActive = false;
            }
            else
            {
                velocity = velocityVector*Speed;
            }
            
            base.Update(gameTime);
        }
    }
}
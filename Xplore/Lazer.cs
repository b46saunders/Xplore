using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class Lazer : Sprite, IParticle, ICollisionEntity
    {
        public Vector2 Origin { get; set; }
        public bool IsParticleActive { get; set; }
        private float Speed = 16f;
        private readonly Vector2 _velocityVector;
        private readonly float _maxDistance;
        protected Vector2 Velocity = Vector2.Zero;

        public Lazer(Texture2D texture, Vector2 position, Vector2 directionVector,float maxDistance) : base(texture, position)
        {
            Guid = Guid.NewGuid();
            _maxDistance = maxDistance;
            Origin = position;
            Rotation = (float)directionVector.GetRotationFromVector();
            _velocityVector = new Vector2(directionVector.X, directionVector.Y);
            _velocityVector.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            if ((position - Origin).Length() > _maxDistance)
            {
                IsParticleActive = false;
            }
            else
            {
                Velocity = _velocityVector*Speed;
            }

            position = position + Velocity;
            base.Update(gameTime);
        }

        public CollisionType CollisionsWith => CollisionType.Lazer;
        public bool Active => IsParticleActive;
        public Guid Guid { get; }
        public Rectangle BoundingBox => BoundingCircle.SourceRectangle;
        public Circle BoundingCircle => new Circle(position,5);
        public void ResolveSphereCollision(Vector2 mtdVector)
        {
            
        }

        public void ApplyCollisionDamage(GameTime gametime, int damage)
        {
            
        }
    }
}
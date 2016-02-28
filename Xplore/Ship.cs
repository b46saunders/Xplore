using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public abstract class Ship : Sprite
    {
        protected Vector2 DirectionVector;
        protected Vector2 DirectionGoalVector;
        protected Vector2 VelocityGoal;
        public Rectangle BoundingBox => new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        protected float RotationSpeed = 0.95f;
        protected float Speed = 4f;
        protected Rectangle ScreenBounds;
        protected double LastFire = 0;
        protected readonly List<IParticle> CurrentParticles = new List<IParticle>();

        protected Ship(Texture2D texture, Vector2 position, Rectangle screenBounds) : base(texture, position)
        {
            ScreenBounds = screenBounds;
        }

    }
}
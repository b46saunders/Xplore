using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public abstract class Ship : Sprite
    {
        protected Vector2 DirectionVector;
        protected Vector2 DirectionGoalVector;
        protected Vector2 VelocityGoal;
        protected static Random random = new Random(100);
        public Rectangle BoundingBox => new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        public Circle BoundingCircle => new Circle(Center,texture.Height/2f);
        protected float RotationSpeed = 0.95f;
        protected float Speed = 4f;
        protected Rectangle ScreenBounds;
        protected double LastFire = 0;
        protected readonly List<IParticle> CurrentParticles = new List<IParticle>();

        protected Ship(Texture2D texture, Vector2 position, Rectangle screenBounds) : base(texture, position)
        {
            ScreenBounds = screenBounds;
        }

        protected void CheckBounds()
        {
            position.Y = MathHelper.Clamp(position.Y, -(ScreenBounds.Height / 2), ScreenBounds.Height / 2 - BoundingBox.Height);
            position.X = MathHelper.Clamp(position.X, -(ScreenBounds.Width / 2), ScreenBounds.Width / 2 - BoundingBox.Width);
        }

        public void CleanupParticles()
        {
            var particleArray = CurrentParticles.ToArray();
            for (int i = 0; i < particleArray.Length; i++)
            {
                if (!particleArray[i].IsActive)
                {
                    CurrentParticles.Remove(particleArray[i]);
                }
            }
        }


        public Vector2 ResolveCollision(Rectangle collidingWith)
        {
            var aMin = new Vector2(BoundingBox.X, BoundingBox.Y);
            var aMax = new Vector2(BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height);
            var bMin = new Vector2(collidingWith.X, collidingWith.Y);
            var bMax = new Vector2(collidingWith.X + collidingWith.Width, collidingWith.Y + collidingWith.Height);

            var mtd = new Vector2();

            float left = bMin.X - aMax.X;
            float right = bMax.X - aMin.X;
            float top = bMin.Y - aMax.Y;
            float bottom = bMax.Y - aMin.Y;

            if (left > 0 || right < 0)
            {
                throw new Exception("not intersecting");
            }
            if (top > 0 || bottom < 0)
            {
                throw new Exception("not intersecting");
            }

            //box intersect. work out the mtd on both x and y
            if (Math.Abs(left) < right)
            {
                mtd.X = left;
            }
            else
            {
                mtd.X = right;
            }
            if (Math.Abs(top) < bottom)
            {
                mtd.Y = top;
            }
            else
            {
                mtd.Y = bottom;
            }

            //0 the axis with the largest mtd value
            if (Math.Abs(mtd.X) < Math.Abs(mtd.Y))
            {
                mtd.Y = 0;
            }
            else
            {
                mtd.X = 0;
            }

            
            if (mtd.X < 0 || mtd.X > 0)
            {
                velocity.X = 0;
            }
            if (mtd.Y < 0 || mtd.Y > 0)
            {
                velocity.Y = 0;
            }
            position += mtd;
            return mtd;

        }

        protected void Fire()
        {
            CurrentParticles.Add(new Lazer(ContentProvider.Laser, new Vector2(Center.X - ContentProvider.Laser.Width / 2f, Center.Y - ContentProvider.Laser.Height / 2f), DirectionVector, 2000f) { IsActive = true });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(BoundingBox, ContentProvider.OutlineTexture, Color.Purple, spriteBatch, false, 1, rotation, new Vector2(position.X, position.Y));
            spriteBatch.Draw(ContentProvider.CollsionSphereTexture,BoundingCircle.SourceRectangle,Color.White);
            foreach (var currentParticle in CurrentParticles)
            {
                currentParticle.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            CleanupParticles();
            foreach (var currentParticle in CurrentParticles)
            {
                currentParticle.Update(gameTime);
            }
            CheckBounds();
            base.Update(gameTime);
        }


        protected void CreateExhaustParticles()
        {
            var rand = random.Next(0, ContentProvider.ExhaustParticles.Count - 1);
            var particleTexture = ContentProvider.ExhaustParticles[rand];


            var exhuastPoint = new Vector2(position.X + texture.Width / 2f, position.Y + texture.Height);
            var origin = new Vector2(position.X + texture.Width / 2f, position.Y + texture.Height / 2f);
            exhuastPoint = exhuastPoint.RotateAboutOrigin(origin, rotation);

            var spread = 30;
            for (int i = 0; i < 4; i++)
            {

                var particleDirection = -DirectionVector;
                var x = (particleDirection.X + random.Next(-spread, spread) / 100f);
                var y = (particleDirection.Y + random.Next(-spread, spread) / 100f);
                var randomVector = new Vector2(x, y);
                randomVector.Normalize();

                CurrentParticles.Add(new ShipExhaust(particleTexture, exhuastPoint, randomVector));
            }

        }

    }

    public class Circle
    {
        public float Radius { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle SourceRectangle
            => new Rectangle((int) (Position.X - Radius),
                    (int) (Position.Y - Radius), (int) (Radius*2),
                    (int) (Radius*2));

        public Circle(Vector2 position,float radius)
        {
            Radius = radius;
            Position = position;
        }

    }
}
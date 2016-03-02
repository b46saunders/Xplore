using System;
using System.Collections.Generic;
using System.Diagnostics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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
        protected float RotationSpeed = 0.95f;
        protected float Speed = 4f;
        protected Rectangle ScreenBounds;
        protected double LastFire = 0;
        protected readonly List<IParticle> CurrentParticles = new List<IParticle>();

        protected Ship(Texture2D texture, Vector2 position, Rectangle screenBounds) : base(texture, position,true)
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

        protected void Fire()
        {
            CurrentParticles.Add(new Lazer(ContentProvider.Laser, new Vector2(Center.X - ContentProvider.Laser.Width / 2f, Center.Y - ContentProvider.Laser.Height / 2f), DirectionVector, 2000f) { IsActive = true });
            Debug.WriteLine(Center);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(new Rectangle((int)ConvertUnits.ToDisplayUnits(Body.Position.X), (int)ConvertUnits.ToDisplayUnits(Body.Position.Y), texture.Width, texture.Height),ContentProvider.OutlineTexture,Color.Green,spriteBatch,false,1,rotation,position);
            DrawHelper.DrawRectangle(BoundingBox, ContentProvider.OutlineTexture, Color.Purple, spriteBatch, false, 1, rotation, new Vector2(position.X, position.Y));
            foreach (var currentParticle in CurrentParticles)
            {
                currentParticle.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            //Body.Position = new Vector2(ConvertUnits.ToSimUnits(position.X), ConvertUnits.ToSimUnits(position.Y));
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
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class Player : Sprite, IShip
    {
        public Rectangle BoundingBox => new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        private float rotationSpeed = 0.01f;
        private float Speed = 5f;
        private Vector2 scale = new Vector2(1.0f,1.0f);
        private Vector2 originalScale = new Vector2(1f,1f);
        private Vector2 zoomOutScale = new Vector2(0.5f,0.5f);
        private Vector2 scaleGoal = new Vector2(1,1);
        private Vector2 directionVector;
        private Vector2 directionGoalVector;
        private Vector2 velocityGoal;
        private Texture2D laser;
        private Rectangle _screenBounds;
        private double lastFire = 0;
        private List<Texture2D> _particleTextures;
        private List<IParticle> currentParticles = new List<IParticle>();
        public Player(Texture2D texture, Vector2 position, Rectangle screenBounds, List<Texture2D> particleTextures,Texture2D laser) : base(texture, position)
        {
            directionVector = new Vector2(0, -1);
            directionGoalVector = directionVector;
            _screenBounds = screenBounds;
            this.laser = laser;
            _particleTextures = particleTextures;
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            velocityGoal = Vector2.Zero;
            var vector = Camera.GetWorldPosition(new Vector2(mouseState.X, mouseState.Y));

            directionGoalVector.X = vector.X - (BoundingBox.Center.X - BoundingBox.Width / 2f);
            directionGoalVector.Y = vector.Y - (BoundingBox.Center.Y - BoundingBox.Height / 2f);
            directionGoalVector.Normalize();
            rotation = (float)directionVector.GetRotationFromVector();

            if (mouseState.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.TotalMilliseconds > lastFire+200 )
            {
                lastFire = gameTime.TotalGameTime.TotalMilliseconds;
                Fire();
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                scaleGoal = zoomOutScale;
                CreateExhaustParticles();
                velocityGoal = (directionVector)*Speed;
            }
            else
            {
                scaleGoal = originalScale;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                velocityGoal += (directionVector) * -Speed;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                velocityGoal += new Vector2(-directionVector.Y, directionVector.X) * Speed;
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                velocityGoal += new Vector2(directionVector.Y, -directionVector.X) * Speed;
            }

            Debug.WriteLine(position);
            Debug.WriteLine(BoundingBox);
            directionVector = Vector2.Lerp(directionGoalVector, directionVector, 0.95f);
            velocity = Vector2.Lerp(velocityGoal, velocity, 0.99f);
            //scale = Vector2.Lerp(scaleGoal,scale,0.995f);

            Camera.Zoom = scale.X;
            //rotation = (float)getRotationFromDirection(directionVector);
            //KeepWithinBounds();
            CleanupParticles();

            foreach (var currentParticle in currentParticles)
            {
                currentParticle.Update(gameTime);
            }
            CheckBounds();
            base.Update(gameTime);
        }

        private void CheckBounds()
        {
            position.Y = MathHelper.Clamp(position.Y, -(_screenBounds.Height/2)+BoundingBox.Height/2, _screenBounds.Height/2 - BoundingBox.Height/2);
            position.X = MathHelper.Clamp(position.X, -(_screenBounds.Width / 2)+BoundingBox.Width/2, _screenBounds.Width/2 - BoundingBox.Width/2);
        }

        private void Fire()
        {
            currentParticles.Add(new Lazer(laser, position,directionVector,2000f) {IsActive = true});
        }


        public void CleanupParticles()
        {
            var particleArray = currentParticles.ToArray();
            for (int i = 0; i < particleArray.Length; i++)
            {
                if (!particleArray[i].IsActive)
                {
                    currentParticles.Remove(particleArray[i]);
                }
            }
        }

        public static Random random = new Random(100);
        private void CreateExhaustParticles()
        {
            var rand = random.Next(0, _particleTextures.Count - 1);
            var particleTexture = _particleTextures[rand];


            var exhuastPoint = new Vector2(position.X+texture.Width/2f,position.Y+texture.Height);
            var origin = new Vector2(position.X+texture.Width/2f,position.Y+texture.Height/2f);
            
            
            exhuastPoint = RotateAboutOrigin(exhuastPoint, origin);
            //Debug.WriteLine(exhuastPoint);

            var spread = 500;
            for (int i = 0; i < 4; i++)
            {
                
                var particleDirection = -directionVector;
                var x = (particleDirection.X + random.Next(-spread, spread) / 100f);
                var y = (particleDirection.Y + random.Next(-spread, spread) / 100f);
                var randomVector = new Vector2(x, y);
                randomVector.Normalize();

                currentParticles.Add(new ShipExhaust(particleTexture, exhuastPoint, randomVector));
            }

        }

        public Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin)
        {
            return Vector2.Transform(point,
                Matrix.CreateTranslation(-origin.X, -origin.Y, 0f)*
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(origin.X,origin.Y,0f)
                );
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var currentParticle in currentParticles)
            {
                currentParticle.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }

    public static class Vector2DEx
    {
        public static double GetRotationFromVector(this Vector2 unitLengthVector)
        {
            return (float)Math.Atan2(unitLengthVector.X, -unitLengthVector.Y);
        }
    }

}
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
   
    public class Player : Ship, IShip
    {
        private Vector2 scale = new Vector2(1.5f,1.5f);
        private Vector2 originalScale = new Vector2(1f,1f);
        private Vector2 zoomOutScale = new Vector2(0.5f,0.5f);
        private Vector2 scaleGoal = new Vector2(1,1);

        public Player(Texture2D texture, Vector2 position,Rectangle screenBounds) : base(texture, position, screenBounds)
        {
            DirectionVector = new Vector2(0, -1);
            DirectionGoalVector = DirectionVector;
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            VelocityGoal = Vector2.Zero;
            var vector = Camera.GetWorldPosition(new Vector2(mouseState.X, mouseState.Y));

            DirectionGoalVector.X = vector.X - (BoundingBox.Center.X - BoundingBox.Width / 2f);
            DirectionGoalVector.Y = vector.Y - (BoundingBox.Center.Y - BoundingBox.Height / 2f);
            DirectionGoalVector.Normalize();
            rotation = (float)DirectionVector.GetRotationFromVector();

            if (mouseState.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.TotalMilliseconds > LastFire+200 )
            {
                LastFire = gameTime.TotalGameTime.TotalMilliseconds;
                Fire();
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                scaleGoal = zoomOutScale;
                CreateExhaustParticles();
                VelocityGoal = (DirectionVector)*Speed;
            }
            else
            {
                scaleGoal = originalScale;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                VelocityGoal += (DirectionVector) * -Speed;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                VelocityGoal += new Vector2(-DirectionVector.Y, DirectionVector.X) * Speed;
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                VelocityGoal += new Vector2(DirectionVector.Y, -DirectionVector.X) * Speed;
            }

            DirectionVector = Vector2.Lerp(DirectionGoalVector, DirectionVector, RotationSpeed);
            velocity = Vector2.Lerp(VelocityGoal, velocity, 0.99f);
            scale = Vector2.Lerp(scaleGoal,scale,0.995f);

            Camera.Zoom = scale.X;
            //rotation = (float)getRotationFromDirection(directionVector);
            //KeepWithinBounds();
            CleanupParticles();

            foreach (var currentParticle in CurrentParticles)
            {
                currentParticle.Update(gameTime);
            }
            CheckBounds();
            base.Update(gameTime);
        }

        private void CheckBounds()
        {
            position.Y = MathHelper.Clamp(position.Y, -(ScreenBounds.Height/2), ScreenBounds.Height/2 - BoundingBox.Height);
            position.X = MathHelper.Clamp(position.X, -(ScreenBounds.Width / 2), ScreenBounds.Width/2 - BoundingBox.Width);
        }

        private void Fire()
        {
            CurrentParticles.Add(new Lazer(ContentProvider.Laser,new Vector2(Center.X- ContentProvider.Laser.Width/2f,Center.Y- ContentProvider.Laser.Height/2f), DirectionVector,2000f) {IsActive = true});
            Debug.WriteLine(Center);
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

        public static void DrawRectangle(Rectangle rec, Texture2D tex, Color col, SpriteBatch spriteBatch, bool solid, int thickness,float rotation,Vector2 origin)
        {
            Vector2 Position = new Vector2(rec.X, rec.Y);
            if (!solid)
            {
                int border = thickness;

                int borderWidth = (int)(rec.Width) + (border * 2);
                int borderHeight = (int)(rec.Height) + (border);

                //now we need to rotate all the vectors...
                var topStart = new Vector2((int) rec.X, (int) rec.Y);
                var topEnd = new Vector2((int) rec.X + rec.Width, (int) rec.Y);
                var bottomStart = new Vector2((int) rec.X, (int) rec.Y + rec.Height);
                var bottomEnd = new Vector2((int) rec.X + rec.Width, (int) rec.Y + rec.Height);
                var leftStart = new Vector2((int) rec.X, (int) rec.Y);
                var leftEnd = new Vector2((int) rec.X, (int) rec.Y + rec.Height);
                var rightStart = new Vector2((int) rec.X + rec.Width, (int) rec.Y);
                var rightEnd = new Vector2((int) rec.X + rec.Width, (int) rec.Y + rec.Height);

                DrawStraightLine(topStart,topEnd , tex, col, spriteBatch, thickness,rotation,origin); //top bar 
                DrawStraightLine(bottomStart, bottomEnd, tex, col, spriteBatch, thickness, rotation, origin); //bottom bar 
                DrawStraightLine(leftStart, leftEnd, tex, col, spriteBatch, thickness, rotation, origin); //left bar 
                DrawStraightLine(rightStart, rightEnd, tex, col, spriteBatch, thickness, rotation, origin); //right bar 
            }
            else
            {
                //var c = new Vector2(rec.X + rec.Width/2f, rec.Y + rec.Height/2f);
                spriteBatch.Draw(tex, Position, rec, col, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
            }

        }
        //draws a line (rectangle of thickness) from A to B.  A and B have make either horiz or vert line. 
        public static void DrawStraightLine(Vector2 A, Vector2 B, Texture2D tex, Color col, SpriteBatch spriteBatch, int thickness,float rotation,Vector2 origin)
        {
            Rectangle rec;
            
            if (A.X < B.X) // horiz line 
            {
                rec = new Rectangle((int)A.X, (int)A.Y, (int)(B.X - A.X), thickness);
            }
            else //vert line 
            {
                rec = new Rectangle((int)A.X, (int)A.Y, thickness, (int)(B.Y - A.Y));
            }

            spriteBatch.Draw(tex, rec,col);
        }

        public static Random random = new Random(100);
        private void CreateExhaustParticles()
        {
            var rand = random.Next(0, ContentProvider.ExhaustParticles.Count - 1);
            var particleTexture = ContentProvider.ExhaustParticles[rand];


            var exhuastPoint = new Vector2(position.X+texture.Width/2f,position.Y+texture.Height);
            var origin = new Vector2(position.X+texture.Width/2f,position.Y+texture.Height/2f);
            exhuastPoint = RotateAboutOrigin(exhuastPoint, origin,rotation);

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

        public static Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin,float rotation)
        {
            return Vector2.Transform(point,
                Matrix.CreateTranslation(-origin.X, -origin.Y, 0f)*
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(origin.X,origin.Y,0f)
                );
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //var firePoint = new Vector2(Center.X - laser.Width/2f, Center.Y+10);


            DrawRectangle(BoundingBox, ContentProvider.OutlineTexture, Color.Purple, spriteBatch, false, 1,rotation,new Vector2(position.X,position.Y));
            foreach (var currentParticle in CurrentParticles)
            {
                currentParticle.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
            //spriteBatch.Draw(ContentProvider.ExhaustParticles[0], firePoint, Color.Black);
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
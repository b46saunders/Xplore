using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public abstract class Ship : Sprite, ICollisionEntity
    {
        protected float Mass = 10000f;
        protected double LastCollisionTime;
        protected double CollisionMillisecondInterval = 100;
        protected Vector2 Velocity = Vector2.Zero;
        public int HealthPoints;
        public int MaxHealthPoints;
        protected bool Colliding;
        protected Vector2 DirectionVector;
        protected Vector2 DirectionGoalVector;
        protected Vector2 VelocityGoal;
        protected static Random Random = new Random(100);
        public CollisionType CollisionsWith =>CollisionType.Ship;
        public bool Active => true;
        public Guid Guid { get; }
        
        public Rectangle BoundingBox => BoundingCircle.SourceRectangle;
        public Circle BoundingCircle => new Circle(Center,texture.Height/2f);
        protected float RotationSpeed = 0.95f;
        protected float Speed = 4f;
        protected double LastFire = 0;
        protected readonly List<IParticle> CurrentParticles = new List<IParticle>();
        public List<Lazer> LazerBullets = new List<Lazer>(); 
        protected MouseState PreviousMouseState;
        protected KeyboardState PreviousKeyboardState;
        protected ShipType ShipType;
        


        protected Ship(Texture2D texture, Vector2 position,ShipType shipType) : base(texture, position)
        {
            ShipType = shipType;
            Guid = Guid.NewGuid();
            MaxHealthPoints = 10;
            HealthPoints = 10;
        }

        public Vector2 GetVelocity()
        {
            return Velocity;
        }

        public void CleanupParticles()
        {
            var particleArray = CurrentParticles.ToArray();
            for (int i = 0; i < particleArray.Length; i++)
            {
                if (!particleArray[i].IsParticleActive)
                {
                    if (particleArray[i].GetType() == typeof (Lazer))
                    {
                        LazerBullets.Remove((Lazer) particleArray[i]);
                    }
                    CurrentParticles.Remove(particleArray[i]);
                }
            }
        }

        public void ResolveSphereCollision(Vector2 mtdVector)
        {
            //calculate the max speed at which the ship should experience speed loss due to friction
            //max speed will just be the speed of the ship...
            if (Velocity.Length() > Speed * 0.5f)
            {
                var newVelocity = new Vector2(Velocity.X,Velocity.Y);
                newVelocity.Normalize();
                newVelocity = newVelocity*Speed*0.5f;
                VelocityGoal = newVelocity;
            }

            position += mtdVector;
        }

        public void ApplyCollisionDamage(GameTime gametime, int damage)
        {
            if (ShipType != ShipType.Player && gametime.TotalGameTime.TotalMilliseconds > LastCollisionTime + CollisionMillisecondInterval)
            {
                LastCollisionTime = gametime.TotalGameTime.TotalMilliseconds;
                HealthPoints -= damage;
                MathHelper.Clamp(HealthPoints, 0, MaxHealthPoints);
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
                Velocity.X = 0;
            }
            if (mtd.Y < 0 || mtd.Y > 0)
            {
                Velocity.Y = 0;
            }
            position += mtd;
            return mtd;

        }

        protected void Fire()
        {
            var lazerPos = new Vector2(Center.X - ContentProvider.Laser.Width/2f,
                Center.Y - ContentProvider.Laser.Height/2f);
            lazerPos = new Vector2(Center.X - ContentProvider.Laser.Width/2f, Center.Y - ContentProvider.Laser.Height).RotateAboutOrigin(lazerPos, Rotation);
            var lazer = new Lazer(ContentProvider.Laser,lazerPos,DirectionVector, 2000f) {IsParticleActive = true};
            LazerBullets.Add(lazer);
            CurrentParticles.Add(lazer);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            //DrawHelper.DrawRectangle(BoundingBox, ContentProvider.OutlineTexture, Color.Purple, spriteBatch, false, 1, rotation, new Vector2(position.X, position.Y));
            //spriteBatch.Draw(ContentProvider.CollsionSphereTexture,BoundingCircle.SourceRectangle,Color.White);
            foreach (var currentParticle in CurrentParticles)
            {
                currentParticle.Draw(spriteBatch);
            }
            if (HealthPoints > 0)
            {
                base.Draw(spriteBatch);
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            
            PreviousMouseState = Mouse.GetState();
            PreviousKeyboardState = Keyboard.GetState();
            CleanupParticles();
            foreach (var currentParticle in CurrentParticles)
            {
                currentParticle.Update(gameTime);
            }
           
            position = position + Velocity;
            base.Update(gameTime);
        }

        protected void CreateSideExhaustParticles(Vector2 exhaustDirection,Vector2 exhaustPoint)
        {
            var rand = Random.Next(0, ContentProvider.SideExhaustParticles.Count - 1);
            var particleTexture = ContentProvider.SideExhaustParticles[rand];
            //var exhuastPoint = new Vector2(position.X + texture.Width / 2f, position.Y + texture.Height);
            var origin = new Vector2(position.X + texture.Width / 2f, position.Y + texture.Height / 2f);
            exhaustPoint = exhaustPoint.RotateAboutOrigin(origin, Rotation);

            var spread = 30;
            for (int i = 0; i < 1; i++)
            {
                var x = (exhaustDirection.X + Random.Next(-spread, spread) / 100f);
                var y = (exhaustDirection.Y + Random.Next(-spread, spread) / 100f);
                var randomVector = new Vector2(x, y);
                randomVector.Normalize();

                CurrentParticles.Add(new ShipExhaust(particleTexture, exhaustPoint, randomVector));
            }
        }

        protected void CreateExhaustParticles()
        {
            var rand = Random.Next(0, ContentProvider.ExhaustParticles.Count - 1);
            var particleTexture = ContentProvider.ExhaustParticles[rand];


            var exhuastPoint = new Vector2(position.X + texture.Width / 2f, position.Y + texture.Height);
            var origin = new Vector2(position.X + texture.Width / 2f, position.Y + texture.Height / 2f);
            exhuastPoint = exhuastPoint.RotateAboutOrigin(origin, Rotation);

            var spread = 30;
            for (int i = 0; i < 4; i++)
            {

                var particleDirection = -DirectionVector;
                var x = (particleDirection.X + Random.Next(-spread, spread) / 100f);
                var y = (particleDirection.Y + Random.Next(-spread, spread) / 100f);
                var randomVector = new Vector2(x, y);
                randomVector.Normalize();

                CurrentParticles.Add(new ShipExhaust(particleTexture, exhuastPoint, randomVector));
            }

        }

    }
}
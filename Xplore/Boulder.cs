using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class Boulder : Sprite, ICollisionEntity
    {
        private readonly HealthBar _healthBar;
        public CollisionType CollisionsWith => CollisionType.Boulder;
        public bool Active => active;
        private bool active = true;
        public Guid Guid { get; }
        public Rectangle BoundingBox => BoundingCircle.SourceRectangle;
        public Circle BoundingCircle => new Circle(Center, (texture.Height / 2f)-20f);
        public int HealthPoints;
        public int MaxHealthPoints;
        protected Vector2 Velocity = Vector2.Zero;
        protected Vector2 DirectionVector;
        protected float RotationSpeed;
        protected float Speed = .14f;
        

        public Boulder(Texture2D texture, Vector2 position) : base(texture, position)
        {
            _healthBar = new HealthBar(new BoulderHealthBar(this));
            RotationSpeed = (float)ContentProvider.Random.Next(1, 3)/1000;
            Speed = Speed*(float)ContentProvider.Random.NextDouble();
            Rotation = MathHelper.WrapAngle(ContentProvider.Random.Next(0, 360));
            //now that we have a random rotation
            var newDirectionVector = Vector2DEx.GetVectorFromAngle(Rotation);
            newDirectionVector.Normalize();
            DirectionVector = newDirectionVector;
            


            Guid = Guid.NewGuid();
            HealthPoints = 50;
            MaxHealthPoints = 50;
        }

        public Vector2 GetVelocity()
        {
            return Velocity;
        }

        public override void Update(GameTime gameTime)
        {

            Rotation = MathHelper.WrapAngle(Rotation + RotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            Velocity = DirectionVector*Speed*(float)gameTime.ElapsedGameTime.TotalMilliseconds;
            position += Velocity;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(ContentProvider.CollsionSphereTexture,BoundingCircle.SourceRectangle,Color.White);
            _healthBar.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public void ResolveSphereCollision(Vector2 mtdVector)
        {
            //calculate the max speed at which the ship should experience speed loss due to friction
            //max speed will just be the speed of the ship...
            //if (Velocity.Length() > Speed * 0.5f)
            //{
            //    var newVelocity = new Vector2(Velocity.X, Velocity.Y);
            //    newVelocity.Normalize();
            //    newVelocity = newVelocity * Speed * 0.3f;
            //    Velocity = newVelocity;
            //}

            //position += mtdVector;
        }

        public void ApplyCollisionDamage(GameTime gametime, int damage)
        {
            
        }
    }
}
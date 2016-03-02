using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public abstract class Sprite
    {
        protected Body Body;
        protected Texture2D texture;
        protected Vector2 position;
        protected float rotation;
        protected MouseState previousMouseState;
        protected KeyboardState previousKeyboardState;
        protected Vector2 velocity = Vector2.Zero;
        protected bool HasPhysics;

        public Vector2 Center => new Vector2(position.X + texture.Width / 2f, position.Y + texture.Height / 2f);


        protected Sprite(Texture2D texture, Vector2 position, bool physics = false)
        {
            this.texture = texture;
            this.position = position;
            this.HasPhysics = physics;
            if (physics)
            {
                Body = BodyFactory.CreateRectangle(GameWorld.World, (float)ConvertUnits.ToSimUnits(texture.Width),
                    (float)ConvertUnits.ToSimUnits(texture.Height), 1.0f);
                Body.Mass = 0.1f;
                Body.BodyType = BodyType.Dynamic;
                Body.Restitution = 0.05f;
                Body.Position = ConvertUnits.ToSimUnits(new Vector2(position.X,position.Y));
            }

            //Body.CollidesWith = Category.All;

        }

        public virtual void Update(GameTime gameTime)
        {
            if (HasPhysics)
            {
                Body.ApplyForce(velocity,Body.WorldCenter);
            }

            
            position = position + velocity;
            previousMouseState = Mouse.GetState();
            previousKeyboardState = Keyboard.GetState();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,ConvertUnits.ToDisplayUnits(Body.Position), null, Color.White, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0f);
        }


    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public abstract class Sprite
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected float rotation;
        protected MouseState previousMouseState;
        protected KeyboardState previousKeyboardState;
        protected Vector2 velocity = Vector2.Zero;

        //expose getters so that we can access externaly if necessary
        public Texture2D Texture => texture;
        public Vector2 Position => position;
        public KeyboardState PreviousKeyboardState => previousKeyboardState;
        public Vector2 Velocity => velocity;

        public Vector2 Center => new Vector2(position.X + texture.Width / 2f, position.Y + texture.Height / 2f);
        

        protected Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public virtual void Update(GameTime gameTime)
        {
            position = position + velocity;
            previousMouseState = Mouse.GetState();
            previousKeyboardState = Keyboard.GetState();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position,null,Color.White,rotation, new Vector2(texture.Width/2f,texture.Height/2f), 1f,SpriteEffects.None,0f);
        }


    }
}

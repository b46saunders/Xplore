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
        private float HalfTextureWidth;
        private float HalfTextureHeight;
        

        //expose getters so that we can access externaly if necessary
        public Texture2D Texture => texture;
        public Vector2 Position => position;

        public Vector2 Center => new Vector2(position.X + HalfTextureWidth, position.Y + HalfTextureHeight);
        

        protected Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            HalfTextureWidth = texture.Width/2f;
            HalfTextureHeight = texture.Height/2f;
            this.position = position;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X+texture.Width/2f,position.Y+texture.Height/2f),null,Color.White,rotation, new Vector2(texture.Width/2f,texture.Height/2f), 1f,SpriteEffects.None,0f);
        }


    }
}

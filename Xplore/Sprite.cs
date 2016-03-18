using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public abstract class Sprite
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected float Rotation;
        private readonly float _halfTextureWidth;
        private readonly float _halfTextureHeight;
        

        //expose getters so that we can access externaly if necessary
        public Texture2D Texture => texture;
        public Vector2 Position => position;

        public Vector2 Center => new Vector2(position.X + _halfTextureWidth, position.Y + _halfTextureHeight);


        protected Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            _halfTextureWidth = texture.Width/2f;
            _halfTextureHeight = texture.Height/2f;
            this.position = position;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X+texture.Width/2f,position.Y+texture.Height/2f),null,Color.White,Rotation, new Vector2(texture.Width/2f,texture.Height/2f), 1f,SpriteEffects.None,0f);
        }


    }
}

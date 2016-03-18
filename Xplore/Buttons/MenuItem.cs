using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore.Buttons
{
    public abstract class MenuItem
    {
        public Vector2 Position { get; set; }

        protected MenuItem(Vector2 position)
        {
            Position = position;
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}
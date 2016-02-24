using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class Enemy : Sprite, IShip
    {
        public Enemy(Texture2D texture, Vector2 position) : base(texture, position)
        {

        }
        

        public Rectangle BoundingBox => new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
    }
}
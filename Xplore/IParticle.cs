using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public interface IParticle
    {
        Vector2 Origin { get; set; }
        bool IsActive { get; set; }
        void Update(GameTime gametime);
        void Draw(SpriteBatch spriteBatch);
    }
}
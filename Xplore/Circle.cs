using Microsoft.Xna.Framework;

namespace Xplore
{
    public class Circle
    {
        public float Radius { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle SourceRectangle
            => new Rectangle((int) (Position.X - Radius),
                (int) (Position.Y - Radius), (int) (Radius*2),
                (int) (Radius*2));

        public Circle(Vector2 position,float radius)
        {
            Radius = radius;
            Position = position;
        }

    }
}
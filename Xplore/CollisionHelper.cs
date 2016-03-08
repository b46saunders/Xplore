using Microsoft.Xna.Framework;

namespace Xplore
{
    public static class CollisionHelper
    {
        public static bool IsCircleColliding(Circle boundingCircle, Circle collidingWith, out Vector2 collisionVector)
        {
            var localBoundingCircle = boundingCircle;
            collisionVector = new Vector2(0, 0);
            var dx = localBoundingCircle.Position.X - collidingWith.Position.X;
            var dy = localBoundingCircle.Position.Y - collidingWith.Position.Y;

            var vector = new Vector2(dx, dy);
            var vectorLength = vector.Length();
            if (vectorLength < localBoundingCircle.Radius + collidingWith.Radius)
            {
                var mtdVectorLength = localBoundingCircle.Radius + collidingWith.Radius - vectorLength;
                vector.Normalize();
                var mtdVector = vector * mtdVectorLength;
                collisionVector = mtdVector;
                return true;
            }
            return false;
        }
    }
}

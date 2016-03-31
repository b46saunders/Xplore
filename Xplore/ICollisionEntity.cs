using System;
using Microsoft.Xna.Framework;

namespace Xplore
{
    public interface ICollisionEntity
    {
        CollisionType CollisionsWith { get; }
        bool Active { get; }
        Guid Guid { get; }
        Rectangle BoundingBox { get;}
        Circle BoundingCircle { get; }
        void ResolveSphereCollision(Vector2 mtdVector);
        void ApplyCollisionDamage(GameTime gametime,int damage);

    }
}
using System;
using Microsoft.Xna.Framework;

namespace Xplore
{
    public static class Vector2DEx
    {

        public static Vector2 GetVectorFromAngle(double angle)
        {
            return new Vector2((float)Math.Cos(angle),(float)Math.Sin(angle));    
        }

        public static double GetRotationFromVector(this Vector2 unitLengthVector)
        {
            return (float)Math.Atan2(unitLengthVector.X, -unitLengthVector.Y);
        }

        public static Vector2 RotateAboutOrigin(this Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point,
                Matrix.CreateTranslation(-origin.X, -origin.Y, 0f) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(origin.X, origin.Y, 0f)
                );
        }

    }
}
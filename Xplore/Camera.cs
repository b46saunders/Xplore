using Microsoft.Xna.Framework;

namespace Xplore
{
    
    public static class Camera
    {
        public static float Zoom { get; set; }
        public static Vector2 Location { get; set; }
        public static float Rotation { get; set; }
        public static Rectangle Bounds { get; set; }

        static Camera()
        {
            Zoom = 1f;
            Rotation = 0f;
        }

        public static Matrix TransformMatrix()
        {
            return
                 Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0)) *
                 Matrix.CreateRotationZ(Rotation) *
                 Matrix.CreateScale(Zoom) *
                 Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));

        }

        public static Vector2 GetScreenPosition(Vector2 location)
        {
            return Vector2.Transform(location, TransformMatrix());
        }

        public static Vector2 GetWorldPosition(Vector2 location)
        {
            return Vector2.Transform(location, Matrix.Invert(TransformMatrix()));
            
        }
    }

}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public static class DrawHelper
    {
        public static void DrawRectangle(Rectangle rec, Texture2D tex, Color col, SpriteBatch spriteBatch, bool solid, int thickness, float rotation, Vector2 origin)
        {
            Vector2 Position = new Vector2(rec.X, rec.Y);
            if (!solid)
            {
                int border = thickness;

                int borderWidth = (int)(rec.Width) + (border * 2);
                int borderHeight = (int)(rec.Height) + (border);

                //now we need to rotate all the vectors...
                var topStart = new Vector2((int)rec.X, (int)rec.Y);
                var topEnd = new Vector2((int)rec.X + rec.Width, (int)rec.Y);
                var bottomStart = new Vector2((int)rec.X, (int)rec.Y + rec.Height);
                var bottomEnd = new Vector2((int)rec.X + rec.Width, (int)rec.Y + rec.Height);
                var leftStart = new Vector2((int)rec.X, (int)rec.Y);
                var leftEnd = new Vector2((int)rec.X, (int)rec.Y + rec.Height);
                var rightStart = new Vector2((int)rec.X + rec.Width, (int)rec.Y);
                var rightEnd = new Vector2((int)rec.X + rec.Width, (int)rec.Y + rec.Height);

                DrawStraightLine(topStart, topEnd, tex, col, spriteBatch, thickness, rotation, origin); //top bar 
                DrawStraightLine(bottomStart, bottomEnd, tex, col, spriteBatch, thickness, rotation, origin); //bottom bar 
                DrawStraightLine(leftStart, leftEnd, tex, col, spriteBatch, thickness, rotation, origin); //left bar 
                DrawStraightLine(rightStart, rightEnd, tex, col, spriteBatch, thickness, rotation, origin); //right bar 
            }
            else
            {
                //var c = new Vector2(rec.X + rec.Width/2f, rec.Y + rec.Height/2f);
                spriteBatch.Draw(tex, Position, rec, col, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
            }

        }
        //draws a line (rectangle of thickness) from A to B.  A and B have make either horiz or vert line. 
        public static void DrawStraightLine(Vector2 A, Vector2 B, Texture2D tex, Color col, SpriteBatch spriteBatch, int thickness, float rotation, Vector2 origin)
        {
            Rectangle rec;

            if (A.X < B.X) // horiz line 
            {
                rec = new Rectangle((int)A.X, (int)A.Y, (int)(B.X - A.X), thickness);
            }
            else //vert line 
            {
                rec = new Rectangle((int)A.X, (int)A.Y, thickness, (int)(B.Y - A.Y));
            }

            spriteBatch.Draw(tex, rec, col);
        }

    }
}

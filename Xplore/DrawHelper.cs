using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public static class DrawHelper
    {
        public static void DrawRectangle(Rectangle rec, Texture2D tex, Color col, SpriteBatch spriteBatch, bool solid, int thickness, float rotation, Vector2 origin)
        {
            if (!solid)
            {
                //now we need to rotate all the vectors...
                var topStart = new Vector2(rec.X, rec.Y);
                var topEnd = new Vector2(rec.X + rec.Width, rec.Y);
                var bottomStart = new Vector2(rec.X, rec.Y + rec.Height);
                var bottomEnd = new Vector2(rec.X + rec.Width, rec.Y + rec.Height);
                var leftStart = new Vector2(rec.X, rec.Y);
                var leftEnd = new Vector2(rec.X, rec.Y + rec.Height);
                var rightStart = new Vector2(rec.X + rec.Width, rec.Y);
                var rightEnd = new Vector2(rec.X + rec.Width, rec.Y + rec.Height);

                DrawStraightLine(topStart, topEnd, tex, col, spriteBatch, thickness, rotation, origin); //top bar 
                DrawStraightLine(bottomStart, bottomEnd, tex, col, spriteBatch, thickness, rotation, origin); //bottom bar 
                DrawStraightLine(leftStart, leftEnd, tex, col, spriteBatch, thickness, rotation, origin); //left bar 
                DrawStraightLine(rightStart, rightEnd, tex, col, spriteBatch, thickness, rotation, origin); //right bar 
            }
            else
            {
                //var c = new Vector2(rec.X + rec.Width/2f, rec.Y + rec.Height/2f);
                spriteBatch.Draw(tex, rec, col);
            }
        }

        //draws a line (rectangle of thickness) from A to B.  A and B have make either horiz or vert line. 
        public static void DrawStraightLine(Vector2 a, Vector2 b, Texture2D tex, Color col, SpriteBatch spriteBatch, int thickness, float rotation, Vector2 origin)
        {
            Rectangle rec;

            if (a.X < b.X) // horiz line 
            {
                rec = new Rectangle((int)a.X, (int)a.Y, (int)(b.X - a.X), thickness);
            }
            else //vert line 
            {
                rec = new Rectangle((int)a.X, (int)a.Y, thickness, (int)(b.Y - a.Y));
            }

            spriteBatch.Draw(tex, rec, col);
        }

    }
}

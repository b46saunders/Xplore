using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore.Buttons
{
    public class MenuButton : MenuItem
    {
        public Texture2D Texture { get; set; }
        public Texture2D MouseOverTexture { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Camera.GetWorldPosition(Position).X, (int)Camera.GetWorldPosition(Position).Y, (int)((float)Texture.Width/Camera.Zoom), (int)((float)Texture.Height / Camera.Zoom));
        private bool pressed = false;
        private bool previousUpdateMouseDown = false;
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        

        public event EventHandler ClickEvent;

        public ScreenManager ScreenManager { get; set; }

        public MenuButton(Vector2 position, Texture2D texture, Texture2D mouseOverTexture, string text, SpriteFont font) : base(position)
        {
            Texture = texture;
            MouseOverTexture = mouseOverTexture;
            Text = text;
            Font = font;

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Debug.WriteLine(Position);
            //we want to make sure the position is trasformed by the camera correctly
            var worldPosition = Camera.GetWorldPosition(Position);
            var stringSize = Font.MeasureString(Text);
            var fontXpos = worldPosition.X + (Texture.Width / 2f) ;
            var fontYpos = worldPosition.Y + (Texture.Height / 2f);
            fontXpos = fontXpos - stringSize.X / 2f;
            fontYpos = fontYpos - stringSize.Y / 2f;


            if (pressed)
            {
                DrawButton(MouseOverTexture, spriteBatch, worldPosition);
            }
            else
            {
                DrawButton(Texture, spriteBatch, worldPosition);
            }
            spriteBatch.DrawString(Font, Text, new Vector2(fontXpos, fontYpos), Color.Black, 0f, Vector2.Zero, 1f/Camera.Zoom, SpriteEffects.None, 0f);

        }

        private void DrawButton(Texture2D texture, SpriteBatch spriteBatch,Vector2 position)
        {
            spriteBatch.Draw(texture,position, null, Color.White, 0f, Vector2.Zero, 1f/Camera.Zoom, SpriteEffects.None, 1f);
        }

        public override void Update(GameTime gameTime)
        {

            var mouseState = Mouse.GetState();
            var mousePos = new Vector2(mouseState.X,mouseState.Y);
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (BoundingBox.Intersects(new Rectangle((int)Camera.GetWorldPosition(mousePos).X, (int)Camera.GetWorldPosition(mousePos).Y, 1, 1)))
                {
                    pressed = true;
                }
                else
                {
                    pressed = false;
                }
                previousUpdateMouseDown = true;
            }

            //click
            if (mouseState.LeftButton == ButtonState.Released && previousUpdateMouseDown)
            {

                if (pressed)
                {
                    ClickEvent?.Invoke(this, null);
                    //Debug.WriteLine("CLICK ON BUTTON");
                }
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                previousUpdateMouseDown = false;
            }


        }
    }
}
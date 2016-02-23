using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class MenuButton : MenuItem
    {
        public Texture2D Texture { get; set; }
        public Texture2D MouseOverTexture { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
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

            var stringSize = Font.MeasureString(Text);
            var fontXpos = Position.X + Texture.Width / 2f;
            var fontYpos = Position.Y + Texture.Height / 2f;
            fontXpos = fontXpos - stringSize.X / 2f;
            fontYpos = fontYpos - stringSize.Y / 2f;


            if (pressed)
            {
                DrawButton(MouseOverTexture, spriteBatch);


            }
            else
            {
                DrawButton(Texture, spriteBatch);
            }
            spriteBatch.DrawString(Font, Text, new Vector2(fontXpos, fontYpos), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }

        private void DrawButton(Texture2D texture, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public override void Update(GameTime gameTime)
        {


            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (BoundingBox.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1)))
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
                    Debug.WriteLine("CLICK ON BUTTON");
                }
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                previousUpdateMouseDown = false;
            }


        }
    }
}
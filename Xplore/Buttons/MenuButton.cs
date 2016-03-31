using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore.Buttons
{
    public class MenuButton : MenuItem
    {
        public Texture2D Texture { get; set; }
        public Texture2D ButtonPressedTexture { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Camera.GetWorldPosition(Position).X, (int)Camera.GetWorldPosition(Position).Y, (int)(Width/Camera.Zoom), (int)(Height / Camera.Zoom));
        private bool _pressed;
        private bool _previousUpdateMouseDown;
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public SpriteFont Font { get; set; }
        

        public event EventHandler ClickEvent;

        public MenuButton(Vector2 position, Texture2D texture, Texture2D buttonPressedTexture, string text, SpriteFont font) : base(position)
        {
            Height = 100;
            Width = 200;
            Texture = texture;
            ButtonPressedTexture = buttonPressedTexture;
            Text = text;
            Font = font;

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //we want to make sure the position is trasformed by the camera correctly
            var worldPosition = Camera.GetWorldPosition(Position);
            var stringSize = Font.MeasureString(Text);
            var fontXpos = worldPosition.X + (Width / 2f) ;
            var fontYpos = worldPosition.Y + (Height / 2f);
            fontXpos = fontXpos - stringSize.X / 2f;
            fontYpos = fontYpos - stringSize.Y / 2f;


            if (_pressed)
            {
                DrawButton(ButtonPressedTexture, spriteBatch, worldPosition);
            }
            else
            {
                DrawButton(Texture, spriteBatch, worldPosition);
            }
            spriteBatch.DrawString(Font, Text, new Vector2(fontXpos, fontYpos), Color.Black, 0f, Vector2.Zero, 1f/Camera.Zoom, SpriteEffects.None, 0f);

        }

        private void DrawButton(Texture2D texture, SpriteBatch spriteBatch,Vector2 position)
        {
            spriteBatch.Draw(texture,null,new Rectangle((int)position.X,(int)position.Y,Width,Height), null, Vector2.Zero,0f,new Vector2(1f / Camera.Zoom), Color.White , SpriteEffects.None, 1f);
        }

        public override void Update(GameTime gameTime)
        {

            var mouseState = Mouse.GetState();
            var mousePos = new Vector2(mouseState.X,mouseState.Y);
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (BoundingBox.Intersects(new Rectangle((int)Camera.GetWorldPosition(mousePos).X, (int)Camera.GetWorldPosition(mousePos).Y, 1, 1)))
                {
                    _pressed = true;
                }
                else
                {
                    _pressed = false;
                }
                _previousUpdateMouseDown = true;
            }

            //click
            if (mouseState.LeftButton == ButtonState.Released && _previousUpdateMouseDown)
            {

                if (_pressed)
                {
                    ClickEvent?.Invoke(this, null);
                    //Debug.WriteLine("CLICK ON BUTTON");
                }
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                _previousUpdateMouseDown = false;
            }


        }
    }
}
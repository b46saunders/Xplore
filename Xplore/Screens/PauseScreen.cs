using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class PauseScreen : Screen
    {
        private MenuButton _resumeButton;
        private MenuButton _menuButton;
        public event EventHandler ResumeClick;
        public event EventHandler MenuClick;
        private readonly List<MenuButton> _menuButtons = new List<MenuButton>();

        public override void LoadContent()
        {
            //get center of screen
            var buttonTexture = Content.ButtonTexture;
            var mouseOverTexture = Content.MouseOverTexture;
            var pos = new Vector2(100, 100);
            var pos2 = new Vector2(100, pos.Y + buttonTexture.Height + 20);
            var font = Content.SpriteFont;

            _resumeButton = new MenuButton(pos, buttonTexture, mouseOverTexture, "RESUME", font);
            _menuButton = new MenuButton(pos2, buttonTexture, mouseOverTexture, "MENU", font);

            _menuButton.ClickEvent += MenuClick;
            _resumeButton.ClickEvent += ResumeClick;
            _menuButtons.Add(_menuButton);
            _menuButtons.Add(_resumeButton);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var menuButton in _menuButtons)
            {
                menuButton.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var menuButton in _menuButtons)
            {
                menuButton.Draw(spriteBatch, gameTime);
            }
        }


        public PauseScreen(bool active, ContentProvider content, Main game, ScreenManager screenManager) : base(active, content, game, screenManager)
        {
            ScreenType = ScreenType.Pause;

        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xplore.Buttons;

namespace Xplore
{
    public class MenuSceen : Screen
    {
        private readonly List<MenuButton> _menuButtons = new List<MenuButton>();
        public event EventHandler StartSinglePlayerGame;
        public event EventHandler EndGame;
        private MenuButton _singleplayer;
        private MenuButton _endButton;

        public override void LoadContent()
        {
            //get center of screen
            var buttonTexture = ContentProvider.ButtonTexture;
            var mouseOverTexture = ContentProvider.MouseOverTexture;
            var pos = new Vector2(100, 100);
            var pos2 = new Vector2(100, pos.Y + buttonTexture.Height + 20);
            var pos3 = new Vector2(100, pos2.Y + buttonTexture.Height + 20);
            var font = ContentProvider.SpriteFont;

            _singleplayer = new MenuButton(pos, buttonTexture, mouseOverTexture, "Singleplayer", font);
            _endButton = new MenuButton(pos3, buttonTexture, mouseOverTexture, "Quit", font);

            _singleplayer.ClickEvent += StartSinglePlayerGame;
            _endButton.ClickEvent += EndGame;
            _menuButtons.Add(_singleplayer);
            _menuButtons.Add(_endButton);
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


        public MenuSceen(bool active, Main game, ScreenManager screenManager) : base(active, game, screenManager)
        {
            UserInterface = true;
            ScreenType = ScreenType.Menu;

        }
    }
}
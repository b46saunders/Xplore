using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public abstract class Screen
    {
        public bool Active { get; set; }
        public bool UserInterface { get; set; }
        public ScreenType ScreenType { get; protected set; }
        protected ScreenManager ScreenManager { get; set; }
        protected Main Game;


        protected Screen(bool active, Main game, ScreenManager screenManager)
        {
            Active = active;
            Game = game;
            ScreenManager = screenManager;
        }

        public abstract void LoadContent();
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}


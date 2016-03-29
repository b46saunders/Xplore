using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore.Screens
{
    public abstract class Screen
    {
        public bool Active { get; set; }
        public bool UserInterface { get; set; }
        public ScreenType ScreenType { get; protected set; }
        protected Main Game;


        protected Screen(bool active, Main game)
        {
            Active = active;
            Game = game;
        }

        public abstract void LoadContent();
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}


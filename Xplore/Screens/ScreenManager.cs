using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class ScreenManager
    {
        public Rectangle ScreenBounds { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public Dictionary<string, Screen> Screens { get; set; }
        public SpriteFont SpriteFont { get; set; }
        public ContentProvider ContentProvider { get; set; }
        public Main Game { get; set; }

        public ScreenManager()
        {
            Screens = new Dictionary<string, Screen>();
        }
        public void Init()
        {

            var menuScreen = new MenuSceen(true, ContentProvider, Game, this);
            var debugScreen = new DebugScreen(true, ContentProvider, Game, this);
            menuScreen.StartSinglePlayerGame += (sender, args) => StartSinglePlayerGame();
            menuScreen.EndGame += (sender, args) => QuitGame();
            var pauseScreen = new PauseScreen(false, ContentProvider, Game, this);
            pauseScreen.MenuClick += (sender, args) => MainMenu();
            pauseScreen.ResumeClick += (sender, args) => ResumeGame();
            Screens.Add("Menu", menuScreen);
            Screens.Add("Pause", pauseScreen);
            Screens.Add("Debug", debugScreen);
            //Screens.Add("Gameplay", gameplayScreen);
        }

        private void StartSinglePlayerGame()
        {
            Screens["Menu"].Active = false;
            Screens.Add("Gameplay", new GameplayScreen(true, ContentProvider, Game, this));
        }

        public void ResumeGame()
        {
            Screens["Pause"].Active = false;
            Screens["Gameplay"].Active = true;
        }

        public void MainMenu()
        {

            Screens["Pause"].Active = false;
            Screens["Menu"].Active = true;
            Screens["Gameplay"] = null;
            Screens.Remove("Gameplay");
        }

        public void PauseGame()
        {
            //get the pause menu
            var pauseMenu = Screens["Pause"];
            var activeScreen = GetActiveScreen();
            activeScreen.Active = false;
            if (pauseMenu != null)
            {
                pauseMenu.Active = true;
            }
        }

        private Screen GetActiveScreen()
        {
            return Screens.Values.First(a => a.Active);
        }

        public void StartGame()
        {
            RestartGame();

        }

        public void QuitGame()
        {
            Game.Exit();
        }

        public void RestartGame()
        {
            Screen screen;
            if (Screens.TryGetValue("Gameplay", out screen))
            {
                Screens.Remove("Gameplay");
            }
            screen = new GameplayScreen(true, ContentProvider, Game, this);
            screen.Active = true;
            Screens["Menu"].Active = false;
            Screens.Add("Gameplay", screen);
        }

        public void LoadContent()
        {
            foreach (var screen in Screens.Values)
            {
                screen.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Screens.Values.Count; i++)
            {
                if (Screens.Values.ElementAt(i).Active)
                {
                    Screens.Values.ElementAt(i).Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var screen in Screens.Values.OrderBy(screen => screen.ScreenType))
            {
                if (screen.Active)
                    screen.Draw(spriteBatch, gameTime);

            }
        }

        public void UnloadContent()
        {
            foreach (var screen in Screens.Values)
            {
                screen.UnloadContent();

            }
        }
    }
}
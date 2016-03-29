using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore.Screens
{
    public static class GameManager
    {
        public static Rectangle ScreenBounds { get; set; }
        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public static Dictionary<string, Screen> Screens { get; set; }
        public static Main Game { get; set; }
        

        static GameManager()
        {
            Screens = new Dictionary<string, Screen>();
        }
        public static void Init()
        {
            var menuScreen = new MenuSceen(true, Game);
            var debugScreen = new DebugScreen(true, Game);
            menuScreen.StartSinglePlayerGame += (sender, args) => StartSinglePlayerGame();
            menuScreen.EndGame += (sender, args) => QuitGame();
            var pauseScreen = new PauseScreen(false, Game);
            pauseScreen.MenuClick += (sender, args) => MainMenu();
            pauseScreen.ResumeClick += (sender, args) => ResumeGame();
            Screens.Add("Menu", menuScreen);
            Screens.Add("Pause", pauseScreen);
            Screens.Add("Debug", debugScreen);
            //Screens.Add("Gameplay", gameplayScreen);
        }

        public static DebugScreen GetDebugScreen()
        {
            return (DebugScreen)Screens["Debug"];
        }

        private static void StartSinglePlayerGame()
        {
            Screens["Menu"].Active = false;
            Screens.Add("Gameplay", new GameplayScreen(true, Game));
        }

        public static void ResumeGame()
        {
            Screens["Pause"].Active = false;
            Screens["Gameplay"].Active = true;
            Screens["Debug"].Active = true;
        }

        public static void MainMenu()
        {

            Screens["Pause"].Active = false;
            Screens["Menu"].Active = true;
            Screens["Gameplay"] = null;
            Screens.Remove("Gameplay");
        }

        public static void PauseGame()
        {
            //get the pause menu
            var pauseMenu = Screens["Pause"];
            var activeScreens = GetActiveScreens();
            foreach (var screen in activeScreens)
            {
                screen.Active = false;
            }
            if (pauseMenu != null)
            {
                pauseMenu.Active = true;
            }
        }

        public static IEnumerable<Screen> GetActiveScreens()
        {
            return Screens.Values.Where(a => a.Active);
        }

        public static void StartGame()
        {
            RestartGame();

        }

        public static void QuitGame()
        {
            Game.Exit();
        }

        public static void RestartGame()
        {
            Screen screen;
            if (Screens.TryGetValue("Gameplay", out screen))
            {
                Screens.Remove("Gameplay");
            }
            screen = new GameplayScreen(true, Game);
            screen.Active = true;
            Screens["Menu"].Active = false;
            Screens.Add("Gameplay", screen);
        }

        public static void LoadContent()
        {
            foreach (var screen in Screens.Values)
            {
                screen.LoadContent();
            }
        }

        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < Screens.Values.Count; i++)
            {
                var screen = Screens.Values.ElementAt(i);
                if (screen.Active)
                {
                    screen.Update(gameTime);
                    
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var screen in Screens.Values.OrderBy(screen => screen.ScreenType))
            {
                if (screen.Active)
                    screen.Draw(spriteBatch, gameTime);
                

            }
        }

        public static void UnloadContent()
        {
            foreach (var screen in Screens.Values)
            {
                screen.UnloadContent();

            }
        }
    }
}
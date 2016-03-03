using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class GameplayScreen : Screen
    {
        private Random rand = new Random();
        private List<Enemy> Enemies = new List<Enemy>();
        private MouseState previousMouseState;
        private Player player;
        private const int MaxEnemyCount = 10;
        
        private const int gameSize = 2000;
        private Rectangle gameBounds = new Rectangle(-(gameSize / 2), -(gameSize / 2), gameSize, gameSize);
        public override void LoadContent()
        {
            
        }

        public override void UnloadContent()
        {

        }

        public void SpawnEnemies()
        {
            //spawn an emeny if the count of total enemies is less than maxEnemyCount
            while (Enemies.Count < MaxEnemyCount)
            {
                float radius = (float)rand.Next(gameBounds.Width/2,gameBounds.Width)/2;
                float angle = (float)rand.NextDouble() * MathHelper.TwoPi;
                float x = player.Center.X + radius * (float)Math.Cos(angle);
                float y = player.Center.Y + radius * (float)Math.Sin(angle);

                Enemies.Add(new Enemy(ContentProvider.EnemyShips[rand.Next(ContentProvider.EnemyShips.Count - 1)],
                    new Vector2(x, y),
                    gameBounds));
            }


        }


        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                ScreenManager.PauseGame();

            Camera.Location = new Vector2(player.Position.X,player.Position.Y);

            var mouseState = Mouse.GetState();

            SpawnEnemies();

            previousMouseState = mouseState;

            player.Update(gameTime);
            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime);
                if ((player.Center - enemy.Center).Length() < 500)
                {
                    enemy.Seek(player.Center);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(ContentProvider.Background, new Vector2(gameBounds.X, gameBounds.Y), new Rectangle(gameBounds.X, gameBounds.Y, gameBounds.Width, gameBounds.Height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            player.Draw(spriteBatch);
            foreach (var enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
            
        }

        public GameplayScreen(bool active, Main game,
            ScreenManager screenManager)
            : base(active, game, screenManager)
        {
            UserInterface = false;
            ScreenType = ScreenType.Gameplay;
            Camera.Bounds = Game.GraphicsDevice.Viewport.Bounds;
            player = new Player(ContentProvider.Ship, new Vector2(0,0),gameBounds);
        }
    }
}
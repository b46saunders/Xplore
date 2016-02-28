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
        private const int gameSize = 1000;
        private Rectangle gameBounds = new Rectangle(-(gameSize / 2), -(gameSize / 2), gameSize, gameSize);
        public override void LoadContent()
        {
            
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                ScreenManager.PauseGame();

            Camera.Location = new Vector2(player.Position.X,player.Position.Y);

            var mouseState = Mouse.GetState();

            if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
            {
                Enemies.Add(new Enemy(ContentProvider.EnemyShips[rand.Next(ContentProvider.EnemyShips.Count-1)],Camera.GetWorldPosition(new Vector2(mouseState.X,mouseState.Y)),gameBounds));
            }


            previousMouseState = mouseState;

            player.Update(gameTime);
            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime);
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
            ScreenType = ScreenType.Gameplay;
            Camera.Bounds = Game.GraphicsDevice.Viewport.Bounds;
            player = new Player(ContentProvider.Ship, new Vector2(0,0),gameBounds);
        }
    }
}
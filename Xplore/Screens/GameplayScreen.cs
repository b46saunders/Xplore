using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class GameplayScreen : Screen
    {
        private Player player;
        private const int gameSize = 1000;
        private Rectangle gameBounds = new Rectangle(-(gameSize / 2), -(gameSize / 2), gameSize, gameSize);
        public override void LoadContent()
        {
            
        }

        public override void UnloadContent()
        {
            //throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                ScreenManager.PauseGame();
            
            //get the new players center after scale etc
            //var playerWorldPos = Camera.GetScreenPosition(new Vector2(player.Position.X, player.Position.Y));
            Camera.Location = new Vector2(player.Position.X,player.Position.Y);

            //Camera.CenterPosition = Camera.GetWorldPosition(mousePos);
            player.Update(gameTime);
            
            Debug.WriteLine($"{player.Position.X},{player.Position.Y}");
            //Camera.CenterPosition = new Vector2(player.Position.X - Game.GraphicsDevice.Viewport.Width/2f,
            //    player.Position.Y - Game.GraphicsDevice.Viewport.Height/2f);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
           //spriteBatch.Draw(Content.Background,Vector2.Zero,Color.White);
            spriteBatch.Draw(Content.Background, new Vector2(gameBounds.X, gameBounds.Y), new Rectangle(gameBounds.X, gameBounds.Y, gameBounds.Width, gameBounds.Height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            player.Draw(spriteBatch);
            //spriteBatch.Draw(Content.ExhaustParticles[0],Camera.GetWorldPosition(new Vector2(Game.GraphicsDevice.Viewport.Width/2f-(Content.ExhaustParticles[0].Width/2f),Game.GraphicsDevice.Viewport.Height/2f-(Content.ExhaustParticles[0].Height / 2f))));
        }

        public GameplayScreen(bool active, ContentProvider content, Main game,
            ScreenManager screenManager)
            : base(active, content, game, screenManager)
        {
            ScreenType = ScreenType.Gameplay;
            Camera.Bounds = Game.GraphicsDevice.Viewport.Bounds;
            player = new Player(Content.Ship, new Vector2(0,0),gameBounds,content.ExhaustParticles,content.Laser);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    /// <summary>
    /// TODO have a static member so we can access the debug screen anywhere in the program - helpful if we want to live debug stuff. maybe have some sort of handle that can be placed on the screen in a location specified
    /// include some functions for updating etc
    /// maybe just some hook into the update method for this class
    /// MOVE THIS TO BE DRAWN LAST!!!
    /// </summary>
    public class DebugScreen : Screen
    {
        private double _lastFrameSecond = 0;
        private double _lastUpdateSecond = 0;
        private double _frameCount = 0;
        private double _updateCount = 0;
        private double _previousUpdateCount = 0;
        private double _previousFrameCount = 0;
        private Vector2 mousePos = Vector2.Zero;
        private Vector2 worldPos = Vector2.Zero;

        public override void LoadContent()
        {
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            _updateCount++;
            if (gameTime.TotalGameTime.TotalSeconds > _lastUpdateSecond + 1)
            {
                _previousUpdateCount = _updateCount;
                _lastUpdateSecond = gameTime.TotalGameTime.TotalSeconds;
                _updateCount = 0;
            }
            var mouseState = Mouse.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;
            worldPos = Camera.GetWorldPosition(new Vector2(mousePos.X, mousePos.Y));
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //draw the update strings
            
            var updatePos = Camera.GetWorldPosition(new Vector2(10, 10));
            var fpsPos = new Vector2(updatePos.X, updatePos.Y + 16);
            var mousePoss = new Vector2(fpsPos.X, fpsPos.Y + 16);
            var worldPosz = new Vector2(mousePoss.X,mousePoss.Y + 16);
            var cameraPos = new Vector2(worldPosz.X,worldPosz.Y + 16);
            spriteBatch.DrawString(ContentProvider.SpriteFont, $"UPS : {_previousUpdateCount}", updatePos, Color.Black);
            spriteBatch.DrawString(ContentProvider.SpriteFont, $"FPS : {_previousFrameCount}", fpsPos, Color.Black);
            spriteBatch.DrawString(ContentProvider.SpriteFont,$"Mouse pos : [{mousePos.X},{mousePos.Y}]", mousePoss, Color.Black);
            spriteBatch.DrawString(ContentProvider.SpriteFont, $"World Pos : [{worldPos.X},{worldPos.Y}]", worldPosz, Color.Black);
            spriteBatch.DrawString(ContentProvider.SpriteFont,$"Camera Pos: [{Camera.Location.X},{Camera.Location.Y}]",cameraPos,Color.Black);
            _frameCount++;
            if (gameTime.TotalGameTime.TotalSeconds > _lastFrameSecond + 1)
            {
                _previousFrameCount = _frameCount;
                _lastFrameSecond = gameTime.TotalGameTime.TotalSeconds;
                _frameCount = 0;
            }
        }

        public DebugScreen(bool active, Main game, ScreenManager screenManager) : base(active, game, screenManager)
        {
            UserInterface = true;
            ScreenType = ScreenType.Debug;
        }
    }
}
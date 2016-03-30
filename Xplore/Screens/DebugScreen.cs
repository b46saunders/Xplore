using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore.Screens
{
    public class DebugScreen : Screen
    {
        private double _lastFrameSecond;
        private double _lastUpdateSecond;
        private double _frameCount;
        private double _updateCount;
        private double _previousUpdateCount;
        private double _previousFrameCount;
        private Vector2 _mousePos = Vector2.Zero;
        private Vector2 _worldPos = Vector2.Zero;

        private readonly List<DebugDisplayItem> _debugDisplayItems = new List<DebugDisplayItem>();

        public override void LoadContent()
        {
        }

        public override void UnloadContent()
        {
        }

        public void AddDebugData(DebugDisplayItem debugDisplayItem)
        {
            _debugDisplayItems.Add(debugDisplayItem);
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
            _mousePos.X = mouseState.X;
            _mousePos.Y = mouseState.Y;
            _worldPos = Camera.GetWorldPosition(new Vector2(_mousePos.X, _mousePos.Y));
        }

        private void InitDebugItems()
        {
            _debugDisplayItems.Add(new DebugDisplayItem(()=>$"UPS : {_previousUpdateCount}"));
            _debugDisplayItems.Add(new DebugDisplayItem(()=>$"FPS : {_previousFrameCount}"));
            _debugDisplayItems.Add(new DebugDisplayItem(() => $"Mouse pos : [{_mousePos.X},{_mousePos.Y}]"));
            _debugDisplayItems.Add(new DebugDisplayItem(() => $"World Pos : [{_worldPos.X},{_worldPos.Y}]"));
            _debugDisplayItems.Add(new DebugDisplayItem(() => $"Camera Pos: [{Camera.Location.X},{Camera.Location.Y}]"));
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            var yPosOffset = 0f;
            foreach (var debugDisplayItem in _debugDisplayItems)
            {
                var newPos = Camera.GetWorldPosition(new Vector2(10, 10));
                newPos.Y += yPosOffset;
                DrawDebugString(debugDisplayItem.Func(), newPos, spriteBatch);
                yPosOffset += (16f / Camera.Zoom);
            }

            _frameCount++;
            if (gameTime.TotalGameTime.TotalSeconds > _lastFrameSecond + 1)
            {
                _previousFrameCount = _frameCount;
                _lastFrameSecond = gameTime.TotalGameTime.TotalSeconds;
                _frameCount = 0;
            }
        }

        public void DrawDebugString(string text,Vector2 textposition,SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentProvider.SpriteFont, text, textposition, Color.Black, 0f, Vector2.Zero, 1f / Camera.Zoom, SpriteEffects.None, 0);
        }

        public DebugScreen(bool active, Main game) : base(active, game)
        {
            InitDebugItems();
            UserInterface = true;
            ScreenType = ScreenType.Debug;
        }
    }
}
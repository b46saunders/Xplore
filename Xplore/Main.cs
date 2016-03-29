using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xplore.Screens;

namespace Xplore
{
    public class Main : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public  float Timestep = 1000 / 250f;
        public  float ElapsedTime = 0f;
        private Rectangle _screenBounds;
        

        public Main()
        {
            IsFixedTimeStep = true;
            float dt = 1000 / 250f;
            
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            _graphics.SynchronizeWithVerticalRetrace = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(dt);
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ContentProvider.InitializeContent(Content);
            Camera.Bounds = _graphics.GraphicsDevice.Viewport.Bounds;
            Camera.Location = new Vector2(_graphics.GraphicsDevice.Viewport.Bounds.Width / 2f, _graphics.GraphicsDevice.Viewport.Bounds.Height / 2f);
            _screenBounds = new Rectangle(0, 0, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
            GameManager.ScreenBounds = _screenBounds;
            GameManager.Game = this;
            GameManager.Init();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            GameManager.LoadContent();
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            GameManager.UnloadContent();
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (ElapsedTime > Timestep)
            {
                GameManager.Update(gameTime);
                ElapsedTime -= Timestep;
                // TODO: Add your update logic here
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null,
                    null, null, Camera.TransformMatrix());
            
            GameManager.Draw(_spriteBatch,gameTime);
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

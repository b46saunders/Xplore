using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public  float timestep = 1000 / 250f;
        public  float elapsedTime = 0f;
        readonly ScreenManager _screenManager = new ScreenManager();
        private Rectangle screenBounds;
        

        public Main()
        {
            IsFixedTimeStep = true;
            float dt = 1000 / 250f;
            
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(dt);
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ContentProvider.InitializeContent(Content);
            Camera.Bounds = graphics.GraphicsDevice.Viewport.Bounds;
            Camera.Location = new Vector2(graphics.GraphicsDevice.Viewport.Bounds.Width / 2f, graphics.GraphicsDevice.Viewport.Bounds.Height / 2f);
            screenBounds = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            _screenManager.ScreenBounds = screenBounds;
            _screenManager.Game = this;
            _screenManager.Init();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _screenManager.LoadContent();
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            _screenManager.UnloadContent();
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime > timestep)
            {
                _screenManager.Update(gameTime);
                elapsedTime -= timestep;
                // TODO: Add your update logic here
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null,
                    null, null, Camera.TransformMatrix());
            
            _screenManager.Draw(spriteBatch,gameTime);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

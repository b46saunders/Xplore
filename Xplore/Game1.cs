using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public static class GameWorld
    {
        public static World World;

        static GameWorld()
        {
            World = new World(new Vector2(0, 0));
        }

    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
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

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _screenManager.LoadContent();
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _screenManager.UnloadContent();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime > timestep)
            {
                GameWorld.World.Step(0.004f);
                _screenManager.Update(gameTime);
                elapsedTime -= timestep;
                // TODO: Add your update logic here
            }
            

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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

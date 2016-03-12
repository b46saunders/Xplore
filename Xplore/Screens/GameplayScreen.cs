using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class GameplayScreen : Screen
    {
        private Random rand = new Random();
        private List<Enemy> Enemies = new List<Enemy>();
        private List<Boulder> Boulders = new List<Boulder>(); 
        private MouseState previousMouseState;
        private MouseState mouseState;
        private Player player;
        private SpatialGrid _spatialGrid;
        private const int MaxEnemyCount = 100;
        private const int MaxBoulderCount = 100;
        private Dictionary<string,ICollisionEntity> _collisionEntities = new Dictionary<string, ICollisionEntity>(); 

        private const int gameSize = 5000;
        private Rectangle gameBounds = new Rectangle(-(gameSize / 2), -(gameSize / 2), gameSize, gameSize);
        public override void LoadContent()
        {

        }

        public override void UnloadContent()
        {

        }

        private void EnemyShipDestroyed(object ship, EventArgs eventArgs)
        {
            _collisionEntities.Remove(((Enemy) ship).Guid.ToString());
             Enemies.Remove((Enemy)ship);
        }

        public void SpawnEnemies()
        {
            //spawn an emeny if the count of total enemies is less than maxEnemyCount
            while (Enemies.Count < MaxEnemyCount)
            {
                float radius = (float)rand.Next(gameBounds.Width / 2, gameBounds.Width) / 2;
                float angle = (float)rand.NextDouble() * MathHelper.TwoPi;
                float x = player.Center.X + radius * (float)Math.Cos(angle);
                float y = player.Center.Y + radius * (float)Math.Sin(angle);

                var enemy = new Enemy(ContentProvider.EnemyShips[rand.Next(ContentProvider.EnemyShips.Count)],
                    new Vector2(x, y),
                    ShipType.NpcEnemy);
                enemy.Wander();
                _collisionEntities.Add(enemy.Guid.ToString(),enemy);
                enemy.Destroyed += EnemyShipDestroyed;
                Enemies.Add(enemy);
            }


        }

        public void SpawnBoulders()
        {
            while (Boulders.Count < MaxBoulderCount)
            {
                float radius = (float)rand.Next(gameBounds.Width / 2, gameBounds.Width) / 2;
                float angle = (float)rand.NextDouble() * MathHelper.TwoPi;
                float x = player.Center.X + radius * (float)Math.Cos(angle);
                float y = player.Center.Y + radius * (float)Math.Sin(angle);

                var boulder = new Boulder(ContentProvider.Boulder,
                    new Vector2(x, y));
                _collisionEntities.Add(boulder.Guid.ToString(), boulder);
                Boulders.Add(boulder);
            }
        }



        public void ApplyMouseWheelZoom()
        {
            //zoom in/out
            if (mouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue)
            {
                Camera.Zoom += 0.05f;
            }
            if (mouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue)
            {
                Camera.Zoom -= 0.05f;
            }
        }

        public void UpdateGameBounds()
        {

            gameBounds = new Rectangle((int)(player.Position.X - gameSize/2f), (int)(player.Position.Y - gameSize/2f),gameSize,gameSize);
            
        }

        public override void Update(GameTime gameTime)
        {
            UpdateGameBounds();
            _spatialGrid = new SpatialGrid(gameBounds,200);
            
            mouseState = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                ScreenManager.PauseGame();
            ApplyMouseWheelZoom();
            Camera.Location = new Vector2(player.Position.X, player.Position.Y);
            SpawnEnemies();
            SpawnBoulders();
            player.Update(gameTime);

            var enemies = Enemies.ToArray();
            for (int i = 0; i < enemies.Length; i++)
            {
                var fleeDistance = 1000f;
                if ((player.Center - enemies[i].Center).Length() < fleeDistance)
                {
                    enemies[i].Flee(player.Center, fleeDistance);
                }
                enemies[i].Update(gameTime);
                
            }
            foreach (var boulder in Boulders)
            {
                boulder.Update(gameTime);
            }

            CheckAndResolveCollisions(gameTime);


            previousMouseState = mouseState;
        }

        /// <summary>
        /// we need to add a broad and narrow phase instead of checking O(n^2)
        /// </summary>
        private void CheckAndResolveCollisions(GameTime gameTime)
        {
            foreach (var collisionEntity in _collisionEntities.Values)
            {
                _spatialGrid.Insert(collisionEntity.BoundingCircle.SourceRectangle,collisionEntity);
            }

            foreach (var gridBox in _spatialGrid.GetCollsionGrid().Values)
            {
                if (gridBox.Count > 1)
                {
                    foreach (var collisionEntity in gridBox)
                    {
                        foreach (var entity in gridBox)
                        {
                            Vector2 collsionVector;
                            if (entity != collisionEntity && CollisionHelper.IsCircleColliding(entity.BoundingCircle,collisionEntity.BoundingCircle,out collsionVector))
                            {
                                _collisionEntities[entity.Guid.ToString()].ResolveSphereCollision(collsionVector);
                                _collisionEntities[entity.Guid.ToString()].ApplyCollisionDamage(gameTime);
                                _collisionEntities[collisionEntity.Guid.ToString()].ApplyCollisionDamage(gameTime);
                            }
                        }
                    }
                }
                
            }
            
            
            //bool collisionFound;
            ////do
            ////{
            //    //collisionFound = false;
            //    foreach (var ship in GetAllShips())
            //    {
            //        //check that this ship is not colliding with any other ships
            //        foreach (var checkShip in GetAllShips())
            //        {
            //            //if intersect and not the same ship
            //            Vector2 collsionVector;
            //            if (checkShip != ship && CollisionHelper.IsCircleColliding(checkShip.BoundingCircle,ship.BoundingCircle,out collsionVector))
            //            {
            //                checkShip.ResolveSphereCollision(collsionVector);
            //                checkShip.ApplyCollisionDamage(gameTime);
            //                ship.ApplyCollisionDamage(gameTime);
            //                //Debug.WriteLine("COLLSION!");
            //                //ship.ResolveCollision(checkShip.BoundingBox);
            //                //collisionFound = true;
            //            }
            //        }
            //    }

            ////} while (collisionFound);
        }


        //private void CheckAndResolveCollisions()
        //{
        //    bool collisionFound;
        //    //do
        //    //{
        //    collisionFound = false;
        //    foreach (var ship in GetAllShips())
        //    {
        //        //check that this ship is not colliding with any other ships
        //        foreach (var checkShip in GetAllShips())
        //        {
        //            //if intersect and not the same ship
        //            if (checkShip != ship && checkShip.BoundingBox.Intersects(ship.BoundingBox))
        //            {
        //                ship.ResolveCollision(checkShip.BoundingBox);
        //                collisionFound = true;
        //            }
        //        }
        //    }

        //    //} while (collisionFound);
        //}

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(ContentProvider.Background, new Vector2(gameBounds.X, gameBounds.Y), new Rectangle(gameBounds.X, gameBounds.Y, gameBounds.Width, gameBounds.Height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            player.Draw(spriteBatch);
            //_spatialGrid.RenderGrid(spriteBatch);
            foreach (var enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
            foreach (var boulder in Boulders)
            {
                boulder.Draw(spriteBatch);
            }

        }

        public GameplayScreen(bool active, Main game,
            ScreenManager screenManager)
            : base(active, game, screenManager)
        {
            UserInterface = false;
            ScreenType = ScreenType.Gameplay;
            Camera.Bounds = Game.GraphicsDevice.Viewport.Bounds;
            player = new Player(ContentProvider.Ship, new Vector2(0, 0),ShipType.Player);
            _collisionEntities.Add(player.Guid.ToString(),player);
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public static class ContentProvider
    {
        public static Random Random { get; set; }
        public static SpriteFont SpriteFont { get; set; }
        public static Texture2D ButtonTexture { get; set; }
        public static Texture2D ButtonPressedTexture { get; set; }
        public static Texture2D Ship { get; set; }
        public static Texture2D Background { get; set; }
        public static Texture2D Laser { get; set; }
        public static Texture2D Boulder { get; set; }
        public static List<Texture2D> ExhaustParticles { get; set; }
        public static List<Texture2D> EnemyShips { get; set; }
        public static Texture2D OutlineTexture { get; set; }
        public static Texture2D CollsionSphereTexture { get; set; }
        public static Texture2D HealthBarSegment { get; set; }
        public static Texture2D HealthBarContainer { get; set; }
        public static List<Texture2D> SideExhaustParticles { get; set; }

        static ContentProvider()
        {
            ExhaustParticles = new List<Texture2D>();
            EnemyShips = new List<Texture2D>();
            SideExhaustParticles = new List<Texture2D>();
            Random = new Random();
        }

        public static void InitializeContent(ContentManager contentManager)
        {
            HealthBarContainer = contentManager.Load<Texture2D>("UI/healthBar");
            HealthBarSegment = contentManager.Load<Texture2D>("UI/healthBarSegment");
            OutlineTexture = contentManager.Load<Texture2D>("Vectors/outline");
            //EnemyShips.Add(contentManager.Load<Texture2D>("Ships/debugEnemy"));
            EnemyShips.Add(contentManager.Load<Texture2D>("Ships/enemyGreen2"));
            EnemyShips.Add(contentManager.Load<Texture2D>("Ships/enemyRed4"));
            EnemyShips.Add(contentManager.Load<Texture2D>("Ships/enemyBlue1"));
            CollsionSphereTexture = contentManager.Load<Texture2D>("Vectors/circle");
            Boulder = contentManager.Load<Texture2D>("Boulders/boulder1"); 
            //Ship = contentManager.Load<Texture2D>("Ships/debugPlayer");
            Ship = contentManager.Load<Texture2D>("Ships/playerShip1_orange");
            SpriteFont = contentManager.Load<SpriteFont>("Fonts/spriteFont");
            ButtonTexture = contentManager.Load<Texture2D>("Buttons/grey");
            ButtonPressedTexture = contentManager.Load<Texture2D>("Buttons/grey_pressed");
            Background = contentManager.Load<Texture2D>("Backgrounds/darkPurple");
            Laser = contentManager.Load<Texture2D>("Lasers/laserBlue01");
            SideExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle8"));
            SideExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle7"));
            SideExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle8"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle1"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle2"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle3"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle4"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle5"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle6"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle7"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle8"));
        }

    }
}
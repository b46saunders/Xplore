using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class ContentProvider
    {
        public SpriteFont SpriteFont { get; }
        public Texture2D ButtonTexture { get; }
        public Texture2D MouseOverTexture { get; }
        public Texture2D Ship { get; set; }
        public Texture2D Background { get; set; }
        public Texture2D Laser { get; set; }
        public Texture2D Boulder { get; set; }
        public List<Texture2D> ExhaustParticles { get; set; }
        public List<Texture2D> EnemyShips { get; set; }

        public ContentProvider(ContentManager contentManager)
        {
            ExhaustParticles = new List<Texture2D>();
            EnemyShips = new List<Texture2D>();
            EnemyShips.Add(contentManager.Load<Texture2D>("Ships/enemyBlue1"));
            EnemyShips.Add(contentManager.Load<Texture2D>("Ships/enemyGreen2"));
            EnemyShips.Add(contentManager.Load<Texture2D>("Ships/enemyRed4"));

            Boulder = contentManager.Load<Texture2D>("Boulders/boulder1");
            //Background = contentManager.Load<Texture2D>("backgroundLevel1");
            Ship = contentManager.Load<Texture2D>("Ships/playerShip1_orange");
            SpriteFont = contentManager.Load<SpriteFont>("Fonts/spriteFont");
            ButtonTexture = contentManager.Load<Texture2D>("Buttons/red_button11");
            MouseOverTexture = contentManager.Load<Texture2D>("Buttons/red_button12");
            Background = contentManager.Load<Texture2D>("Backgrounds/starBackground");
            Laser = contentManager.Load<Texture2D>("Lasers/laserBlue01");
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle1"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle2"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle3"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle4"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle5"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle6"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle7"));
            ExhaustParticles.Add(contentManager.Load<Texture2D>("Exhausts/particle8"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke00"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke01"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke02"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke03"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke04"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke05"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke06"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke07"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke08"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke09"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke10"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke11"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke12"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke13"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke14"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke15"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke16"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke17"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke18"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke19"));
            //ExhaustParticles.Add(contentManager.Load<Texture2D>("blackSmoke20"));
        }

    }
}
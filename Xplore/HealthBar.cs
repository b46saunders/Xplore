using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class HealthBar
    {
        private Vector2 HealthBarPosition => new Vector2(_ship.Center.X,_ship.Position.Y - 50);
        private Ship _ship;
        private const int HealthBarWidth = 100;
        private const int HealthBarHeight = 10;
        public HealthBar(Ship ship)
        {
            _ship = ship;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var healthBarPosition = new Vector2(HealthBarPosition.X - HealthBarWidth/2f,
                HealthBarPosition.Y);
            float healthBarPercent = (float) _ship.HealthPoints/(float) _ship.MaxHealthPoints;
            int healthBarWidth = (int)(healthBarPercent*(float)HealthBarWidth);
            var drawRectangle = new Rectangle((int)healthBarPosition.X,(int)healthBarPosition.Y, healthBarWidth, HealthBarHeight);


            if (healthBarPercent < 0.3f)
            {
                spriteBatch.Draw(ContentProvider.HealthBarContainer, drawRectangle, Color.Red);
            }
            else
            {
                spriteBatch.Draw(ContentProvider.HealthBarContainer, drawRectangle, Color.White);
            }
            
        }

        public void Update(GameTime gameTime)
        {
           
        }


    }
}
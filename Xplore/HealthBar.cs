using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class HealthBar
    {
        private readonly IHealthBarEntity _healthBarEntity;
        private const int HealthBarWidth = 100;
        private const int HealthBarHeight = 10;
        public HealthBar(IHealthBarEntity healthBarEntity)
        {
            _healthBarEntity = healthBarEntity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var healthBarPosition = new Vector2(_healthBarEntity.HealthBarPosition.X - HealthBarWidth/2f,
                _healthBarEntity.HealthBarPosition.Y);
            float healthBarPercent = _healthBarEntity.HealthPoints/(float) _healthBarEntity.MaxHealthPoints;
            int healthBarWidth = (int)(healthBarPercent*HealthBarWidth);
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
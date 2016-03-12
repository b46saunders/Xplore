using Microsoft.Xna.Framework;

namespace Xplore
{
    public class BoulderHealthBar : IHealthBarEntity
    {
        private readonly Boulder _boulder;
        public Vector2 HealthBarPosition => new Vector2(_boulder.Center.X,_boulder.Position.Y - 50);
        public int MaxHealthPoints => _boulder.MaxHealthPoints;
        public int HealthPoints => _boulder.HealthPoints;

        public BoulderHealthBar(Boulder boulder)
        {
            _boulder = boulder;
        }
    }
}
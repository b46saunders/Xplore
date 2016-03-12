using Microsoft.Xna.Framework;

namespace Xplore
{
    public class ShipHealthBar : IHealthBarEntity
    {
        private readonly Ship _ship;
        public Vector2 HealthBarPosition => new Vector2(_ship.Center.X, _ship.Position.Y - 50);
        public int MaxHealthPoints => _ship.MaxHealthPoints;
        public int HealthPoints => _ship.HealthPoints;

        public ShipHealthBar(Ship ship)
        {
            _ship = ship;
        }
    }
}
using Microsoft.Xna.Framework;

namespace Xplore
{
    public interface IHealthBarEntity
    {
        Vector2 HealthBarPosition { get;}
        int MaxHealthPoints { get;}
        int HealthPoints { get; }
    }
}
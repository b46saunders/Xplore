using System;

namespace Xplore
{
    [Flags]
    public enum CollisionType
    {
        None =0,
        Ship =2,
        Boulder =4,
        Lazer = 8   
    }
}
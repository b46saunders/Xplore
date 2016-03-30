using System;
using Microsoft.Xna.Framework;

namespace Xplore.Screens
{
    public class DebugDisplayItem
    {
        public Func<string> Func { get; set; }
        public DebugDisplayItem(Func<string> func)
        {
            Func = func;
        }
          
    }
}
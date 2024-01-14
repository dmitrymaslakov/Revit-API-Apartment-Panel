using System;

namespace Utility.Extensions
{
    public static class MathExtensions
    {
        public static double ToRadians(this double degrees) 
            => degrees * Math.PI / 180;
    }
}

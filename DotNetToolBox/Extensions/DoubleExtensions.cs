using System;

namespace DotNetToolBox.Extensions
{
    public static class DoubleExtensions
    {
        public static double ToRadians(this double degreeValue)
        {
            return (degreeValue * Math.PI) / 180;
        }

        public static double ToDegrees(this double radianValue)
        {
            return (radianValue * 180) / Math.PI;
        }
    }
}

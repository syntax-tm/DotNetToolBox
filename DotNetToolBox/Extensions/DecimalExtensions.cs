using System;

namespace DotNetToolBox.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal ToRadians(this decimal degreeValue)
        {
            return (degreeValue * (decimal) Math.PI) / 180;
        }

        public static decimal ToDegrees(this decimal radianValue)
        {
            return (radianValue * 180) / (decimal) Math.PI;
        }

    }
}

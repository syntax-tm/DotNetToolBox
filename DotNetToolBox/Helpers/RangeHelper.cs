namespace DotNetToolBox.Helpers
{
    public static class RangeHelper
    {
        public static int InRange(int min, int max, int actualValue)
        {
            //  if it's over the max, return the max
            var isGreaterThanMax = actualValue > max;
            if (isGreaterThanMax)
            {
                return max;
            }

            //  if it's less than the min, return the min
            var isLessThanMin = actualValue < min;
            if (isLessThanMin)
            {
                return min;
            }

            //  value is within range
            return actualValue;
        }

        public static decimal InRange(decimal min, decimal max, decimal actualValue)
        {
            //  if it's over the max, return the max
            var isGreaterThanMax = actualValue > max;
            if (isGreaterThanMax)
            {
                return max;
            }

            //  if it's less than the min, return the min
            var isLessThanMin = actualValue < min;
            if (isLessThanMin)
            {
                return min;
            }

            //  value is within range
            return actualValue;
        }

        public static double InRange(double min, double max, double actualValue)
        {
            //  if it's over the max, return the max
            var isGreaterThanMax = actualValue > max;
            if (isGreaterThanMax)
            {
                return max;
            }

            //  if it's less than the min, return the min
            var isLessThanMin = actualValue < min;
            if (isLessThanMin)
            {
                return min;
            }

            //  value is within range
            return actualValue;
        }


    }
}

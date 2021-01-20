using System;

namespace DotNetToolBox.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the maximum time for the given date.
        /// </summary>
        /// <param name="curDate">The DateTime used to return the max time.</param>
        /// <returns>The maximum DateTime for a given date.</returns>
        public static DateTime? ToEndOfDay(this DateTime? curDate)
        {
            if (curDate == null) return null;
            var endOfDay = new DateTime(curDate.Value.Year, curDate.Value.Month, curDate.Value.Day, 23, 59, 59);
            return endOfDay;
        }

        /// <summary>
        /// Strips the time zone conversion information from a DateTime.
        /// </summary>
        /// <param name="dateToConvert">The DateTime to convert.</param>
        /// <returns>A DateTime of Kind "Unspecified".</returns>
        public static DateTime ToUnspecified(this DateTime dateToConvert)
        {
            return DateTime.SpecifyKind(dateToConvert, DateTimeKind.Unspecified);
        }
    }
}
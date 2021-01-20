using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace DotNetToolBox.Extensions
{
    public static class EnumExtensions
    {

        /// <summary>
        /// Returns the <see cref="Enum"/> member's <see cref="Enum.ToString()"/> value in lowercase.
        /// </summary>
        /// <param name="source">The <paramref name="source"/> Enum member.</param>
        public static string ToLower([NotNull] this Enum source)
        {
            return source.ToString().ToLower();
        }

        /// <summary>
        /// Returns the <see cref="Enum"/> member's <see cref="Enum.ToString()"/> value in uppercase.
        /// </summary>
        /// <param name="source">The <paramref name="source"/> Enum member.</param>
        public static string ToUpper([NotNull] this Enum source)
        {
            return source.ToString().ToUpper();
        }

        /// <summary>
        /// Returns the <see cref="DescriptionAttribute"/> string value in lowercase.
        /// </summary>
        /// <param name="source">The <paramref name="source"/> Enum member with the <see cref="DescriptionAttribute"/>.</param>
        public static string GetDescriptionToLower([NotNull] this Enum source)
        {
            return source.GetDescription().ToLower();
        }

        /// <summary>
        /// Returns the <see cref="DescriptionAttribute"/> string value in uppercase.
        /// </summary>
        /// <param name="source">The <paramref name="source"/> Enum member with the <see cref="DescriptionAttribute"/>.</param>
        public static string GetDescriptionToUpper([NotNull] this Enum source)
        {
            return source.GetDescription().ToUpper();
        }        
    }
}

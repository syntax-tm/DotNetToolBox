using System;
using System.Globalization;
using JetBrains.Annotations;

namespace DotNetToolBox.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Checks if the <see cref="input" /> starts with the <see cref="contains"/> text while ignoring case.
        /// </summary>
        /// <param name="input">The <c>source</c> input <see cref="string"/>.</param>
        /// <param name="contains">The <c>containing</c> text searched for in the <c>source</c>.</param>
        public static bool StartsWithIgnoreCase([NotNull] this string input, [NotNull] string contains)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return input.StartsWith(contains, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Checks if the <see cref="input" /> ends with the <see cref="contains"/> text while ignoring case.
        /// </summary>
        /// <param name="input">The <c>source</c> input <see cref="string"/>.</param>
        /// <param name="contains">The <c>containing</c> text searched for in the <c>source</c>.</param>
        public static bool EndsWithIgnoreCase([NotNull] this string input, [NotNull] string contains)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return input.EndsWith(contains, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Checks if the <see cref="input" /> contains the <see cref="contains"/> text while ignoring case.
        /// </summary>
        /// <param name="input">The <c>source</c> input <see cref="string"/>.</param>
        /// <param name="contains">The <c>containing</c> text searched for in the <c>source</c>.</param>
        public static bool ContainsIgnoreCase([NotNull] this string input, [NotNull] string contains)
        {
            var culture = CultureInfo.CurrentCulture;
            return culture.CompareInfo.IndexOf(input, contains, CompareOptions.IgnoreCase) >= 0;
        }

        /// <summary>
        /// Checks if the <see cref="source" /> string contains the same text as the <see cref="target"/>,
        /// using the <see cref="StringComparison.InvariantCultureIgnoreCase" /> and optionally trimming the
        /// the comparison strings with <paramref name="trim"/>.
        /// </summary>
        /// <param name="source">The <c>source</c> input <see cref="string"/>.</param>
        /// <param name="target">The <c>target</c> being compared to the <c>source</c>.</param>
        /// <param name="trim">Whether or not to <see cref="string.Trim()"/> the <see cref="source"/> and <see cref="target"/>.</param>
        public static bool EqualsIgnoreCase([NotNull] this string source, [NotNull] string target, bool trim = false)
        {
            if (trim)
            {
                source = source.Trim();
                target = target.Trim();
            }
            return source.Equals(target, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Verifies each character in the <see cref="source" /> is uppercase. Non-letter characters are ignored.
        /// </summary>
        public static bool IsUpper([NotNull] this string source)
        {
            foreach (var c in source.ToCharArray())
            {
                if (!char.IsLetter(c)) continue;
                if (char.IsLower(c)) return false;
            }
            return true;
        }

        /// <summary>
        /// Verifies each character in the <see cref="source" /> is lowercase. Non-letter characters are ignored.
        /// </summary>
        public static bool IsLower([NotNull] this string source)
        {
            foreach (var c in source.ToCharArray())
            {
                if (!char.IsLetter(c)) continue;
                if (char.IsUpper(c)) return false;
            }
            return true;
        }

        ///// <summary>
        ///// Returns the singular formatted text of the <see cref="source"/>.
        ///// </summary>
        //public static string ToSingular(this string source)
        //{
        //    var pluralization = new EnglishPluralizationService();
        //    return pluralization.Singularize(source);
        //}

        ///// <summary>
        ///// Returns the pluralized formatted text of the <see cref="source"/>.
        ///// </summary>
        //public static string ToPlural([NotNull] this string source)
        //{
        //    var pluralization = new EnglishPluralizationService();
        //    return pluralization.Pluralize(source);
        //}

        /// <summary>
        /// Returns whether or not the <see cref="source"/> starts with a vowel.
        /// </summary>
        public static bool StartsWithVowel([NotNull] this string source)
        {
            if (string.IsNullOrEmpty(source)) return false;
            if (source.StartsWithIgnoreCase("A")) return true;
            if (source.StartsWithIgnoreCase("E")) return true;
            if (source.StartsWithIgnoreCase("I")) return true;
            if (source.StartsWithIgnoreCase("O")) return true;
            if (source.StartsWithIgnoreCase("U")) return true;
            return false;
        }

        /// <summary>
        /// Prefixes <c>a</c> or <c>an</c> to the <see cref="source"/> text looking for a leading vowel character
        /// and optionally abbreviation handling with <paramref name="handleAbbreviations"/>.
        /// </summary>
        /// <param name="source">The input <c>source</c> string.</param>
        /// <param name="handleAbbreviations">Whether or not to assume the <see cref="source"/> is an if it's
        /// all uppercase and under the maximum of <c>6</c> characters.</param>
        public static string AnOrA([NotNull] this string source, bool handleAbbreviations = true)
        {
            //  if it starts with A, E, I, O, or U then it uses the 'an' article
            if (source.StartsWithVowel())
            {
                return $"an {source}";
            }

            //  if the source string is all caps, assume it's an abbreviation
            //  and needs to use the 'an' article. max length for this is 6.
            //  i.e. MBA, OE, FTP
            if (handleAbbreviations && source.IsUpper() && source.Length <= 6)
            {
                return $"an {source}";
            }

            //  otherwise, it uses the 'a' article
            return $"a {source}";
        }

        /// <summary>
        /// Converts the <see cref="source"/> to title case using the current <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="source">The input <c>source</c> string.</param>
        public static string ToTitleCase([NotNull] this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(source);
        }

    }
}

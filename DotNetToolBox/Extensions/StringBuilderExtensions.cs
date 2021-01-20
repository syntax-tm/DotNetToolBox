using System;
using System.Globalization;
using System.Text;

namespace DotNetToolBox.Extensions
{
    public static class StringBuilderExtensions
    {

        public static void StartSentence(this StringBuilder source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.ToString().EndsWith("."))
            {
                source.Append(" ");
                return;
            }
            if (source.ToString().EndsWith(" "))
            {
                return;
            }
            source.Append(" ");
        }
        
        public static void EndSentence(this StringBuilder source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.ToString().EndsWith(".")) return;
            source.Append(".");
        }

        public static void Space(this StringBuilder source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(source.ToString())) return;
            if (source.ToString().EndsWith(" ")) return;
            source.Append(" ");
        }


        public static void AppendIf(this StringBuilder source, bool condition, string message)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (!condition) return;
            source.Append(message);
        }

        public static void AppendIfNot(this StringBuilder source, bool condition, string message)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (condition) return;
            source.Append(message);
        }

        public static void AppendLineIf(this StringBuilder source, bool condition, string message)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (!condition) return;
            source.AppendLine(message);
        }

        public static void AppendLineIfNot(this StringBuilder source, bool condition, string message)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (condition) return;
            source.AppendLine(message);
        }

        public static string ToFormattedString(this StringBuilder source, TextCasing casing = TextCasing.Default)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            
            if (casing == TextCasing.Uppercase)
            {
                return source.ToString().ToUpper();
            }
            if (casing == TextCasing.Lowercase)
            {
                return source.ToString().ToLower();
            }
            if (casing == TextCasing.TitleCase)
            {
                var culture = CultureInfo.CurrentCulture;
                var textInfo = culture.TextInfo;
                var trimmedSource = source.ToString().Trim();
                return textInfo.ToTitleCase(trimmedSource);
            }
            return source.ToString().Trim();
        }

    }
}

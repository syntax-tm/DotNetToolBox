using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace DotNetToolBox.WPF.Converters
{
    [ValueConversion(typeof(byte?), typeof(string))]
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var fi = value?.GetType().GetField(value.ToString());
                var attributes = fi?.GetType().GetCustomAttribute<DescriptionAttribute>();
                return string.IsNullOrEmpty(attributes?.Description) ? fi.ToString() : attributes.Description;
            }
            catch (Exception)
            {
                return value?.ToString() ?? string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
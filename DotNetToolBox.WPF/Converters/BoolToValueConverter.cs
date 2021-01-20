using System;
using System.Globalization;
using System.Windows.Data;

namespace DotNetToolBox.WPF.Converters
{
    public class BoolToValueConverter : IValueConverter
    {
        public string FalseValue { get; set; }
        public string TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return FalseValue;

            if (value is bool)
                return (bool) value ? TrueValue : FalseValue;
            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : value.Equals(TrueValue);
        }
    }
}
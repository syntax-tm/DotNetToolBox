using System;
using System.Globalization;
using System.Windows.Data;

namespace DotNetToolBox.WPF.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public int FalseValue { get; set; }
        public int TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int?) value == TrueValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool) value ? TrueValue : FalseValue;
        }
    }
}
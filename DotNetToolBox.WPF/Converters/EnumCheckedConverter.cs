using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace DotNetToolBox.WPF.Converters
{
    public class EnumCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value != null, "value cannot be null");
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
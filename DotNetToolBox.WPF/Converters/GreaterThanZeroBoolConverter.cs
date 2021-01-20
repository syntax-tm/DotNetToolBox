using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace DotNetToolBox.WPF.Converters
{
    public class GreaterThanZeroBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            int? intValue = System.Convert.ToInt16(value);
            return intValue > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(true);
        }
    }

    public class GreaterThanZeroBoolConverterExtension : MarkupExtension
    {
        public IValueConverter ItemConverter { get; set; }

        public GreaterThanZeroBoolConverterExtension()
        {

        }

        public GreaterThanZeroBoolConverterExtension(IValueConverter itemConverter)
        {
            ItemConverter = itemConverter;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new GreaterThanZeroBoolConverter();
        }
    }
}

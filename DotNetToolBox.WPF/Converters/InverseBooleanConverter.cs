﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace DotNetToolBox.WPF.Converters
{
    [ValueConversion(typeof(bool?), typeof(bool?))]
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !(bool?) value;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;

            if (value is bool)
                return !(bool?) value;
            return value;
        }
    }
}
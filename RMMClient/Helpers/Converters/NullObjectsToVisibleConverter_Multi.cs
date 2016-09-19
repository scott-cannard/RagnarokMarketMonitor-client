using System;
using System.Windows;
using System.Windows.Data;

namespace RMMClient
{
    public class NullObjectsToVisibleConverter_Multi : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility conversion = Visibility.Visible;

            foreach (object val in values)
            {
                if (val != null)
                    conversion = Visibility.Collapsed;
            }
            return conversion;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new[] { Binding.DoNothing };
        }
    }
}

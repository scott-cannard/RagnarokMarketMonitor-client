using RMMSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RMMClient
{
    public class MagnitudeToRedGreenConverter_Multi : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush conversion = new SolidColorBrush(Colors.Black);

            if (values[0] != DependencyProperty.UnsetValue)
            {
                int magnitude = (int)values[0];
                ShopInfo.TransactionRole shopType = (ShopInfo.TransactionRole)values[1];

                if (shopType == ShopInfo.TransactionRole.Vendor)
                { //shopType = Vendor
                    if (magnitude > 0)
                        conversion = new SolidColorBrush(Colors.Red);
                    else if (magnitude < 0)
                        conversion = new SolidColorBrush(Colors.Green);
                }
                else
                { //shopType = Buyer
                    if (magnitude > 0)
                        conversion = new SolidColorBrush(Colors.Green);
                    else if (magnitude < 0)
                        conversion = new SolidColorBrush(Colors.Red);
                }
            }
            return conversion;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new[] { Binding.DoNothing };
        }
    }
}

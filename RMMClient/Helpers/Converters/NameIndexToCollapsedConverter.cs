using RMMSharedModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RMMClient
{
    public class NameIndexToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var vm = App.Current.MainWindow.DataContext as MainViewmodel;
            ItemInfo namedItem = vm.TrackedItems.FirstOrDefault(item => item.Name.Equals((string)value));
            return  namedItem.Equals(vm.TrackedItems.First()) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

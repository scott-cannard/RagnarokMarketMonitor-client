using RMMClient.RMMDataService;
using RMMSharedModels;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace RMMClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewmodel MainVM { get { return this.DataContext as MainViewmodel; } }
        private object wutev = new object();

        public MainWindow()
        {
            InitializeComponent();

            lv_TrackedItems.ItemsSource = MainVM.TrackedItems;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainVM.HeightForScrollTrigger = this.ActualHeight - 96;
        }

        private void OnListBoxMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        private void OnItemSelected(object sender, MouseButtonEventArgs e)
        {
            var presenter = ((sender as StackPanel).Parent as StackPanel).TemplatedParent as ContentPresenter;
            if (presenter.Content != null)
            {
                MainVM.ItemGeneral = presenter.Content as ItemInfo;
                MainVM.ShopSpec = null;
                MainVM.NoSelection = null;
            }
            e.Handled = true;
        }

        private void OnShopSelected(object sender, MouseButtonEventArgs e)
        {
            var presenter = (sender as Border).TemplatedParent as ContentPresenter;
            if (presenter.Content != null)
            {
                MainVM.ShopSpec = presenter.Content as ShopInfo;
                MainVM.ItemGeneral = null;
                MainVM.NoSelection = null;
            }
            e.Handled = true;
        }
    }
}

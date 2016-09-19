using System.Windows;
using System.Windows.Threading;

namespace RMMClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool Debug = false;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_UnhandledExceptionEventHandler;
        }

        private void App_UnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message + '\n' + e.Exception.StackTrace);
            e.Handled = true;
            this.Shutdown();
        }     
    }
}

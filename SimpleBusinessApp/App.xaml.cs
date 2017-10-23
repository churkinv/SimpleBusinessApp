using Autofac;
using SimpleBusinessApp.Data;
using SimpleBusinessApp.Startup;
using System;
using System.Windows;

namespace SimpleBusinessApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //var mainWindow = new MainWindow(
            //    new ViewModel.MainViewModel(
            //        new ClientDataService()));  // Dependency Injection and is it good, the problem when you adjust one of your constructors you have to ajust this code. So we use Autofac...with created class boostrapper 
            //mainWindow.Show();

            var bootstrapper = new Bootstrapper();
            var containter = bootstrapper.Bootstrap();
            var mainWindow = containter.Resolve<MainWindow>();
            mainWindow.Show();

        }

        private void Application_DispatcherUnhandledException(object sender, 
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error happened. Please infrom the admin." + 
                Environment.NewLine + e.Exception.Message, "Unexpected error" );
            e.Handled = true; // in this case if exception happened it will show it but we can continue and app will run. It is used in App.xaml
        }
    }
}

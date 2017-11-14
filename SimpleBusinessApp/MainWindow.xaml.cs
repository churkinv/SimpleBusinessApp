using MahApps.Metro.Controls;
using SimpleBusinessApp.ViewModel;
using System.Windows;

namespace SimpleBusinessApp
{

    public partial class MainWindow : MetroWindow
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel) // here we are assigning view model to the main window
        {
            InitializeComponent();
            _viewModel = viewModel; // before assigning it to data context we store it the field
            DataContext = _viewModel; // calling a load method in the constructor is a bad idea. Constr should just initialize objects it should not hit a database or call a web service
            Loaded += MainWindow_Loaded; // so instead of calling it derectly we call it inside Loaded event handler of main window
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}

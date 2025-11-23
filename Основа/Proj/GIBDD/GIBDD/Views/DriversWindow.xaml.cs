using GIBDD.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace GIBDD.Views
{
    public partial class DriversWindow : Window
    {
        private DriverViewModel _viewModel;

        public DriversWindow()
        {
            InitializeComponent();

            _viewModel = new DriverViewModel();
            this.DataContext = _viewModel;
        }

        // Обработчик двойного клика по DataGrid
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.OpenDriverProfile();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteDriver();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddDriver();
        }
    }
}
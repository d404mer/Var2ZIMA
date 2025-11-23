using GIBDD.ViewModels;
using System.Windows;

namespace GIBDD.Views
{
    /// <summary>
    /// Логика взаимодействия для DriversWindow.xaml
    /// </summary>
    public partial class DriversWindow : Window
    {
        private DriverViewModel _viewModel;
        public DriversWindow()
        {
            InitializeComponent();

            _viewModel = new DriverViewModel();

            this.DataContext = new DriverViewModel();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddDriver();
            _viewModel.LoadDrivers();
        }
    }

}

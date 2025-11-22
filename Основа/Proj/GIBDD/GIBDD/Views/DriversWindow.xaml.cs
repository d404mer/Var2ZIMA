using GIBDD.Models;
using GIBDD.Services;
using GIBDD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static GIBDD.ViewModels.BaseViewModel;

namespace GIBDD.Views
{
    /// <summary>
    /// Логика взаимодействия для DriversWindow.xaml
    /// </summary>
    public partial class DriversWindow : Window
    {
        public DriversWindow()
        {
            InitializeComponent();

            this.DataContext = new DriverViewModel();

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

    public class DriverViewModel : BaseViewModel
    {
        private DataService<Driver> _driverService = new DataService<Driver>();
        private List<Driver> _drivers;

        // Коллекция для привязки к DataGrid
        public List<Driver> Drivers
        {
            get => _drivers;
            set { _drivers = value; OnPropertyChanged(); }
        }

        public RelayCommand LoadDriversCommand { get; }

        public DriverViewModel()
        {
            LoadDriversCommand = new RelayCommand(LoadDrivers);
            LoadDrivers(); // Загружаем сразу при создании
        }

        private void LoadDrivers()
        {
            Drivers = _driverService.GetAll();
        }
    }
}

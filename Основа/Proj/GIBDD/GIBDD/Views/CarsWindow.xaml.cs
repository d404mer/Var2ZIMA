using GIBDD.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;

namespace GIBDD.Views
{
    /// <summary>
    /// Окно для работы со списком автомобилей
    /// </summary>
    public partial class CarsWindow : Window
    {
        private CarViewModel _viewModel;

        /// <summary>
        /// Конструктор окна CarsWindow
        /// </summary>
        public CarsWindow()
        {
            InitializeComponent();

            _viewModel = new CarViewModel();
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// Обработчик двойного клика по DataGrid для открытия профиля автомобиля
        /// </summary>
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.OpenCarProfile();
            RefreshDataGrid();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Назад"
        /// </summary>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Удалить"
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteCar();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить"
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddCar();
            RefreshDataGrid();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Обновить"
        /// </summary>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadCars();
            RefreshDataGrid();
        }

        /// <summary>
        /// Принудительно обновляет привязку DataGrid
        /// </summary>
        private void RefreshDataGrid()
        {
            // Обновляем привязку ItemsSource
            var binding = BindingOperations.GetBindingExpression(CarsDataGrid, System.Windows.Controls.DataGrid.ItemsSourceProperty);
            binding?.UpdateTarget();
            
            // Также обновляем саму коллекцию через привязку
            CarsDataGrid.Items.Refresh();
        }
    }
}


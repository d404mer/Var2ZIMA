using GIBDD.ViewModels;
using System.Windows;
using System.Windows.Input;

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
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Обновить"
        /// </summary>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadCars();
        }
    }
}


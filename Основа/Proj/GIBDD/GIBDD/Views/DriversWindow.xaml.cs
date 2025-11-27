using GIBDD.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace GIBDD.Views
{
    /// <summary>
    /// Окно для работы со списком водителей
    /// </summary>
    public partial class DriversWindow : Window
    {
        private DriverViewModel _viewModel;

        /// <summary>
        /// Конструктор окна DriversWindow
        /// </summary>
        public DriversWindow()
        {
            InitializeComponent();

            _viewModel = new DriverViewModel();
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// Обработчик двойного клика по DataGrid для открытия профиля водителя
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.OpenDriverProfile();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Назад"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Удалить"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteDriver();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddDriver();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Обновить"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadDrivers();
        }
    }
}
using System.Windows;

namespace GIBDD.Views
{
    /// <summary>
    /// Главное меню приложения после авторизации
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        /// <summary>
        /// Конструктор окна MainMenuWindow
        /// </summary>
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Водители"
        /// </summary>
        private void DriversBtn_Click(object sender, RoutedEventArgs e)
        {
            var driversWindow = new DriversWindow();
            driversWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Автомобили"
        /// </summary>
        private void CarsBtn_Click(object sender, RoutedEventArgs e)
        {
            var carsWindow = new CarsWindow();
            carsWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Выход"
        /// </summary>
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}


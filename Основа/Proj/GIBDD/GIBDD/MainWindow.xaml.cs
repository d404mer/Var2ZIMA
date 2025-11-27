using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GIBDD.Services;
using GIBDD.Views;

namespace GIBDD
{
    /// <summary>
    /// Окно авторизации пользователя
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор окна MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Войти"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Text;

            AuthService authService = new AuthService();

            if (authService.Login(login, password))
            {
                // Успешный вход
                MessageBox.Show($"Добро пожаловать, {AuthService.CurrentUser.Login}!");

                // Открываем главное меню
                var mainMenuWindow = new Views.MainMenuWindow();
                mainMenuWindow.Show();

                // Закрываем окно авторизации
                this.Close();
            }
            else
            {
                // Неудачный вход
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
    }
}
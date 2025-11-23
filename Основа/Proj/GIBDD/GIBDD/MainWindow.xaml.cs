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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

       
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Text;

            AuthService authService = new AuthService();

            if (authService.Login(login, password))
            {
                // Успешный вход
                MessageBox.Show($"Добро пожаловать, {AuthService.CurrentUser.Login}!");

                // Открываем основное рабочее окно
                var driversWindow = new DriversWindow();
                driversWindow.Show();

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
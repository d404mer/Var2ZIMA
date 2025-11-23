using GIBDD.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            this.DataContext = _viewModel;

            // Подписываемся на событие загрузки окна
            this.Loaded += DriversWindow_Loaded;
        }

        private void DriversWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Находим DataGrid и подписываемся на двойной клик
            var dataGrid = FindVisualChild<DataGrid>(this);
            if (dataGrid != null)
            {
                dataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
            }
        }

        // Вспомогательный метод для поиска DataGrid
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                {
                    return result;
                }
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // При двойном клике открываем анкету водителя
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

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddDriver();
            _viewModel.LoadDrivers();
        }
    }

}

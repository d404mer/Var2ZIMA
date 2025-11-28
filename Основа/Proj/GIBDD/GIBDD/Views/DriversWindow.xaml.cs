using GIBDD.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Data;

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

            Debug.WriteLine("[DriversWindow] Initialized and DataContext set to DriverViewModel");
        }

        /// <summary>
        /// Обработчик двойного клика по DataGrid для открытия профиля водителя
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("[DriversWindow] DataGrid_MouseDoubleClick");
            _viewModel.OpenDriverProfile();
            RefreshDataGrid();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Назад"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[DriversWindow] Back_Click");
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
            Debug.WriteLine("[DriversWindow] Delete_Click");
            _viewModel.DeleteDriver();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[DriversWindow] Add_Click");
            _viewModel.AddDriver();
            RefreshDataGrid();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Обновить"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[DriversWindow] Update_Click");
            _viewModel.LoadDrivers();
            RefreshDataGrid();
        }

        /// <summary>
        /// Принудительно обновляет привязку DataGrid
        /// </summary>
        private void RefreshDataGrid()
        {
            Debug.WriteLine("[DriversWindow] RefreshDataGrid called");
            // Обновляем привязку ItemsSource
            var binding = BindingOperations.GetBindingExpression(DriversDataGrid, System.Windows.Controls.DataGrid.ItemsSourceProperty);
            binding?.UpdateTarget();
            
            // Также обновляем саму коллекцию через привязку
            DriversDataGrid.Items.Refresh();
            
            Debug.WriteLine($"[DriversWindow] DataGrid refreshed. Items count = {DriversDataGrid.Items.Count}");
        }
    }
}
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using GIBDD.Models;

namespace GIBDD.Views
{
    /// <summary>
    /// Окно для просмотра и редактирования водителя
    /// </summary>
    public partial class ViewDriverWindow : Window
    {
        /// <summary>
        /// Отредактированный водитель
        /// </summary>
        public Driver EditedDriver { get; private set; }

        /// <summary>
        /// Конструктор окна ViewDriverWindow
        /// </summary>
        /// <param name="driver">Водитель для редактирования</param>
        public ViewDriverWindow(Driver driver)
        {
            InitializeComponent();

            // Создаем копию для редактирования
            EditedDriver = new Driver
            {
                Guid = driver.Guid,
                Surname = driver.Surname,
                Name = driver.Name,
                MiddleName = driver.MiddleName,
                PassportSeries = driver.PassportSeries,
                PassportNumber = driver.PassportNumber,
                Postcode = driver.Postcode,
                Address = driver.Address,
                ActualAdress = driver.ActualAdress,
                Company = driver.Company,
                JobName = driver.JobName,
                Phone = driver.Phone,
                Email = driver.Email,
                Photo = driver.Photo,
                Description = driver.Description,
                LicenceDate = driver.LicenceDate,
                ExpireDate = driver.ExpireDate,
                Categories = driver.Categories,
                LicenceSeries = driver.LicenceSeries,
                LicenceNumber = driver.LicenceNumber,
                LicenceStatus = driver.LicenceStatus
            };

            this.DataContext = EditedDriver;
            LoadPhoto();
        }

        /// <summary>
        /// Загружает фотографию водителя
        /// </summary>
        private void LoadPhoto()
        {
            string photoPath = Path.Combine(Directory.GetCurrentDirectory(), "photo", EditedDriver.Photo);

            if (File.Exists(photoPath))
            {
                DriverPhoto.Source = new BitmapImage(new Uri(photoPath));
            }
        }
 
        /// <summary>
        /// Обработчик нажатия кнопки "Отмена"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Сохранить"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Простая проверка обязательных полей
            if (string.IsNullOrWhiteSpace(EditedDriver.Surname) ||
                string.IsNullOrWhiteSpace(EditedDriver.Name))
            {
                MessageBox.Show("Заполните обязательные поля!");
                return;
            }

            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Выбрать фото"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png";

            if (dialog.ShowDialog() == true)
            {
                EditedDriver.Photo = dialog.FileName;
                LoadPhoto();
            }
        }
    }
}
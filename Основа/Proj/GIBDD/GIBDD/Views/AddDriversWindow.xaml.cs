using GIBDD.Models;
using GIBDD.Services;
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

namespace GIBDD.Views
{
    /// <summary>
    /// Логика взаимодействия для AddDriversWindow.xaml
    /// </summary>
    public partial class AddDriversWindow : Window
    {
        public Driver NewDriver { get; private set; }
        public AddDriversWindow()
        {
            InitializeComponent();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SurnameBox.Text) ||
            string.IsNullOrWhiteSpace(NameBox.Text) ||
            string.IsNullOrWhiteSpace(MiddleNameBox.Text) ||
            string.IsNullOrWhiteSpace(PassportSeriesBox.Text) ||
            string.IsNullOrWhiteSpace(PassportNumberBox.Text) ||
            string.IsNullOrWhiteSpace(AddressBox.Text) ||
            string.IsNullOrWhiteSpace(ActualAddressBox.Text) ||
            string.IsNullOrWhiteSpace(PhoneBox.Text) ||
            string.IsNullOrWhiteSpace(EmailBox.Text) ||
            string.IsNullOrWhiteSpace(PhotoBox.Text))
            {
                MessageBox.Show("Заполните все обязательные поля (помечены *)!");
                return;
            }


            // Проверка паспорта (только цифры)
            if (!IsAllDigits(PassportSeriesBox.Text) || !IsAllDigits(PassportNumberBox.Text))
            {
                MessageBox.Show("Серия и номер паспорта должны содержать только цифры!");
                return;
            }

            // Проверка телефона (только цифры)
            if (!IsAllDigits(PhoneBox.Text.Replace("+", "").Replace(" ", "").Replace("-", "")))
            {
                MessageBox.Show("Телефон должен содержать только цифры!");
                return;
            }

            // Проверка email (наличие @)
            if (!EmailBox.Text.Contains("@"))
            {
                MessageBox.Show("Email должен содержать символ @!");
                return;
            }


            long nextId = GetNextDriverId();

            // Создаем нового водителя
            NewDriver = new Driver
            {
                Guid = nextId,
                Surname = SurnameBox.Text,
                Name = NameBox.Text,
                MiddleName = MiddleNameBox.Text,
                PassportSeries = PassportSeriesBox.Text,
                PassportNumber = PassportNumberBox.Text,
                Address = AddressBox.Text,
                ActualAdress = ActualAddressBox.Text,
                Company = CompanyBox.Text,
                JobName = JobNameBox.Text,
                Phone = PhoneBox.Text,
                Email = EmailBox.Text,
                Photo = PhotoBox.Text,
                Description = DescriptionBox.Text,
                // Заполняем обязательные поля которые есть в модели
                Postcode = "000000", // По умолчанию
                LicenceDate = DateTime.Now.ToString("yyyy-MM-dd"),
                ExpireDate = DateTime.Now.AddYears(10).ToString("yyyy-MM-dd"),
                Categories = "B",
                LicenceSeries = "AB",
                LicenceNumber = "123456",
                LicenceStatus = "Активен"
            };

            this.DialogResult = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BrowsePhoto_Click(object sender, RoutedEventArgs e)
        {
            // Диалог выбора файла
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png";

            if (dialog.ShowDialog() == true)
            {
                PhotoBox.Text = dialog.FileName;
            }
        }

        private long GetNextDriverId()
        {
            var service = new DataService<Driver>();
            var lastDriver = service.GetAll().OrderByDescending(d => d.Guid).FirstOrDefault();
            return lastDriver?.Guid + 1 ?? 1;
        }
        private bool IsAllDigits(string text)
        {
            return text.All(char.IsDigit);
        }
    }
}


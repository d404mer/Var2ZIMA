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
using System.Diagnostics;

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
            Debug.WriteLine("[AddDriversWindow] Initialized");
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[AddDriversWindow] SaveBtn_Click called");
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
                Debug.WriteLine("[AddDriversWindow] Validation failed: required fields are empty");
                return;
            }


            // Проверка паспорта (только цифры)
            if (!IsAllDigits(PassportSeriesBox.Text) || !IsAllDigits(PassportNumberBox.Text))
            {
                MessageBox.Show("Серия и номер паспорта должны содержать только цифры!");
                Debug.WriteLine("[AddDriversWindow] Validation failed: passport not numeric");
                return;
            }

            // Проверка телефона (только цифры)
            if (!IsAllDigits(PhoneBox.Text.Replace("+", "").Replace(" ", "").Replace("-", "")))
            {
                MessageBox.Show("Телефон должен содержать только цифры!");
                Debug.WriteLine("[AddDriversWindow] Validation failed: phone not numeric");
                return;
            }

            // Проверка email (наличие @)
            if (!EmailBox.Text.Contains("@"))
            {
                MessageBox.Show("Email должен содержать символ @!");
                Debug.WriteLine("[AddDriversWindow] Validation failed: email without @");
                return;
            }

            // Проверка длины полей в соответствии с ограничениями БД
            if (!ValidateFieldLengths())
            {
                // Подробности уже выведены пользователю
                Debug.WriteLine("[AddDriversWindow] Validation failed: one or more fields exceed max length");
                return;
            }


            try
            {
                long nextId = GetNextDriverId();
                Debug.WriteLine($"[AddDriversWindow] Next driver GUID = {nextId}");

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

                Debug.WriteLine($"[AddDriversWindow] NewDriver created: GUID={NewDriver.Guid}, Name={NewDriver.Surname} {NewDriver.Name}");

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddDriversWindow] Exception in SaveBtn_Click: {ex}");
                MessageBox.Show("Ошибка при создании нового водителя. Подробности смотрите в Debug Output.");
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[AddDriversWindow] CancelBtn_Click called");
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
                Debug.WriteLine($"[AddDriversWindow] Photo selected: {dialog.FileName}");
            }
        }

        private long GetNextDriverId()
        {
            Debug.WriteLine("[AddDriversWindow] GetNextDriverId called");
            var service = new DataService<Driver>();
            var lastDriver = service.GetAll().OrderByDescending(d => d.Guid).FirstOrDefault();
            var next = lastDriver?.Guid + 1 ?? 1;
            Debug.WriteLine($"[AddDriversWindow] Last GUID = {lastDriver?.Guid}, Next GUID = {next}");
            return next;
        }

        /// <summary>
        /// Проверка длины вводимых полей по ограничениям, заданным в модели/БД
        /// </summary>
        /// <returns>true, если все длины в пределах допустимых, иначе false</returns>
        private bool ValidateFieldLengths()
        {
            // Ограничения смотрим в GibddDbContext (HasMaxLength)
            var errors = new List<string>();

            if (SurnameBox.Text.Length > 15)
                errors.Add("Фамилия (максимум 15 символов)");
            if (NameBox.Text.Length > 15)
                errors.Add("Имя (максимум 15 символов)");
            if (MiddleNameBox.Text.Length > 15)
                errors.Add("Отчество (максимум 15 символов)");

            if (PassportSeriesBox.Text.Length > 10)
                errors.Add("Серия паспорта (максимум 10 символов)");
            if (PassportNumberBox.Text.Length > 10)
                errors.Add("Номер паспорта (максимум 10 символов)");

            if (AddressBox.Text.Length > 30)
                errors.Add("Адрес регистрации (максимум 30 символов)");
            if (ActualAddressBox.Text.Length > 30)
                errors.Add("Фактический адрес (максимум 30 символов)");

            if (CompanyBox.Text.Length > 30)
                errors.Add("Место работы (максимум 30 символов)");
            if (JobNameBox.Text.Length > 30)
                errors.Add("Должность (максимум 30 символов)");

            if (PhoneBox.Text.Length > 20)
                errors.Add("Телефон (максимум 20 символов)");

            if (EmailBox.Text.Length > 30)
                errors.Add("Email (максимум 30 символов)");

            if (PhotoBox.Text.Length > 50)
                errors.Add("Путь к фотографии (максимум 50 символов; уменьшите длину пути/имени файла)");

            if (DescriptionBox.Text.Length > 50)
                errors.Add("Описание (максимум 50 символов)");

            if (errors.Count > 0)
            {
                var message = "Некоторые поля слишком длинные и не могут быть сохранены в базу данных:\n\n" +
                              string.Join("\n", errors) +
                              "\n\nУкоротите указанные поля и попробуйте снова.";
                MessageBox.Show(message, "Ошибка валидации длины полей", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        private bool IsAllDigits(string text)
        {
            return text.All(char.IsDigit);
        }
    }
}


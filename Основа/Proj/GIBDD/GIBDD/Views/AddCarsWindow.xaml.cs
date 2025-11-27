using GIBDD.Models;
using GIBDD.Services;
using System;
using System.Linq;
using System.Windows;

namespace GIBDD.Views
{
    /// <summary>
    /// Окно для добавления нового автомобиля
    /// </summary>
    public partial class AddCarsWindow : Window
    {
        /// <summary>
        /// Новый автомобиль, созданный в этом окне
        /// </summary>
        public Car NewCar { get; private set; }

        /// <summary>
        /// Конструктор окна AddCarsWindow
        /// </summary>
        public AddCarsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Сохранить"
        /// </summary>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CarVimBox.Text))
            {
                MessageBox.Show("Заполните VIN номер (обязательное поле)!");
                return;
            }

            // Проверка на уникальность VIN
            var service = new DataService<Car>();
            var existingCar = service.GetAll().FirstOrDefault(c => c.CarVim == CarVimBox.Text);
            if (existingCar != null)
            {
                MessageBox.Show("Автомобиль с таким VIN номером уже существует!");
                return;
            }

            // Проверка года (только цифры, 4 символа)
            if (!string.IsNullOrWhiteSpace(CarYearBox.Text))
            {
                if (!IsAllDigits(CarYearBox.Text) || CarYearBox.Text.Length != 4)
                {
                    MessageBox.Show("Год выпуска должен содержать 4 цифры!");
                    return;
                }
            }

            // Проверка веса (только цифры)
            if (!string.IsNullOrWhiteSpace(CarWeightBox.Text))
            {
                if (!IsAllDigits(CarWeightBox.Text))
                {
                    MessageBox.Show("Вес должен содержать только цифры!");
                    return;
                }
            }

            // Создаем новый автомобиль
            NewCar = new Car
            {
                CarVim = CarVimBox.Text,
                CarModel = CarModelBox.Text,
                CarYear = CarYearBox.Text,
                CarWeight = CarWeightBox.Text
            };

            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Отмена"
        /// </summary>
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Проверяет, содержит ли строка только цифры
        /// </summary>
        /// <param name="text">Проверяемая строка</param>
        /// <returns>True, если строка содержит только цифры</returns>
        private bool IsAllDigits(string text)
        {
            return text.All(char.IsDigit);
        }
    }
}


using System.Linq;
using System.Windows;
using GIBDD.Models;

namespace GIBDD.Views
{
    /// <summary>
    /// Окно для просмотра и редактирования автомобиля
    /// </summary>
    public partial class ViewCarWindow : Window
    {
        /// <summary>
        /// Отредактированный автомобиль
        /// </summary>
        public Car EditedCar { get; private set; }

        /// <summary>
        /// Конструктор окна ViewCarWindow
        /// </summary>
        /// <param name="car">Автомобиль для редактирования</param>
        public ViewCarWindow(Car car)
        {
            InitializeComponent();

            // Создаем копию для редактирования
            EditedCar = new Car
            {
                CarVim = car.CarVim,
                CarModel = car.CarModel,
                CarYear = car.CarYear,
                CarWeight = car.CarWeight
            };

            this.DataContext = EditedCar;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Отмена"
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Сохранить"
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Проверка года (только цифры, 4 символа)
            if (!string.IsNullOrWhiteSpace(EditedCar.CarYear))
            {
                if (!IsAllDigits(EditedCar.CarYear) || EditedCar.CarYear.Length != 4)
                {
                    MessageBox.Show("Год выпуска должен содержать 4 цифры!");
                    return;
                }
            }

            // Проверка веса (только цифры)
            if (!string.IsNullOrWhiteSpace(EditedCar.CarWeight))
            {
                if (!IsAllDigits(EditedCar.CarWeight))
                {
                    MessageBox.Show("Вес должен содержать только цифры!");
                    return;
                }
            }

            this.DialogResult = true;
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


using GIBDD.Models;
using GIBDD.Services;
using GIBDD.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace GIBDD.ViewModels
{
    /// <summary>
    /// ViewModel для управления автомобилями
    /// </summary>
    public class CarViewModel : BaseViewModel
    {
        private DataService<Car> _carService = new DataService<Car>();
        private List<Car> _allCars;
        private ObservableCollection<Car> _cars;
        private Car _selectedCar;
        private string _searchText;

        /// <summary>
        /// Команда загрузки автомобилей
        /// </summary>
        public RelayCommand LoadCarsCommand { get; }

        /// <summary>
        /// Команда добавления автомобиля
        /// </summary>
        public RelayCommand AddCarsCommand { get; }

        /// <summary>
        /// Команда удаления автомобиля
        /// </summary>
        public RelayCommand DeleteCarsCommand { get; }

        /// <summary>
        /// Команда обновления списка автомобилей
        /// </summary>
        public RelayCommand UpdateCommand { get; }

        /// <summary>
        /// Коллекция автомобилей для привязки к DataGrid
        /// </summary>
        public ObservableCollection<Car> Cars
        {
            get => _cars;
            set { _cars = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Выбранный автомобиль
        /// </summary>
        public Car SelectedCar
        {
            get => _selectedCar;
            set { _selectedCar = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Текст для поиска
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                SearchCars();
            }
        }

        /// <summary>
        /// Конструктор CarViewModel
        /// </summary>
        public CarViewModel()
        {
            LoadCarsCommand = new RelayCommand(LoadCars);
            AddCarsCommand = new RelayCommand(AddCar);
            DeleteCarsCommand = new RelayCommand(DeleteCar);
            UpdateCommand = new RelayCommand(LoadCars);

            LoadCars();
        }

        /// <summary>
        /// Загружает список всех автомобилей из базы данных
        /// </summary>
        public void LoadCars()
        {
            _allCars = _carService.GetAll();
            Cars = new ObservableCollection<Car>(_allCars);
        }

        /// <summary>
        /// Выполняет поиск автомобилей по тексту
        /// </summary>
        private void SearchCars()
        {
            if (_allCars == null) return;

            IEnumerable<Car> searchedCars;

            if (string.IsNullOrEmpty(SearchText))
            {
                searchedCars = _allCars;
            }
            else
            {
                searchedCars = _allCars
                    .Where(c => (c.CarVim?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                               (c.CarModel?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                               (c.CarYear?.Contains(SearchText) ?? false));
            }

            Cars = new ObservableCollection<Car>(searchedCars);
        }

        /// <summary>
        /// Открывает окно добавления нового автомобиля
        /// </summary>
        public void AddCar()
        {
            var addCar = new AddCarsWindow();
            if (addCar.ShowDialog() == true)
            {
                var newCar = addCar.NewCar;
                _carService.Add(newCar);
                LoadCars();
                MessageBox.Show("Автомобиль успешно добавлен!");
            }
        }

        /// <summary>
        /// Удаляет выбранный автомобиль
        /// </summary>
        public void DeleteCar()
        {
            if (SelectedCar == null)
            {
                MessageBox.Show("Выберите автомобиль для удаления!");
                return;
            }

            var result = MessageBox.Show(
                $"Удалить автомобиль {SelectedCar.CarVim}?",
                "Подтверждение",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                _carService.Delete(SelectedCar);
                LoadCars();
                MessageBox.Show("Автомобиль удален!");
            }
        }

        /// <summary>
        /// Открывает окно просмотра и редактирования автомобиля
        /// </summary>
        public void OpenCarProfile()
        {
            if (SelectedCar == null)
            {
                MessageBox.Show("Выберите автомобиль!");
                return;
            }

            var viewWindow = new ViewCarWindow(SelectedCar);
            if (viewWindow.ShowDialog() == true)
            {
                var updatedCar = viewWindow.EditedCar;
                _carService.Update(updatedCar);
                LoadCars();
                MessageBox.Show("Данные обновлены!");
            }
        }
    }
}


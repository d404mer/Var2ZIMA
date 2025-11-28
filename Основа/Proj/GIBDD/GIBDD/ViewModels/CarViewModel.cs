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
            // Инициализируем коллекцию один раз
            _cars = new ObservableCollection<Car>();
            
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
            try
            {
                _allCars = _carService.GetAll();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка автомобилей: {ex.Message}");
            }
        }

        /// <summary>
        /// Применяет все активные фильтры (поиск) к загруженным данным
        /// </summary>
        private void ApplyFilters()
        {
            if (_allCars == null)
            {
                _cars.Clear();
                return;
            }

            IEnumerable<Car> filtered = _allCars;

            // Применяем поиск
            if (!string.IsNullOrEmpty(SearchText))
            {
                filtered = filtered.Where(c => 
                    (c.CarVim != null && c.CarVim.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (c.CarModel != null && c.CarModel.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (c.CarYear != null && c.CarYear.Contains(SearchText)));
            }

            // Обновляем существующую коллекцию вместо создания новой
            var filteredList = filtered.ToList();
            _cars.Clear();
            foreach (var car in filteredList)
            {
                _cars.Add(car);
            }
            
            OnPropertyChanged(nameof(Cars));
        }

        /// <summary>
        /// Выполняет поиск автомобилей по тексту
        /// </summary>
        private void SearchCars()
        {
            ApplyFilters();
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
                try
                {
                    _carService.Add(newCar);
                    
                    // Сбрасываем фильтры перед обновлением, чтобы новый автомобиль был виден
                    SearchText = string.Empty;
                    
                    LoadCars();
                    
                    // Явно уведомляем об изменении коллекции
                    OnPropertyChanged(nameof(Cars));
                    
                    MessageBox.Show("Автомобиль успешно добавлен!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении автомобиля: {ex.Message}");
                }
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
                try
                {
                    _carService.Delete(SelectedCar);
                    LoadCars();
                    OnPropertyChanged(nameof(Cars));
                    MessageBox.Show("Автомобиль удален!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении автомобиля: {ex.Message}");
                }
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
                try
                {
                    _carService.Update(updatedCar);
                    LoadCars();
                    OnPropertyChanged(nameof(Cars));
                    MessageBox.Show("Данные обновлены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении автомобиля: {ex.Message}");
                }
            }
        }
    }
}


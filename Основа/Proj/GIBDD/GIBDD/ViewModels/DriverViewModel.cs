using GIBDD.Models;
using GIBDD.Services;
using GIBDD.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // Добавь это
using System.Linq;
using System.Windows;

namespace GIBDD.ViewModels
{
    /// <summary>
    /// ViewModel для управления водителями
    /// </summary>
    public class DriverViewModel : BaseViewModel
    {
        private DataService<Driver> _driverService = new DataService<Driver>();
        private List<Driver> _allDrivers;
        private ObservableCollection<Driver> _drivers;
        private Driver _selectedDriver;
        private string _searchText;
        private string _selectedCategory;

        /// <summary>
        /// Команда загрузки водителей
        /// </summary>
        public RelayCommand LoadDriversCommand { get; }

        /// <summary>
        /// Команда добавления водителя
        /// </summary>
        public RelayCommand AddDriversCommand { get; }

        /// <summary>
        /// Команда удаления водителя
        /// </summary>
        public RelayCommand DeleteDriversCommand { get; }

        /// <summary>
        /// Команда обновления списка водителей
        /// </summary>
        public RelayCommand UpdateCommand { get; }

        /// <summary>
        /// Коллекция водителей для привязки к DataGrid
        /// </summary>
        public ObservableCollection<Driver> Drivers
        {
            get => _drivers;
            set { _drivers = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Выбранный водитель
        /// </summary>
        public Driver SelectedDriver
        {
            get => _selectedDriver;
            set { _selectedDriver = value; OnPropertyChanged(); }
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
                SearchDrivers();
            }
        }

        /// <summary>
        /// Выбранная категория для фильтрации
        /// </summary>
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                FilterByCategory();
            }
        }

        /// <summary>
        /// Список всех доступных категорий водительских прав
        /// </summary>
        public List<string> Categories
        {
            get
            {
                var categories = new List<string> { "Все категории" };
                if (_allDrivers != null)
                {
                    categories.AddRange(_allDrivers
                        .Select(d => d.Categories)
                        .Distinct()
                        .Where(c => !string.IsNullOrEmpty(c)));
                }
                return categories;
            }
        }

        /// <summary>
        /// Конструктор DriverViewModel
        /// </summary>
        public DriverViewModel()
        {
            LoadDriversCommand = new RelayCommand(LoadDrivers);
            AddDriversCommand = new RelayCommand(AddDriver);
            DeleteDriversCommand = new RelayCommand(DeleteDriver);
            UpdateCommand = new RelayCommand(LoadDrivers);

            LoadDrivers();
        }

        /// <summary>
        /// Загружает список всех водителей из базы данных
        /// </summary>
        public void LoadDrivers()
        {
            _allDrivers = _driverService.GetAll();
            Drivers = new ObservableCollection<Driver>(_allDrivers);
            OnPropertyChanged(nameof(Categories));
        }

        /// <summary>
        /// Фильтрует водителей по выбранной категории
        /// </summary>
        public void FilterByCategory()
        {
            if (_allDrivers == null) return;

            IEnumerable<Driver> filteredDrivers;

            if (string.IsNullOrEmpty(SelectedCategory) || SelectedCategory == "Все категории")
            {
                filteredDrivers = _allDrivers;
            }
            else
            {
                filteredDrivers = _allDrivers
                    .Where(d => d.Categories.Contains(SelectedCategory));
            }

            Drivers = new ObservableCollection<Driver>(filteredDrivers);
        }

        /// <summary>
        /// Выполняет поиск водителей по тексту
        /// </summary>
        private void SearchDrivers()
        {
            if (_allDrivers == null) return;

            IEnumerable<Driver> searchedDrivers;

            if (string.IsNullOrEmpty(SearchText))
            {
                searchedDrivers = _allDrivers;
            }
            else
            {
                searchedDrivers = _allDrivers
                    .Where(d => d.Surname.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                               d.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                               d.Phone.Contains(SearchText) ||
                               d.LicenceNumber.Contains(SearchText));
            }

            Drivers = new ObservableCollection<Driver>(searchedDrivers);
        }

        /// <summary>
        /// Открывает окно добавления нового водителя
        /// </summary>
        public void AddDriver()
        {
            var addDriver = new AddDriversWindow();
            if (addDriver.ShowDialog() == true)
            {
                var newDriver = addDriver.NewDriver;
                _driverService.Add(newDriver);
                LoadDrivers();
                MessageBox.Show("Водитель успешно добавлен!");
            }
        }

        /// <summary>
        /// Удаляет выбранного водителя
        /// </summary>
        public void DeleteDriver()
        {
            if (SelectedDriver == null)
            {
                MessageBox.Show("Выберите водителя для удаления!");
                return;
            }

            var result = MessageBox.Show(
                $"Удалить водителя {SelectedDriver.Surname} {SelectedDriver.Name}?",
                "Подтверждение",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                _driverService.Delete(SelectedDriver);
                LoadDrivers();
                MessageBox.Show("Водитель удален!");
            }
        }

        /// <summary>
        /// Открывает окно просмотра и редактирования водителя
        /// </summary>
        public void OpenDriverProfile()
        {
            if (SelectedDriver == null)
            {
                MessageBox.Show("Выберите водителя!");
                return;
            }

            var viewWindow = new ViewDriverWindow(SelectedDriver);
            if (viewWindow.ShowDialog() == true)
            {
                var updatedDriver = viewWindow.EditedDriver;
                _driverService.Update(updatedDriver);
                LoadDrivers();
                MessageBox.Show("Данные обновлены!");
            }
        }
    }
}
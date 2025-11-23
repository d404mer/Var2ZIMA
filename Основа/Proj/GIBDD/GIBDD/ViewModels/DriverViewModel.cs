using GIBDD.Models;
using GIBDD.Services;
using GIBDD.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GIBDD.ViewModels
{
    public class DriverViewModel : BaseViewModel
    {
        private DataService<Driver> _driverService = new DataService<Driver>();
        private List<Driver> _allDrivers; // Все водители
        private List<Driver> _drivers;    // Отфильтрованный список
        private Driver _selectedDriver;
        private string _searchText;
        private string _selectedCategory;

        // Команды
        public RelayCommand LoadDriversCommand { get; }
        public RelayCommand AddDriversCommand { get; }
        public RelayCommand DeleteDriversCommand { get; }
        public RelayCommand SearchDriversCommand { get; }
        public RelayCommand OpenProfileCommand { get; }
        public RelayCommand FilterByCategoryCommand { get; }

        // Коллекция для привязки к DataGrid
        public List<Driver> Drivers
        {
            get => _drivers;
            set { _drivers = value; OnPropertyChanged(); }
        }

        // Выбранный элемент
        public Driver SelectedDriver
        {
            get => _selectedDriver;
            set { _selectedDriver = value; OnPropertyChanged(); }
        }

        // Текст для поиска
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        // Выбранная категория для фильтрации
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                FilterByCategory(); // Автоматически фильтруем при изменении
            }
        }

        // Список категорий для ComboBox
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

        public DriverViewModel()
        {
            LoadDriversCommand = new RelayCommand(LoadDrivers);
            AddDriversCommand = new RelayCommand(AddDriver);
            DeleteDriversCommand = new RelayCommand(DeleteDriver);
            SearchDriversCommand = new RelayCommand(SearchDrivers);
            OpenProfileCommand = new RelayCommand(OpenDriverProfile);
            FilterByCategoryCommand = new RelayCommand(FilterByCategory);

            LoadDrivers();
        }

        public void LoadDrivers()
        {
            _allDrivers = _driverService.GetAll();
            Drivers = _allDrivers;
            OnPropertyChanged(nameof(Categories)); // Обновляем список категорий
        }

        // Фильтрация по категории
        public void FilterByCategory()
        {
            if (_allDrivers == null) return;

            if (string.IsNullOrEmpty(SelectedCategory) || SelectedCategory == "Все категории")
            {
                Drivers = _allDrivers;
            }
            else
            {
                Drivers = _allDrivers
                    .Where(d => d.Categories.Contains(SelectedCategory))
                    .ToList();
            }
        }

        // Поиск по тексту
        public void SearchDrivers()
        {
            if (_allDrivers == null) return;

            if (string.IsNullOrEmpty(SearchText))
            {
                FilterByCategory(); // Возвращаемся к текущей фильтрации по категории
            }
            else
            {
                Drivers = _allDrivers
                    .Where(d => d.Surname.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                               d.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                               d.Phone.Contains(SearchText) ||
                               d.LicenceNumber.Contains(SearchText))
                    .ToList();
            }
        }

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
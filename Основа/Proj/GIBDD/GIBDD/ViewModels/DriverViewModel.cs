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
    public class DriverViewModel : BaseViewModel
    {
        private DataService<Driver> _driverService = new DataService<Driver>();
        private List<Driver> _allDrivers;
        private ObservableCollection<Driver> _drivers; // Измени на ObservableCollection
        private Driver _selectedDriver;
        private string _searchText;
        private string _selectedCategory;

        // Команды
        public RelayCommand LoadDriversCommand { get; }
        public RelayCommand AddDriversCommand { get; }
        public RelayCommand DeleteDriversCommand { get; }
        public RelayCommand UpdateCommand { get; } // Добавим команду обновления

        // Коллекция для привязки к DataGrid
        public ObservableCollection<Driver> Drivers // Измени тип
        {
            get => _drivers;
            set { _drivers = value; OnPropertyChanged(); }
        }

        // Остальные свойства без изменений...
        public Driver SelectedDriver
        {
            get => _selectedDriver;
            set { _selectedDriver = value; OnPropertyChanged(); }
        }

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
            UpdateCommand = new RelayCommand(LoadDrivers); // Команда обновления

            LoadDrivers();
        }

        public void LoadDrivers()
        {
            _allDrivers = _driverService.GetAll();

            // Вместо присваивания создаем новую ObservableCollection
            Drivers = new ObservableCollection<Driver>(_allDrivers);

            OnPropertyChanged(nameof(Categories));
        }

        // Фильтрация по категории
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

            // Создаем новую коллекцию для обновления DataGrid
            Drivers = new ObservableCollection<Driver>(filteredDrivers);
        }

        // Поиск по тексту
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

        public void AddDriver()
        {
            var addDriver = new AddDriversWindow();
            if (addDriver.ShowDialog() == true)
            {
                var newDriver = addDriver.NewDriver;
                _driverService.Add(newDriver);
                LoadDrivers(); // Теперь это сработает!
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
                LoadDrivers(); // Теперь это сработает!
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
                LoadDrivers(); // Теперь это сработает!
                MessageBox.Show("Данные обновлены!");
            }
        }
    }
}
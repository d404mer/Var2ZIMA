using GIBDD.Models;
using GIBDD.Services;
using GIBDD.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // Добавь это
using System.Linq;
using System.Windows;
using System.Diagnostics;

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
            // Инициализируем коллекцию один раз
            _drivers = new ObservableCollection<Driver>();
            
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
            Debug.WriteLine("[DriverViewModel] LoadDrivers called");
            try
            {
                _allDrivers = _driverService.GetAll();
                Debug.WriteLine($"[DriverViewModel] Loaded {_allDrivers.Count} drivers from DB");
                
                // Применяем текущие фильтры после загрузки
                ApplyFilters();
                
                OnPropertyChanged(nameof(Categories));
                Debug.WriteLine($"[DriverViewModel] LoadDrivers completed. Drivers collection count = {Drivers?.Count ?? 0}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DriverViewModel] LoadDrivers exception: {ex}");
                MessageBox.Show("Ошибка при загрузке списка водителей. Подробности смотрите в Debug Output.");
            }
        }

        /// <summary>
        /// Применяет все активные фильтры (поиск и категория) к загруженным данным
        /// </summary>
        private void ApplyFilters()
        {
            Debug.WriteLine($"[DriverViewModel] ApplyFilters called. SearchText='{SearchText}', SelectedCategory='{SelectedCategory}'");
            
            if (_allDrivers == null)
            {
                Debug.WriteLine("[DriverViewModel] ApplyFilters: _allDrivers is null, clearing collection");
                _drivers.Clear();
                return;
            }

            IEnumerable<Driver> filtered = _allDrivers;

            // Применяем фильтр по категории
            if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "Все категории")
            {
                filtered = filtered.Where(d => d.Categories != null && d.Categories.Contains(SelectedCategory));
                Debug.WriteLine($"[DriverViewModel] Applied category filter '{SelectedCategory}'. Count after category filter: {filtered.Count()}");
            }

            // Применяем поиск
            if (!string.IsNullOrEmpty(SearchText))
            {
                filtered = filtered.Where(d => 
                    (d.Surname != null && d.Surname.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (d.Name != null && d.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (d.Phone != null && d.Phone.Contains(SearchText)) ||
                    (d.LicenceNumber != null && d.LicenceNumber.Contains(SearchText)));
                Debug.WriteLine($"[DriverViewModel] Applied search filter '{SearchText}'. Count after search filter: {filtered.Count()}");
            }

            // Обновляем существующую коллекцию вместо создания новой
            var filteredList = filtered.ToList();
            Debug.WriteLine($"[DriverViewModel] Final filtered count: {filteredList.Count}");
            
            // Очищаем и заполняем существующую коллекцию
            _drivers.Clear();
            foreach (var driver in filteredList)
            {
                _drivers.Add(driver);
            }
            
            // Явно уведомляем об изменении коллекции
            OnPropertyChanged(nameof(Drivers));
            
            Debug.WriteLine($"[DriverViewModel] ApplyFilters completed. Drivers collection updated. Count = {_drivers.Count}");
        }

        /// <summary>
        /// Фильтрует водителей по выбранной категории
        /// </summary>
        public void FilterByCategory()
        {
            Debug.WriteLine($"[DriverViewModel] FilterByCategory called. SelectedCategory = '{SelectedCategory}'");
            ApplyFilters();
        }

        /// <summary>
        /// Выполняет поиск водителей по тексту
        /// </summary>
        private void SearchDrivers()
        {
            Debug.WriteLine($"[DriverViewModel] SearchDrivers called. SearchText = '{SearchText}'");
            ApplyFilters();
        }

        /// <summary>
        /// Открывает окно добавления нового водителя
        /// </summary>
        public void AddDriver()
        {
            Debug.WriteLine("[DriverViewModel] AddDriver called");
            var addDriver = new AddDriversWindow();
            if (addDriver.ShowDialog() == true)
            {
                var newDriver = addDriver.NewDriver;
                Debug.WriteLine($"[DriverViewModel] New driver from dialog: GUID={newDriver?.Guid}, Name={newDriver?.Surname} {newDriver?.Name}");
                
                try
                {
                    _driverService.Add(newDriver);
                    Debug.WriteLine("[DriverViewModel] Driver added successfully. Reloading drivers...");
                    
                    // Сбрасываем фильтры перед обновлением, чтобы новый водитель был виден
                    SearchText = string.Empty;
                    SelectedCategory = "Все категории";
                    
                    LoadDrivers();
                    
                    // Явно уведомляем об изменении коллекции
                    OnPropertyChanged(nameof(Drivers));
                    
                    Debug.WriteLine($"[DriverViewModel] Drivers collection after add: Count = {Drivers?.Count ?? 0}");
                    MessageBox.Show("Водитель успешно добавлен!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[DriverViewModel] Error adding driver: {ex}");
                    MessageBox.Show($"Ошибка при добавлении водителя: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("[DriverViewModel] AddDriversWindow dialog canceled");
            }
        }

        /// <summary>
        /// Удаляет выбранного водителя
        /// </summary>
        public void DeleteDriver()
        {
            if (SelectedDriver == null)
            {
                Debug.WriteLine("[DriverViewModel] DeleteDriver called with null SelectedDriver");
                MessageBox.Show("Выберите водителя для удаления!");
                return;
            }

            var result = MessageBox.Show(
                $"Удалить водителя {SelectedDriver.Surname} {SelectedDriver.Name}?",
                "Подтверждение",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Debug.WriteLine($"[DriverViewModel] Deleting driver GUID={SelectedDriver.Guid}, Name={SelectedDriver.Surname} {SelectedDriver.Name}");
                _driverService.Delete(SelectedDriver);
                Debug.WriteLine("[DriverViewModel] Reloading drivers after delete");
                LoadDrivers();
                MessageBox.Show("Водитель удален!");
            }
            else
            {
                Debug.WriteLine("[DriverViewModel] Delete cancelled by user");
            }
        }

        /// <summary>
        /// Открывает окно просмотра и редактирования водителя
        /// </summary>
        public void OpenDriverProfile()
        {
            if (SelectedDriver == null)
            {
                Debug.WriteLine("[DriverViewModel] OpenDriverProfile called with null SelectedDriver");
                MessageBox.Show("Выберите водителя!");
                return;
            }

            Debug.WriteLine($"[DriverViewModel] Opening ViewDriverWindow for GUID={SelectedDriver.Guid}, Name={SelectedDriver.Surname} {SelectedDriver.Name}");
            var viewWindow = new ViewDriverWindow(SelectedDriver);
            if (viewWindow.ShowDialog() == true)
            {
                var updatedDriver = viewWindow.EditedDriver;
                Debug.WriteLine($"[DriverViewModel] Updating driver GUID={updatedDriver?.Guid}, Name={updatedDriver?.Surname} {updatedDriver?.Name}");
                
                try
                {
                    _driverService.Update(updatedDriver);
                    Debug.WriteLine("[DriverViewModel] Driver updated successfully. Reloading drivers...");
                    
                    LoadDrivers();
                    
                    // Явно уведомляем об изменении коллекции
                    OnPropertyChanged(nameof(Drivers));
                    
                    Debug.WriteLine($"[DriverViewModel] Drivers collection after update: Count = {Drivers?.Count ?? 0}");
                    MessageBox.Show("Данные обновлены!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[DriverViewModel] Error updating driver: {ex}");
                    MessageBox.Show($"Ошибка при обновлении водителя: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("[DriverViewModel] ViewDriverWindow dialog canceled");
            }
        }
    }
}
using GIBDD.Models;
using GIBDD.Services;
using GIBDD.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GIBDD.ViewModels
{
    public class DriverViewModel : BaseViewModel
    {
        private DataService<Driver> _driverService = new DataService<Driver>();

        private List<Driver> _drivers;

        private Driver _selectedDriver;

        private string _searchText;

        // обязательный минимум команд (CRUD-операции для конкретной модели)
        public RelayCommand LoadDriversCommand { get; }
        public RelayCommand AddDriversCommand { get; }
        public RelayCommand DeleteDriversCommand { get; }
        public RelayCommand SearchDriversCommand { get; }

        public RelayCommand OpenProfileCommand { get; }





        // коллекция для привязки к DataGrid
        public List<Driver> Drivers
        {
            get => _drivers;
            set { _drivers = value; OnPropertyChanged(); }
        }

        // выбранный элемент
        public Driver SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                _selectedDriver = value; OnPropertyChanged();
            }
        }

        // текст для поиска
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }


        public DriverViewModel()
        {
            LoadDriversCommand = new RelayCommand(LoadDrivers);
            AddDriversCommand = new RelayCommand(AddDriver);
            DeleteDriversCommand = new RelayCommand(DeleteDriver);
            //SearchDriversCommand = new RelayCommand(SearchDrivers);
            OpenProfileCommand = new RelayCommand(OpenDriverProfile); // если нужна отдельная команда

            LoadDrivers(); // загружаем данные сразу при создании
        }

        public void LoadDrivers() => Drivers = _driverService.GetAll();

        public void AddDriver()
        {
            var addDriver = new AddDriversWindow();

            if (addDriver.ShowDialog() == true)
            {
                // Берем созданного водителя из окна
                var newDriver = addDriver.NewDriver;

                // Сохраняем в БД
                _driverService.Add(newDriver);
                LoadDrivers(); // Обновляем список

                MessageBox.Show("Водитель успешно добавлен!");
            }
        }

        public void DeleteDriver()
        {

            _driverService.Delete(SelectedDriver);
            LoadDrivers();

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
                // Сохраняем изменения в БД
                var updatedDriver = viewWindow.EditedDriver;
                _driverService.Update(updatedDriver); // или UpdateByGuid если есть такой метод
                LoadDrivers(); // Обновляем список
                MessageBox.Show("Данные водителя обновлены!");
            }
        }


        //public void SearchDrivers()
        //{
        //    if (string.IsNullOrEmpty(SearchText))
        //        LoadDrivers();
        //    else
        //        Drivers = _driverService.Search(SearchText);
        //}
    }
}

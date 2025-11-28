# 📚 Шаблон CRUD приложения на WPF + Entity Framework

## 🎯 Структура проекта для экзамена

Этот документ описывает стандартную структуру CRUD приложения, которую можно легко запомнить и воспроизвести на экзамене.

---

## 📁 Архитектура проекта (MVVM Pattern)

```
GIBDD/
├── Models/              # Модели данных (Entity Framework)
│   ├── Driver.cs
│   ├── Car.cs
│   └── GibddDbContext.cs
│
├── Services/            # Сервисы для работы с данными
│   ├── DataService.cs   # Универсальный CRUD сервис
│   └── AuthService.cs
│
├── ViewModels/          # ViewModel (логика представления)
│   ├── BaseViewModel.cs # Базовый класс с INotifyPropertyChanged
│   ├── DriverViewModel.cs
│   └── CarViewModel.cs
│
└── Views/               # Окна (XAML + Code-Behind)
    ├── DriversWindow.xaml
    ├── DriversWindow.xaml.cs
    ├── AddDriversWindow.xaml
    └── AddDriversWindow.xaml.cs
```

---

## 🔑 Ключевые компоненты

### 1. **BaseViewModel** - Основа для всех ViewModel

**Назначение:** Реализует `INotifyPropertyChanged` для автоматического обновления UI

```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
    // RelayCommand для команд
    public class RelayCommand : ICommand { ... }
}
```

**Зачем нужен:** Избегаем дублирования кода в каждой ViewModel

---

### 2. **DataService<T>** - Универсальный CRUD сервис

**Назначение:** Универсальный сервис для работы с любой сущностью через Entity Framework

**Основные методы:**
- `GetAll()` - получить все записи
- `Add(T item)` - добавить запись
- `Update(T item)` - обновить запись
- `Delete(T item)` - удалить запись
- `Search(string keyword)` - поиск

**Паттерн использования:**
```csharp
var service = new DataService<Driver>();
var drivers = service.GetAll();  // Чтение
service.Add(newDriver);          // Создание
service.Update(driver);          // Обновление
service.Delete(driver);          // Удаление
```

**Важно:** Каждый метод создает новый контекст БД для избежания кэширования

---

### 3. **ViewModel** - Логика представления

**Структура типичной ViewModel:**

```csharp
public class DriverViewModel : BaseViewModel
{
    // 1. Сервис для работы с данными
    private DataService<Driver> _driverService = new DataService<Driver>();
    
    // 2. Приватные поля
    private List<Driver> _allDrivers;           // Все данные из БД
    private ObservableCollection<Driver> _drivers; // Отображаемая коллекция
    private Driver _selectedDriver;             // Выбранный элемент
    
    // 3. Публичные свойства с OnPropertyChanged
    public ObservableCollection<Driver> Drivers
    {
        get => _drivers;
        set { _drivers = value; OnPropertyChanged(); }
    }
    
    // 4. Команды
    public RelayCommand AddDriversCommand { get; }
    public RelayCommand DeleteDriversCommand { get; }
    
    // 5. Конструктор - инициализация
    public DriverViewModel()
    {
        _drivers = new ObservableCollection<Driver>(); // Важно!
        AddDriversCommand = new RelayCommand(AddDriver);
        LoadDrivers(); // Загружаем данные при создании
    }
    
    // 6. CRUD методы
    public void LoadDrivers() { ... }
    public void AddDriver() { ... }
    public void DeleteDriver() { ... }
    public void OpenDriverProfile() { ... }
}
```

**Паттерн CRUD операций:**

```csharp
// CREATE
public void AddDriver()
{
    var addWindow = new AddDriversWindow();
    if (addWindow.ShowDialog() == true)
    {
        _driverService.Add(addWindow.NewDriver);
        LoadDrivers(); // Обновляем список
    }
}

// READ
public void LoadDrivers()
{
    _allDrivers = _driverService.GetAll();
    ApplyFilters(); // Применяем фильтры
}

// UPDATE
public void OpenDriverProfile()
{
    var viewWindow = new ViewDriverWindow(SelectedDriver);
    if (viewWindow.ShowDialog() == true)
    {
        _driverService.Update(viewWindow.EditedDriver);
        LoadDrivers();
    }
}

// DELETE
public void DeleteDriver()
{
    if (MessageBox.Show(...) == MessageBoxResult.Yes)
    {
        _driverService.Delete(SelectedDriver);
        LoadDrivers();
    }
}
```

---

### 4. **View (Window)** - Представление

**Структура окна:**

```csharp
public partial class DriversWindow : Window
{
    private DriverViewModel _viewModel;
    
    public DriversWindow()
    {
        InitializeComponent();
        _viewModel = new DriverViewModel();
        this.DataContext = _viewModel; // Привязка данных
    }
    
    // Обработчики событий
    private void Add_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.AddDriver();
        RefreshDataGrid(); // Обновляем UI
    }
    
    private void RefreshDataGrid()
    {
        DriversDataGrid.Items.Refresh();
    }
}
```

**XAML привязка:**

```xml
<DataGrid ItemsSource="{Binding Drivers}" 
          SelectedItem="{Binding SelectedDriver, Mode=TwoWay}"/>
          
<Button Click="Add_Click" Content="Добавить"/>
```

---

## 🔄 Паттерн работы с ObservableCollection

**Проблема:** DataGrid не обновляется при создании новой коллекции

**Решение:** Обновляем существующую коллекцию через `Clear()` и `Add()`

```csharp
// ❌ ПЛОХО - создает новую коллекцию
Drivers = new ObservableCollection<Driver>(filteredList);

// ✅ ХОРОШО - обновляет существующую
_drivers.Clear();
foreach (var item in filteredList)
{
    _drivers.Add(item);
}
OnPropertyChanged(nameof(Drivers));
```

---

## 📝 Чек-лист для создания нового CRUD модуля

### Шаг 1: Модель (если нужно)
```csharp
public class MyEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### Шаг 2: ViewModel
```csharp
public class MyEntityViewModel : BaseViewModel
{
    private DataService<MyEntity> _service = new DataService<MyEntity>();
    private ObservableCollection<MyEntity> _items;
    
    public ObservableCollection<MyEntity> Items
    {
        get => _items;
        set { _items = value; OnPropertyChanged(); }
    }
    
    public MyEntityViewModel()
    {
        _items = new ObservableCollection<MyEntity>();
        LoadItems();
    }
    
    public void LoadItems() { ... }
    public void AddItem() { ... }
    public void DeleteItem() { ... }
    public void UpdateItem() { ... }
}
```

### Шаг 3: Окно списка (ListWindow)
```csharp
public partial class MyEntityWindow : Window
{
    private MyEntityViewModel _viewModel;
    
    public MyEntityWindow()
    {
        InitializeComponent();
        _viewModel = new MyEntityViewModel();
        this.DataContext = _viewModel;
    }
}
```

### Шаг 4: Окно добавления (AddWindow)
```csharp
public partial class AddMyEntityWindow : Window
{
    public MyEntity NewEntity { get; private set; }
    
    private void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        // Валидация
        // Создание объекта
        NewEntity = new MyEntity { ... };
        this.DialogResult = true;
        this.Close();
    }
}
```

### Шаг 5: Окно редактирования (ViewWindow)
```csharp
public partial class ViewMyEntityWindow : Window
{
    public MyEntity EditedEntity { get; private set; }
    
    public ViewMyEntityWindow(MyEntity entity)
    {
        InitializeComponent();
        // Заполняем поля из entity
    }
    
    private void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        // Обновляем EditedEntity
        this.DialogResult = true;
        this.Close();
    }
}
```

---

## 🎓 Ключевые моменты для запоминания

1. **BaseViewModel** - всегда наследуемся от него
2. **DataService<T>** - универсальный сервис, один для всех сущностей
3. **ObservableCollection** - инициализируем в конструкторе, обновляем через Clear/Add
4. **OnPropertyChanged()** - вызываем при изменении свойств
5. **DialogResult** - используем для диалоговых окон (Add/Edit)
6. **RefreshDataGrid()** - принудительное обновление UI после операций

---

## 🔧 Типичные проблемы и решения

### Проблема: DataGrid не обновляется после добавления
**Решение:** 
- Использовать `Clear()` и `Add()` вместо создания новой коллекции
- Вызывать `RefreshDataGrid()` после операций
- Использовать `AsNoTracking()` в `GetAll()` для свежих данных

### Проблема: Ошибка "Entity is already being tracked"
**Решение:** Создавать новый контекст БД в каждом методе DataService

### Проблема: Привязка данных не работает
**Решение:** 
- Убедиться, что `DataContext` установлен
- Проверить, что свойства вызывают `OnPropertyChanged()`
- Использовать `Mode=TwoWay` для двусторонней привязки

---

## 📌 Формула успеха на экзамене

```
1. BaseViewModel (INotifyPropertyChanged)
2. DataService<T> (CRUD операции)
3. ViewModel (логика + ObservableCollection)
4. View (Window + XAML привязка)
5. AddWindow / ViewWindow (диалоги)
```

**Помните:** Структура всегда одинаковая, меняются только названия сущностей!

---

## 🚀 Быстрый старт

1. Создай модель (если нужно)
2. Создай ViewModel с CRUD методами
3. Создай окно списка с DataGrid
4. Создай окна Add/Edit
5. Привяжи всё через DataContext

**Готово!** У тебя есть рабочий CRUD модуль.


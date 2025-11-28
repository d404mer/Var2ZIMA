using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GIBDD.ViewModels
{
    /// <summary>
    /// Базовый класс для всех ViewModel, реализующий INotifyPropertyChanged
    /// 
    /// НАЗНАЧЕНИЕ: Избегаем дублирования кода в каждой ViewModel
    /// 
    /// ИСПОЛЬЗОВАНИЕ:
    /// public class MyViewModel : BaseViewModel
    /// {
    ///     private string _name;
    ///     public string Name
    ///     {
    ///         get => _name;
    ///         set { _name = value; OnPropertyChanged(); } // Автоматически обновляет UI
    ///     }
    /// }
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие, возникающее при изменении свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged для указанного свойства
        /// </summary>
        /// <param name="name">Имя измененного свойства</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            try
            {

                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            catch { }
        }

        /// <summary>
        /// Простая команда для выполнения действий
        /// </summary>
        public class RelayCommand : ICommand
        {
            private Action _execute;

            /// <summary>
            /// Конструктор RelayCommand
            /// </summary>
            /// <param name="execute">Действие для выполнения</param>
            public RelayCommand(Action execute) => _execute = execute;

            /// <summary>
            /// Событие изменения возможности выполнения команды
            /// </summary>
            public event EventHandler CanExecuteChanged;

            /// <summary>
            /// Определяет, может ли команда выполняться
            /// </summary>
            /// <param name="parameter">Параметр команды</param>
            /// <returns>Всегда возвращает true</returns>
            public bool CanExecute(object parameter) => true;

            /// <summary>
            /// Выполняет команду
            /// </summary>
            /// <param name="parameter">Параметр команды</param>
            public void Execute(object parameter) => _execute();
        }
    }
}

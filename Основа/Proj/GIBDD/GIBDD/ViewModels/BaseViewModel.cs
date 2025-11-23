using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GIBDD.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Простая команда для запоминания
        public class RelayCommand : ICommand
        {
            private Action _execute;

            public RelayCommand(Action execute) => _execute = execute;

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter) => _execute();
        }
    }
}

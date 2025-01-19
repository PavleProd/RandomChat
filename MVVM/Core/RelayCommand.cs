using System.Windows.Input;

namespace ChatClient.MVVM.Core
{
    class RelayCommand : ICommand
    {
        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this._execute(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;  }
            remove {  CommandManager.RequerySuggested -= value; }
        }

        private readonly Action<object> _execute;
        private readonly Func<object, bool>? _canExecute;
    }
}

using System;
using System.Windows.Input;

namespace DotNetToolBox.WPF.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _executeAction;
        private bool _canExecuteCache = true;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return _canExecuteCache;
            var temp = _canExecute(parameter);
            if (_canExecuteCache == temp) return _canExecuteCache;
            _canExecuteCache = temp;
            if (CanExecuteChanged != null) CanExecuteChanged.Invoke(this, new EventArgs());
            return _canExecuteCache;
        }

        public event EventHandler CanExecuteChanged;
    }
}
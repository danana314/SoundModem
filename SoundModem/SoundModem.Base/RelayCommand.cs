using System;
using System.Diagnostics;
using System.Windows.Input;

namespace SoundModem.Base
{
    public class RelayCommand : ICommand
    {
        protected readonly Action<object> _execute;
        protected Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            //Debug.Assert(execute != null, "Null Execution Parameter");

            this._execute = execute;
            this._canExecute = canExecute;
        }

        public virtual void Execute(object parameter)
        {
            this._execute(parameter);
        }

        public void SetCanExecute(Predicate<object> canExecute)
        {
            this._canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public virtual bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}

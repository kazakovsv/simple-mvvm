using System;
using System.Windows.Input;

namespace SimpleMVVM.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> m_execute;
        private readonly Predicate<object> m_canExecute;

        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            m_execute = execute ?? throw new ArgumentNullException(nameof(execute));
            m_canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecute == null || m_canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            m_execute(parameter);
        }
    }
}

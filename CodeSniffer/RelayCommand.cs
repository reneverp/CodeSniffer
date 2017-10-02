

using System;
using System.Windows.Input;

namespace CodeSniffer
{
    class RelayCommand : ICommand
    {
        private Action _actionToBeExecuted;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action actionToBeExecuted)
        {
            _actionToBeExecuted = actionToBeExecuted;
        }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteChanged != null)
            {
                //do nothing
            }

            return true;
        }

        public void Execute(object parameter)
        {
            _actionToBeExecuted();
        }
    }

    class RelayCommand<T> : ICommand
    {
        private Action<T> _actionToBeExecuted;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> actionToBeExecuted)
        {
            _actionToBeExecuted = actionToBeExecuted;
        }

        public bool CanExecute(object parameter)
        {
            if(CanExecuteChanged != null)
            {
                //do nothing
            }

            return true;
        }

        public void Execute(object parameter)
        {
            _actionToBeExecuted((T)parameter);
        }
    }
}

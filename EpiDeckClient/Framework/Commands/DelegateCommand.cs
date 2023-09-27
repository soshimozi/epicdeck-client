using System.Windows.Input;
using System;

namespace EpiDeckClient.Framework.Commands
{
    public class DelegateCommand : ICommand
    {
        public Action<object?>? CommandAction { get; set; }
        public Func<object?, bool>? CanExecuteFunc { get; set; }

        public void Execute(object? parameter = null)
        {
            CommandAction?.Invoke(parameter);
        }

        public bool CanExecute(object? parameter = null)
        {
            return CanExecuteFunc == null || CanExecuteFunc(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
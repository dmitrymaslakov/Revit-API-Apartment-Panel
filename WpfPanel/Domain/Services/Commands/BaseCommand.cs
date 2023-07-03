using System;
using System.Windows.Input;

namespace WpfPanel.Domain.Services.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public abstract void Execute(object parameter);
    }
}

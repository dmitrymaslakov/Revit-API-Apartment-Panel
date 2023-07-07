using System;

namespace WpfPanel.Domain.Services.Commands
{
    public class RelayCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute) => _execute = execute;

        public override void Execute(object parameter) => _execute(parameter);
    }
}

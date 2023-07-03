using System;

namespace WpfPanel.Domain.Services.Commands
{
    public class RemoveApartmentElementCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public RemoveApartmentElementCommand(Action<object> execute) => _execute = execute; 

        public override void Execute(object parameter) => _execute(parameter);
    }
}

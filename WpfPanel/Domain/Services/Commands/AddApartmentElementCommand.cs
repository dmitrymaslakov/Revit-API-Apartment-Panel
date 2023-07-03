using System;

namespace WpfPanel.Domain.Services.Commands
{
    public class AddApartmentElementCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public AddApartmentElementCommand(Action<object> execute) => _execute = execute;

        public override void Execute(object parameter) => _execute(parameter);
    }
}

using System;

namespace WpfPanel.Domain.Services.Commands
{
    public class EditApartmentElementCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public EditApartmentElementCommand(Action<object> execute) => _execute = execute; 

        public override void Execute(object parameter) => _execute(parameter);
    }
}

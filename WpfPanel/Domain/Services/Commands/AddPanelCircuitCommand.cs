using System;

namespace WpfPanel.Domain.Services.Commands
{
    public class AddPanelCircuitCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public AddPanelCircuitCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}

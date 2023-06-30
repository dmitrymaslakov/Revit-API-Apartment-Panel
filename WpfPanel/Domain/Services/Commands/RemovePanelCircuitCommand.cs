using System;

namespace WpfPanel.Domain.Services.Commands
{
    public class RemovePanelCircuitCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public RemovePanelCircuitCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}

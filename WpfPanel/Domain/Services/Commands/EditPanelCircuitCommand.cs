using System;

namespace WpfPanel.Domain.Services.Commands
{
    public class EditPanelCircuitCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public EditPanelCircuitCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}

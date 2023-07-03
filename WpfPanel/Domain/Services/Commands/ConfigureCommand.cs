using System;
using WpfPanel.View.Components;
using WpfPanel.ViewModel.ComponentsVM;

namespace WpfPanel.Domain.Services.Commands
{
    public class ConfigureCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public ConfigureCommand(Action<object> execute) => _execute = execute;

        public override void Execute(object parameter) => _execute(parameter);
    }
}

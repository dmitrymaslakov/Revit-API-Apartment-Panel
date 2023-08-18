using System;
using DockableDialogs.Domain.Services.Commands;

namespace DockableDialogs.Domain
{
    public class PlacementFixturiesCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public PlacementFixturiesCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}

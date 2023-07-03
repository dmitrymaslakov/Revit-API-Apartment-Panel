using System;

namespace DockableDialogs.Domain
{
    public class TestCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public TestCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}

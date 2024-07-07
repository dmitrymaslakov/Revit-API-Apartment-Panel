using System;
using System.Threading.Tasks;
using System.Windows;

namespace WpfTest.Commands
{
    public class RelayCommandAsync : BaseCommand
    {
        private readonly Func<object, Task> _execute;

        public RelayCommandAsync(Func<object, Task> execute) =>
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

        public async override void Execute(object parameter)
        {
            try
            {
                var r = _execute(parameter);
                await r;
            }
            catch (Exception ex)
            {
                MessageBox.Show("RelayCommand_Exception", ex.Message);
            }
        }
    }
}

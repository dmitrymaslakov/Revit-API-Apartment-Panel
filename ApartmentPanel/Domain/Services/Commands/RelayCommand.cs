using Autodesk.Revit.UI;
using System;

namespace ApartmentPanel.Domain.Services.Commands
{
    public class RelayCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute) => 
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

        public override void Execute(object parameter)
        {
            try
            {
                _execute(parameter);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("RelayCommand_Exception", ex.Message);
            }
        }
    }
}

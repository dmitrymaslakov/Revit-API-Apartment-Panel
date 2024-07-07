using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;

namespace ApartmentPanel.Presentation.Commands.ConfigPanelCommands
{
    public class CircuitElementsCommandCreater : BaseCommandsCreater
    {
        private readonly IConfigPanelViewModel _configPanelVM;

        public CircuitElementsCommandCreater(IConfigPanelViewModel configPanelVM) 
            => _configPanelVM = configPanelVM;

        public ICommand CreateSelectCircuitElementCommand() => new RelayCommand(o =>
        {
            var circuitElements = (o as IList<object>)?.OfType<IApartmentElement>();
            if (_configPanelVM.CircuitElementsVM.SelectedCircuitElements.Count != 0)
                _configPanelVM.CircuitElementsVM.SelectedCircuitElements.Clear();

            if (circuitElements.Count() != 0)
                foreach (var circuitElement in circuitElements)
                    _configPanelVM.CircuitElementsVM.SelectedCircuitElements.Add(circuitElement);
        });
    }
}

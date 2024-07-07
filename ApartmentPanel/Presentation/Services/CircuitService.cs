using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using System.Collections.ObjectModel;

namespace ApartmentPanel.Presentation.Services
{
    public class CircuitService
    {
        private readonly IConfigPanelViewModel _configPanelViewModel;

        public CircuitService(IConfigPanelViewModel configPanelProperties) => 
            _configPanelViewModel = configPanelProperties;

        public void AddCurrentCircuitElements(ObservableCollection<IApartmentElement> currentCircuitElements)
        {
            if (_configPanelViewModel.CircuitElementsVM.CircuitElements.Count != 0)
                _configPanelViewModel.CircuitElementsVM.CircuitElements.Clear();

            foreach (var item in currentCircuitElements)
                _configPanelViewModel.CircuitElementsVM.CircuitElements.Add(item);
        }
    }
}

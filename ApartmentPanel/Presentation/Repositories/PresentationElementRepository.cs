using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Presentation.Interfaces;
using ApartmentPanel.Presentation.Services;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ApartmentPanel.Infrastructure.Repositories
{
    public class PresentationElementRepository : IPresentationElementRepository
    {
        private readonly IConfigPanelViewModel _configPanelViewModel;
        private readonly CircuitService _circuitService;

        public PresentationElementRepository(IConfigPanelViewModel configPanelViewModel)
        {
            _configPanelViewModel = configPanelViewModel;
            _circuitService = new CircuitService(_configPanelViewModel);
        }

        /*public void RemoveFromApartment()
        {
            foreach (var element in _configPanelViewModel.SelectedApartmentElements.ToArray())
                _configPanelViewModel.ApartmentElements.Remove(element);
        }*/
        /*public void RemoveFromCircuit()
        {
            var selectedPanelCircuit = _configPanelViewModel.SelectedPanelCircuits.SingleOrDefault();
            if (string.IsNullOrEmpty(selectedPanelCircuit.Key)) return;

            foreach (var selectedCircuitElement in _configPanelViewModel.SelectedCircuitElements)
                selectedPanelCircuit.Value.Remove(selectedCircuitElement);

            _circuitService.AddCurrentCircuitElements(selectedPanelCircuit.Value);
        }*/
        /*public void AddToCircuit()
        {
            var selectedPanelCircuit = _configPanelViewModel.SelectedPanelCircuits.SingleOrDefault();

            foreach (var selectedApartmentElement in _configPanelViewModel.SelectedApartmentElements)
            {
                if (selectedApartmentElement == null
                || string.IsNullOrEmpty(selectedPanelCircuit.Key))
                    return;

                var IsElementExist = _configPanelViewModel.PanelCircuits
                .Where(e => e.Key == selectedPanelCircuit.Key)
                .First()
                .Value
                .Select(ae => ae.Name)
                .Contains(selectedApartmentElement.Name);

                if (!IsElementExist)
                    _configPanelViewModel.PanelCircuits[selectedPanelCircuit.Key].Add(selectedApartmentElement);

                _circuitService.AddCurrentCircuitElements(_configPanelViewModel.PanelCircuits[selectedPanelCircuit.Key]);
            }
        }*/
    }
}

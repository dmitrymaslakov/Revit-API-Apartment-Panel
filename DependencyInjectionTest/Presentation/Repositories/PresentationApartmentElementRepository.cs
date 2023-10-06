using DependencyInjectionTest.Core.Models.Interfaces;
using DependencyInjectionTest.Core.Presentation.Interfaces;
using DependencyInjectionTest.Presentation.View.Components;
using DependencyInjectionTest.Presentation.ViewModel.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DependencyInjectionTest.Infrastructure.Repositories
{
    public class PresentationApartmentElementRepository : IPresentationApartmentElementRepository
    {
        private readonly IConfigPanelViewModel _configPanelViewModel;

        public PresentationApartmentElementRepository(IConfigPanelViewModel configPanelViewModel) 
            => _configPanelViewModel = configPanelViewModel;

        public void RemoveElementsFromApartment()
        {
            foreach (var element in _configPanelViewModel.SelectedApartmentElements.ToArray())
                _configPanelViewModel.ApartmentElements.Remove(element);
        }
        public void AddToCircuit(IApartmentElement apartmentElement)
        {
            KeyValuePair<string, ObservableCollection<IApartmentElement>> selectedPanelCircuit = 
                _configPanelViewModel.SelectedPanelCircuits.FirstOrDefault();

            _configPanelViewModel.PanelCircuits[selectedPanelCircuit.Key].Add(apartmentElement);
        }
    }
}

using DependencyInjectionTest.Core.Models.Interfaces;
using DependencyInjectionTest.Core.Presentation.Interfaces;
using DependencyInjectionTest.Presentation.View.Components;
using DependencyInjectionTest.Presentation.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

namespace DependencyInjectionTest.Infrastructure.Repositories
{
    public class PresentationPanelRepository : IPresentationPanelRepository
    {
        private readonly IConfigPanelViewModel _configPanelViewModel;

        public PresentationPanelRepository(IConfigPanelViewModel configPanelViewModel) 
            => _configPanelViewModel = configPanelViewModel;

        public void AddCircuitToPanel()
        {
            _configPanelViewModel.PanelCircuits.Add(_configPanelViewModel.NewCircuit,
                    new ObservableCollection<IApartmentElement>());
        }

        public void Configure() => new ConfigPanel(_configPanelViewModel).Show();

        public void RemoveCircuitsFromPanel()
        {
            foreach (var circuit in _configPanelViewModel.SelectedPanelCircuits.ToArray())
                _configPanelViewModel.PanelCircuits.Remove(circuit.Key);
        }
    }
}

using DependencyInjectionTest.Core.Presentation.Interfaces;
using DependencyInjectionTest.Core.Services.Interfaces;

namespace DependencyInjectionTest.Core.Services
{
    public class PanelService : IPanelService
    {
        private readonly IPresentationPanelRepository _panelRepository;

        public PanelService(IPresentationPanelRepository panelRepository) =>
            _panelRepository = panelRepository;

        public void AddCircuit()
        {
            _panelRepository.AddCircuitToPanel();
        }

        public void Configure() => _panelRepository.Configure();

        public void RemoveCircuits()
        {
            _panelRepository.RemoveCircuitsFromPanel();
        }
    }
}

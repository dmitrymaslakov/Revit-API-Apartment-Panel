using DependencyInjectionTest.Core.Models.Interfaces;

namespace DependencyInjectionTest.Core.Presentation.Interfaces
{
    public interface IPresentationPanelRepository
    {
        void RemoveCircuitsFromPanel();
        void AddCircuitToPanel();
        void Configure();
    }
}
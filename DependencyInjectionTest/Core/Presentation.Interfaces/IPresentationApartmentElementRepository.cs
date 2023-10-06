
using DependencyInjectionTest.Core.Models.Interfaces;

namespace DependencyInjectionTest.Core.Presentation.Interfaces
{
    public interface IPresentationApartmentElementRepository
    {
        void RemoveElementsFromApartment();
        void AddToCircuit(IApartmentElement apartmentElement);
    }
}
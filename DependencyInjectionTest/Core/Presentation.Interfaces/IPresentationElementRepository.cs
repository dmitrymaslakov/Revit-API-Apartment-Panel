
using DependencyInjectionTest.Core.Models.Interfaces;

namespace DependencyInjectionTest.Core.Presentation.Interfaces
{
    public interface IPresentationElementRepository
    {
        void RemoveFromApartment();
        void RemoveFromCircuit();
        void AddToCircuit();
    }
}
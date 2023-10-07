using DependencyInjectionTest.Core.Infrastructure.Interfaces;
using DependencyInjectionTest.Core.Models.Interfaces;
using DependencyInjectionTest.Core.Presentation.Interfaces;
using DependencyInjectionTest.Core.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace DependencyInjectionTest.Core.Services
{
    public class ElementService : IElementService
    {
        private readonly IInfrastructureElementRepository _infrApartmentElementRepo;
        private readonly IPresentationElementRepository _presentApartmentElementRepo;

        public ElementService(IInfrastructureElementRepository infrApartmentElementRepo,
            IPresentationElementRepository presentApartmentElementRepo)
        {
            _infrApartmentElementRepo = infrApartmentElementRepo;
            _presentApartmentElementRepo = presentApartmentElementRepo;
        }

        public void AddToApartment(Action<IApartmentElement> addElementToApartment) => 
            _infrApartmentElementRepo.AddToApartment(addElementToApartment);
        public void AddToCircuit() => 
            _presentApartmentElementRepo.AddToCircuit();
        public void InsertToModel(Dictionary<string, string> apartmentElementDto) => 
            _infrApartmentElementRepo.InsertToModel(apartmentElementDto);
        public void RemoveFromApartment() => _presentApartmentElementRepo.RemoveFromApartment();
        public void RemoveFromCircuit() => _presentApartmentElementRepo.RemoveFromCircuit();
    }
}

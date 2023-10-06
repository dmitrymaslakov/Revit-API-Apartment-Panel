using DependencyInjectionTest.Core.Infrastructure.Interfaces;
using DependencyInjectionTest.Core.Models.Interfaces;
using DependencyInjectionTest.Core.Presentation.Interfaces;
using DependencyInjectionTest.Core.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace DependencyInjectionTest.Core.Services
{
    public class ApartmentElementService : IApartmentElementService
    {
        private readonly IInfrastructureApartmentElementRepository _infrApartmentElementRepo;
        private readonly IPresentationApartmentPanelRepository _presentApartmentElementRepo;

        public ApartmentElementService(IInfrastructureApartmentElementRepository infrApartmentElementRepo, 
            IPresentationApartmentPanelRepository presentApartmentElementRepo)
        {
            _infrApartmentElementRepo = infrApartmentElementRepo;
            _presentApartmentElementRepo = presentApartmentElementRepo;
        }

        public void AddToApartment(Action<IApartmentElement> addElementToApartment)
        {
            _infrApartmentElementRepo.AddToApartment(addElementToApartment);
        }
        public void Insert(Dictionary<string, string> apartmentElementDto)
        {
            _infrApartmentElementRepo.InsertElement(apartmentElementDto);
        }
        public void RemoveFromApartment()
        {
            _presentApartmentElementRepo.RemoveFromApartment(apartmentElement);
        }
    }
}

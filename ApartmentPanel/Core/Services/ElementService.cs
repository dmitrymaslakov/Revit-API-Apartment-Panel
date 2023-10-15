using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ApartmentPanel.Core.Services
{
    public class ElementService : IElementService
    {
        private readonly IInfrastructureElementRepository _infrApartmentElementRepo;

        public ElementService(IInfrastructureElementRepository infrApartmentElementRepo)
        {
            _infrApartmentElementRepo = infrApartmentElementRepo;
        }

        public void AddToApartment(Action<IApartmentElement> addElementToApartment) => 
            _infrApartmentElementRepo.AddToApartment(addElementToApartment);

        public JsonSerializerOptions GetSerializationOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new TypeMappingConverter<IApartmentElement, ApartmentElement>()
                }
            };

        }

        public void InsertToModel(Dictionary<string, string> apartmentElementDto) => 
            _infrApartmentElementRepo.InsertToModel(apartmentElementDto);
    }
}

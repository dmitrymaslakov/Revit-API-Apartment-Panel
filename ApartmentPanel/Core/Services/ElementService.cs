using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ApartmentPanel.Core.Services
{
    public class ElementService : IElementService
    {
        private readonly IInfrastructureElementRepository _elementRepo;

        public ElementService(IInfrastructureElementRepository elementRepo)
        {
            _elementRepo = elementRepo;
        }

        /*public List<IApartmentElement> GetAllByCategory(List<string> categories)
        {
            List<(string name, string category)> props = 
                _elementRepo.GetPropertiesByCategory(categories);

            return props
                .Select(p => new ApartmentElement { Name = p.name, Category = p.category})
                .Cast<IApartmentElement>()
                .ToList();
        }*/

        public List<IApartmentElement> GetAll(List<(string name, string category)> props)
        {
            return props
                .Select(p => new ApartmentElement { Name = p.name, Category = p.category })
                .Cast<IApartmentElement>()
                .ToList();
        }


        public void AddToApartment(Action<List<(string name, string category)>> addElementsToApartment)
        {
            _elementRepo.AddToApartment(addElementsToApartment);
        }
        /*public void AddToApartment(Action<IApartmentElement> addElementToApartment) => 
            _elementRepo.AddToApartment(addElementToApartment);*/

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
            _elementRepo.InsertToModel(apartmentElementDto);
    }
}

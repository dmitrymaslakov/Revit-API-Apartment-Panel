using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Utility.AnnotationUtility;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Core.Services
{
    public class ElementService : IElementService
    {
        private readonly IElementRepository _elementRepo;

        public ElementService
            (IElementRepository elementRepo) => _elementRepo = elementRepo;

        public List<IApartmentElement> GetAll(List<(string name, string category)> props)
        {
            return props
                .Select(p => new ApartmentElement { Name = p.name, Category = p.category })
                .Cast<IApartmentElement>()
                .ToList();
        }

        public void AddToApartment
            (Action<List<(string name, string category)>> addElementsToApartment) =>
            _elementRepo.AddToApartment(addElementsToApartment);

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

        public void InsertToModel(InsertElementDTO apartmentElementDto) =>
            _elementRepo.InsertToModel(apartmentElementDto);

        public IApartmentElement CloneFrom(IApartmentElement element)
        {
            IApartmentElement newApartmentElement = element.Clone();
            if (newApartmentElement.Annotation == null)
                newApartmentElement.Annotation = GetAnnotationFor(element.Name);
            return newApartmentElement;
        }

        public void SetAnnotationTo(IApartmentElement element, BitmapSource annotation)
        {
            var annotationService = new AnnotationService(
                new FileAnnotationCommunicatorFactory(element.Name));

            element.Annotation = annotationService.Save(annotation);
        }

        public ImageSource GetAnnotationFor(string elementName)
        {
            var annotationService = new AnnotationService(
            new FileAnnotationCommunicatorFactory(elementName));

            return annotationService.IsAnnotationExists()
                ? annotationService.Get() : null;
        }

        public void InsertBatchToModel(InsertElementDTO batchDto) =>
            _elementRepo.InsertBatchToModel(batchDto);

        public void GetInstanceParameters()
        {

        }
    }
}

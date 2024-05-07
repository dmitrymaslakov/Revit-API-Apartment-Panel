using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Utility.AnnotationUtility;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ApartmentPanel.Core.Services
{
    public class ElementService : IElementService
    {
        private readonly IElementRepository _elementRepo;
        private string _annotationName;

        public ElementService
            (IElementRepository elementRepo) => _elementRepo = elementRepo;

        public List<IApartmentElement> GetAll(List<(string name, string category, string family)> props)
        {
            return props
                .Select(p => new ApartmentElement
                {
                    Name = p.name,
                    Category = p.category,
                    Family = p.family,
                    MountingHeight = new Height()
                })
                .Cast<IApartmentElement>()
                .ToList();
        }

        public void AddToApartment
            (Action<List<(string name, string category, string family)>> addElementsToApartment) =>
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

        public void SetAnnotationTo(IApartmentElement element, BitmapImage annotation)
        {
            var annotationService = new AnnotationService(
                new FileAnnotationCommunicatorFactory(_annotationName));
            element.Annotation = annotationService.Save(annotation);
        }

        public BitmapImage GetAnnotation()
        {
            var annotationService = new AnnotationService(
            new FileAnnotationCommunicatorFactory(_annotationName));

            return annotationService.IsAnnotationExists()
                ? annotationService.Get() : null;
        }

        public void InsertBatchToModel(InsertBatchDTO batchDto) =>
            _elementRepo.InsertBatchToModel(batchDto);

        public void SetElementParameters(SetParamsDTO setParamsDTO) => 
            _elementRepo.SetParameters(setParamsDTO);

        public ElementService SetAnnotationName(string annotationName)
        {
            _annotationName = annotationName;
            return this;
        }
    }
}

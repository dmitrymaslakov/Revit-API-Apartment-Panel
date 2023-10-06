using Autodesk.Revit.UI;
using DependencyInjectionTest.Core.Infrastructure.Interfaces;
using DependencyInjectionTest.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace DependencyInjectionTest.Infrastructure.Repositories
{
    public class InfrastructureApartmentElementRepository : 
        BaseRepository, IInfrastructureApartmentElementRepository
    {
        public InfrastructureApartmentElementRepository(ExternalEvent exEvent, RequestHandler handler) 
            : base(exEvent, handler) { }

        public void AddToApartment(Action<IApartmentElement> addElementToApartment)
        {
            _handler.Request.Make(RequestId.AddElement);
            _handler.Props = addElementToApartment;
            _exEvent.Raise();
        }

        public void InsertElement(Dictionary<string, string> apartmentElementDto)
        {
            _handler.Request.Make(RequestId.Insert);
            _handler.Props = apartmentElementDto;
            _exEvent.Raise();
        }


    }
}

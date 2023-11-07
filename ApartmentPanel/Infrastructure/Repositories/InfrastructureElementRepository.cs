using Autodesk.Revit.UI;
using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ApartmentPanel.Infrastructure.Repositories
{
    public class InfrastructureElementRepository : BaseRepository, IInfrastructureElementRepository
    {
        public InfrastructureElementRepository(ExternalEvent exEvent, RequestHandler handler)
            : base(exEvent, handler) { }

        public void AddToApartment(Action<List<(string name, string category)>> addElementsToApartment)
        {
            _handler.Request.Make(RequestId.AddElement);
            _handler.Props = addElementsToApartment;
            _exEvent.Raise();
        }

        public void InsertToModel(Dictionary<string, string> apartmentElementDto)
        {
            _handler.Request.Make(RequestId.Insert);
            _handler.Props = apartmentElementDto;
            _exEvent.Raise();
        }

        public void Analize()
        {
            _handler.Request.Make(RequestId.None);
            _exEvent.Raise();
        }
    }
}

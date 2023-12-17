using Autodesk.Revit.UI;
using ApartmentPanel.Core.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;

namespace ApartmentPanel.Infrastructure.Repositories
{
    public class InfrastructureElementRepository : BaseRepository, IElementRepository
    {
        public InfrastructureElementRepository(ExternalEvent exEvent, RequestHandler handler)
            : base(exEvent, handler) { }

        public void AddToApartment(Action<List<(string name, string category)>> addElementsToApartment)
        {
            _handler.Request.Make(RequestId.AddElement);
            _handler.Props = addElementsToApartment;
            _exEvent.Raise();
        }

        public void InsertToModel(InsertElementDTO apartmentElementDto)
        {
            _handler.Request.Make(RequestId.Insert);
            _handler.Props = apartmentElementDto;
            _exEvent.Raise();
        }

        public void GetParameters(GettingParamsDTO gettingParamsDTO)
        {
            _handler.Request.Make(RequestId.GettingParameters);
            _handler.Props = gettingParamsDTO;
            _exEvent.Raise();
        }

        public void Analize()
        {
            _handler.Request.Make(RequestId.None);
            _exEvent.Raise();
        }
    }
}

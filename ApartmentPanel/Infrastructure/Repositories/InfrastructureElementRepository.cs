using Autodesk.Revit.UI;
using ApartmentPanel.Core.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Infrastructure.Handler.HandlerStates;
using ApartmentPanel.Infrastructure.Handler;

namespace ApartmentPanel.Infrastructure.Repositories
{
    public class InfrastructureElementRepository : BaseRepository, IElementRepository
    {
        public InfrastructureElementRepository(ExternalEvent exEvent, ExternalEventHandler handler)
            : base(exEvent, handler) { }

        public void AddToApartment(Action<List<(string name, string category, string family)>> addElementsToApartment)
        {
            _handler.Props = addElementsToApartment;
            _handler.SetState(new AddElementHandlerState());
            _exEvent.Raise();
        }

        public void InsertToModel(InsertElementDTO apartmentElementDto)
        {
            _handler.Props = apartmentElementDto;
            _handler.SetState(new InsertElementHandlerState());
            _exEvent.Raise();
        }

        public void InsertBatchToModel(InsertBatchDTO batchDto)
        {
            _handler.Props = batchDto;
            _handler.SetState(new InsertBatchHandlerState());
            _exEvent.Raise();
        }

        public void SetParameters(SetParamsDTO setParamsDTO)
        {
            _handler.Props = setParamsDTO;
            _handler.SetState(new SetParametersHandlerState());
            _exEvent.Raise();
        }

        public void Analize()
        {
            _handler.SetState(new AnalyzingHandlerState());
            _exEvent.Raise();
        }
    }
}

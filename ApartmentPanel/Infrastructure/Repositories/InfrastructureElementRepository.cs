using Autodesk.Revit.UI;
using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ApartmentPanel.Infrastructure.Repositories
{
    public class InfrastructureElementRepository : BaseRepository, IInfrastructureElementRepository
    {
        /*private readonly Action<List<(string, string)>> _settingProperties;
        private List<(string name, string category)> _elementProperties;*/

        public InfrastructureElementRepository(ExternalEvent exEvent, RequestHandler handler) 
            : base(exEvent, handler) 
        {
            //_settingProperties = SetProperties;
        }
        public void AddToApartment(Action<List<(string name, string category)>> addElementsToApartment)
        {
            _handler.Request.Make(RequestId.AddElement);
            _handler.Props = addElementsToApartment;
            _exEvent.Raise();
        }

        /*public List<(string name, string category)> GetPropertiesByCategory(List<string> categoriesDto)
        {
            _handler.Request.Make(RequestId.GetProperties);
            _handler.Props = categoriesDto;
            _exEvent.Raise();
            return null;
        }*/

        public void InsertToModel(Dictionary<string, string> apartmentElementDto)
        {
            _handler.Request.Make(RequestId.Insert);
            _handler.Props = apartmentElementDto;
            _exEvent.Raise();
        }

        /*private void SetProperties(List<(string name, string category)> props)
        {
            _elementProperties = props;
        }*/
    }
}

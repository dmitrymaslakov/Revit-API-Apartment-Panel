using ApartmentPanel.Core.Infrastructure.Interfaces;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ApartmentPanel.Infrastructure.Repositories
{
    public class ApartmentElementRepository : IApartmentElementRepository
    {
        public RequestHandler Handler { get; set; }
        public ExternalEvent ExEvent { get; set; }

        public ApartmentElementRepository(ExternalEvent exEvent, RequestHandler handler)
        {
            ExEvent = exEvent;
            Handler = handler;
        }

        public void InsertElement(Dictionary<string, string> apartmentElementDto)
        {
            Handler.Request.Make(RequestId.Insert);
            Handler.Props = apartmentElementDto;
            ExEvent.Raise();
        }
    }
}

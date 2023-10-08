using Autodesk.Revit.UI;
using ApartmentPanel.Core.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace ApartmentPanel.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly RequestHandler _handler;
        protected readonly ExternalEvent _exEvent;

        public BaseRepository(ExternalEvent exEvent, RequestHandler handler)
        {
            _exEvent = exEvent;
            _handler = handler;
        }
    }
}

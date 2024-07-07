using ApartmentPanel.Infrastructure.Handler;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ExternalEventHandler _handler;
        protected readonly ExternalEvent _exEvent;

        public BaseRepository(ExternalEvent exEvent, ExternalEventHandler handler)
        {
            _exEvent = exEvent;
            _handler = handler;
        }
    }
}

using Autodesk.Revit.UI;
using DependencyInjectionTest.Core.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace DependencyInjectionTest.Infrastructure.Repositories
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

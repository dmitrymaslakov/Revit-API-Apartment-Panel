using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DockableDialogs.Domain;

namespace DockableDialogs.ViewModel
{
    public class UIViewModel : ViewModelBase
    {
        private readonly RequestHandler _handler;
        private readonly ExternalEvent _exEvent;

        //public Action<object> Execute { get; set; }

        public UIViewModel(ExternalEvent exEvent, RequestHandler handler)
        {
            _handler = handler;
            _exEvent = exEvent;

            PlacementFixturies = new PlacementFixturiesCommand(o => {
                _handler.Request.Make(RequestId.Insert);
                _exEvent.Raise();
            });
        }

        public ICommand PlacementFixturies
        {
            get;
            set;
        }
    }
}

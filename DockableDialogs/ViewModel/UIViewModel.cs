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
using System.Collections.ObjectModel;
using DockableDialogs.Domain.Models;

namespace DockableDialogs.ViewModel
{
    public class UIViewModel : ViewModelBase
    {
        private readonly RequestHandler _handler;
        private readonly ExternalEvent _exEvent;

        public UIViewModel()
        {
            Circuits = new ObservableCollection<Circuit>
            {
                new Circuit { Number = 1 },
                new Circuit { Number = 2 },
                new Circuit { Number = 3 },
            };
        }

        public UIViewModel(ExternalEvent exEvent, RequestHandler handler)
        {
            _handler = handler;
            _exEvent = exEvent;
            Circuits = new ObservableCollection<Circuit> 
            { 
                new Circuit { Number = 1 }, 
                new Circuit { Number = 2 }, 
                new Circuit { Number = 3 },
            };
            PlacementFixturies = new PlacementFixturiesCommand(o => {
                _handler.Request.Make(RequestId.Insert);
                _exEvent.Raise();
            });
        }
        private ObservableCollection<Circuit> _circuits;

        public ObservableCollection<Circuit> Circuits
        {
            get => _circuits;
            set => Set(ref _circuits, value);
        }

        public ICommand PlacementFixturies
        {
            get;
            set;
        }
    }
}

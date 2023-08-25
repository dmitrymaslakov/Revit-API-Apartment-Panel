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
using DockableDialogs.Domain.Services.Commands;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;

namespace DockableDialogs.ViewModel
{
    public class UIViewModel : ViewModelBase
    {
        private const string TRISSA_SWITCH = "Trissa Switch";
        private const string USB = "USB";
        private const string BLOCK1 = "BLOCK1";
        private const string SINGLE_SOCKET = "Single Socket";
        private const string THROUGH_SWITCH = "Through Switch";
        private const string LAMP = "Lamp";

        private const string TELEPHONE_DEVICES = "Telephone Devices";
        private const string COMMUNICATION_DEVICES = "Communication Devices";
        private const string FIRE_ALARM_DEVICES = "Fire Alarm Devices";
        private const string LIGHTING_DEVICES = "Lighting Devices";
        private const string LIGHTING_FIXTURES = "Lighting Fixtures";
        private const string ELECTRICAL_FIXTURES = "Electrical Fixtures";

        /*private readonly RequestHandler _handler;
        private readonly ExternalEvent _exEvent;*/

        /*public UIViewModel()
        {
            Circuits = new ObservableCollection<Circuit>
            {
                new Circuit { Number = 1 },
                new Circuit { Number = 2 },
                new Circuit { Number = 3 },
            };
        }*/

        public UIViewModel(ExternalEvent exEvent, RequestHandler handler) : base(exEvent, handler)
        {
            /*_handler = handler;
            _exEvent = exEvent;*/
            Circuits = new ObservableCollection<Circuit> 
            { 
                new Circuit { Number = 1 }, 
                new Circuit { Number = 2 }, 
                new Circuit { Number = 3 },
            };
            InsertLamp = new RelayCommand(o =>
            {
                _handler.Props = new Dictionary<string, string>
                {
                    { "circuit", "2"},
                    { "elementName", LAMP},
                    { "elementCategory", LIGHTING_FIXTURES},
                    { "lampSuffix", "1"},
                    { "switchHeight", "110"},
                    { "socketHeight", "80"},
                };
                _handler.Request.Make(RequestId.Insert);
                _exEvent.Raise();
            });
            InsertSwitch = new RelayCommand(o =>
            {
                _handler.Props = new Dictionary<string, string>
                {
                    { "circuit", "2"},
                    { "elementName", THROUGH_SWITCH},
                    { "elementCategory", LIGHTING_DEVICES},
                    { "lampSuffix", "1"},
                    { "switchHeight", "110"},
                    { "socketHeight", "80"},
                };
                _handler.Request.Make(RequestId.Insert);
                _exEvent.Raise();
            });
            InsertSocket = new RelayCommand(o =>
            {
                _handler.Props = new Dictionary<string, string>
                {
                    { "circuit", "1"},
                    { "elementName", SINGLE_SOCKET},
                    { "elementCategory", ELECTRICAL_FIXTURES},
                    { "lampSuffix", "1"},
                    { "switchHeight", "110"},
                    { "socketHeight", "80"},
                };
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

        public ICommand InsertLamp { get; set; }
        public ICommand InsertSwitch { get; set; }
        public ICommand InsertSocket { get; set; }

        private void MakeRequest(RequestId request)
        {
            _handler.Request.Make(request);
            _exEvent.Raise();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfPanel.Domain;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Models.RevitMockupModels;
using WpfPanel.Domain.Services.Commands;

namespace WpfPanel.ViewModel.ComponentsVM
{
    public class EditPanelVM : ViewModelBase
    {
        private const string TRISSA_SWITCH = "Trissa Switch";
        private const string USB = "USB";
        private const string BLOCK1 = "BLOCK1";
        private const string SINGLE_SOCKET = "Single Socket";
        private const string THROUGH_SWITCH = "Through Switch";

        private readonly Action<FamilySymbol> _addElementToApartment;

        public EditPanelVM(ExternalEvent exEvent, RequestHandler handler)
            : base(exEvent, handler)
        {
            ApartmentElements = new ObservableCollection<string>
            {
                TRISSA_SWITCH, USB, BLOCK1, SINGLE_SOCKET, THROUGH_SWITCH
            };
            PanelCircuits = new ObservableCollection<string>
            {
                "1", "2", "3"
            };
            CircuitElements = new ObservableCollection<string>
            {
                TRISSA_SWITCH, USB, BLOCK1, SINGLE_SOCKET, THROUGH_SWITCH
            };
            AddApartmentElement = new AddApartmentElementCommand(o 
                => MakeRequest(RequestId.AddElement, _addElementToApartment));
            EditApartmentElement = new EditApartmentElementCommand(o => MakeRequest(RequestId.EditElement));
            RemoveApartmentElement = new RemoveApartmentElementCommand(o =>
            {
                if (o is string el) ApartmentElements.Remove(el);
            });
            AddPanelCircuit = new AddPanelCircuitCommand(o => MakeRequest(RequestId.AddCircuit));
            EditPanelCircuit = new EditPanelCircuitCommand(o => MakeRequest(RequestId.EditCircuit));
            RemovePanelCircuit = new RemovePanelCircuitCommand(o => MakeRequest(RequestId.RemoveCircuit));

            _addElementToApartment = newElement =>
            {
                if (!ApartmentElements.Contains(newElement.Name))
                    ApartmentElements.Add(newElement.Name);
            };
        }

        private ObservableCollection<string> _apartmentElements;

        public ObservableCollection<string> ApartmentElements
        {
            get => _apartmentElements;
            set => Set(ref _apartmentElements, value);
        }

        private ObservableCollection<string> _panelCircuits;

        public ObservableCollection<string> PanelCircuits
        {
            get => _panelCircuits;
            set => Set(ref _panelCircuits, value);
        }

        private ObservableCollection<string> _circuitElements;

        public ObservableCollection<string> CircuitElements
        {
            get => _circuitElements;
            set => Set(ref _circuitElements, value);
        }

        private string _selectedApartmentElement;

        public string SelectedApartmentElement
        {
            get => _selectedApartmentElement;
            set => Set(ref _selectedApartmentElement, value);
        }

        public ICommand AddApartmentElement { get; set; }
        public ICommand EditApartmentElement { get; set; }
        public ICommand RemoveApartmentElement { get; set; }
        public ICommand AddPanelCircuit { get; set; }
        public ICommand EditPanelCircuit { get; set; }
        public ICommand RemovePanelCircuit { get; set; }

        private void MakeRequest(RequestId request, object props = null)
        {
            _handler.Props = props;
            _handler.Request.Make(request);
            _exEvent.Raise();
        }

    }
}

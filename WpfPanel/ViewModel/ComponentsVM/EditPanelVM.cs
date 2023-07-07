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
                TRISSA_SWITCH, USB
            };
            PanelCircuits = new ObservableCollection<string>
            {
                "1", "2", "3"
            };
            CircuitElements = new ObservableCollection<string>
            {
                TRISSA_SWITCH, USB, BLOCK1, SINGLE_SOCKET, THROUGH_SWITCH
            };
            CircuitElementsDictionary = new Dictionary<string, ObservableCollection<string>>
            {
                {"1",  CircuitElements}
            };
            AddApartmentElement = new RelayCommand(o
                => MakeRequest(RequestId.AddElement, _addElementToApartment));

            RemoveApartmentElement = new RelayCommand(o =>
            {
                if (SelectedApartmentElement is string)
                    ApartmentElements.Remove(SelectedApartmentElement);
            });
            AddPanelCircuit = new RelayCommand(o =>
            {
                if (!PanelCircuits.Contains(NewCircuit))
                    PanelCircuits.Add(NewCircuit);
            });
            EditPanelCircuit = new RelayCommand(o => MakeRequest(RequestId.EditCircuit));
            RemovePanelCircuit = new RelayCommand(o => MakeRequest(RequestId.RemoveCircuit));

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
        
        private Dictionary<string, ObservableCollection<string>> _circuitElementsDictionary;

        public Dictionary<string, ObservableCollection<string>> CircuitElementsDictionary
        {
            get => _circuitElementsDictionary; 
            set => Set(ref _circuitElementsDictionary, value);
        }

        private string _selectedApartmentElement;

        public string SelectedApartmentElement
        {
            get => _selectedApartmentElement;
            set => Set(ref _selectedApartmentElement, value);
        }

        private string _newCircuit;

        public string NewCircuit { get => _newCircuit; set => Set(ref _newCircuit, value); }

        public ICommand AddApartmentElement { get; set; }
        public ICommand RemoveApartmentElement { get; set; }
        public ICommand AddPanelCircuit { get; set; }
        public ICommand EditPanelCircuit { get; set; }
        public ICommand RemovePanelCircuit { get; set; }
        public ICommand SelectedCircuitsCommand
        {
            get
            {
                if (selectedCircuitsCommand == null)
                {
                    selectedCircuitsCommand = new RelayCommand(SelectedCircuits);
                }

                return selectedCircuitsCommand;
            }
        }

        private void MakeRequest(RequestId request, object props = null)
        {
            _handler.Props = props;
            _handler.Request.Make(request);
            _exEvent.Raise();
        }

        private RelayCommand selectedCircuitsCommand;


        private void SelectedCircuits(object commandParameter)
        {
            //CircuitElementsDictionary;
        }
    }
}

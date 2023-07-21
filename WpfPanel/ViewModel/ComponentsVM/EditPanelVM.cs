using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfPanel.Domain;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Models.RevitMockupModels;
using WpfPanel.Domain.Services.Commands;
using WpfPanel.Utilities;

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
        private readonly Action<object, OkApplyCancel> _okApplyCancelActions;

        public EditPanelVM(ExternalEvent exEvent, RequestHandler handler, 
            Action<object, OkApplyCancel> okApplyCancelActions) : base(exEvent, handler)
        {
            ApartmentElements = new ObservableCollection<string>
            {
                TRISSA_SWITCH, USB
            };
            /*PanelCircuits = new ObservableCollection<string>
            {
                "1", "2", "3"
            };
            CircuitElements = new ObservableCollection<string>
            {
                TRISSA_SWITCH, USB, BLOCK1, SINGLE_SOCKET, THROUGH_SWITCH
            };*/
            PanelCircuits = new ObservableDictionary<string, ObservableCollection<string>>
            {
                {"1", new ObservableCollection<string>{ TRISSA_SWITCH, USB, BLOCK1 }},
                {"2", new ObservableCollection<string>{ SINGLE_SOCKET, THROUGH_SWITCH }},
            };
            
            CircuitElements = new ObservableCollection<string>();
            
            SelectedApartmentElements = new ObservableCollection<string>();
            
            SelectedPanelCircuits = new ObservableCollection<KeyValuePair<string, ObservableCollection<string>>>();
            
            SelectedCircuitElements = new ObservableCollection<string>();
            
            AddApartmentElementCommand = new RelayCommand(o
                => MakeRequest(RequestId.AddElement, _addElementToApartment));

            RemoveApartmentElementsCommand = new RelayCommand(o =>
            {
                if (SelectedApartmentElements.Count != 0)
                    foreach (var element in SelectedApartmentElements.ToArray())
                        ApartmentElements.Remove(element);
            });
            
            AddPanelCircuitCommand = new RelayCommand(o =>
            {
                if (!string.IsNullOrEmpty(NewCircuit) && !PanelCircuits.ContainsKey(NewCircuit))
                    PanelCircuits.Add(NewCircuit, new ObservableCollection<string>());
                NewCircuit = string.Empty;
            });
            
            RemovePanelCircuitsCommand = new RelayCommand(o =>
            {
                CircuitElements.Clear();
                SelectedCircuitElements.Clear();
                foreach (var circuit in SelectedPanelCircuits.ToArray())
                    PanelCircuits.Remove(circuit.Key);
                SelectedPanelCircuits.Clear();
            });
            
            AddElementToCircuitCommand = new RelayCommand(o =>
            {
                if (SelectedPanelCircuits.Count > 1 
                    || SelectedPanelCircuits.Count == 0 
                    || SelectedApartmentElements.Count == 0)
                    return;

                var selectedPanelCircuit = SelectedPanelCircuits.SingleOrDefault();

                foreach (var selectedApartmentElement in SelectedApartmentElements)
                {
                    if (string.IsNullOrEmpty(selectedApartmentElement)
                        || string.IsNullOrEmpty(selectedPanelCircuit.Key))
                        return;

                    var IsElementExist = PanelCircuits
                    .Where(e => e.Key == selectedPanelCircuit.Key)
                    .First()
                    .Value.Contains(selectedApartmentElement);

                    if (!IsElementExist)
                        PanelCircuits[selectedPanelCircuit.Key].Add(selectedApartmentElement);

                    AddCurrentCircuitElements(PanelCircuits[selectedPanelCircuit.Key]);
                }
            });
            
            RemoveElementsFromCircuitCommand = new RelayCommand(o =>
            {
                if (SelectedPanelCircuits.Count > 1 
                    || SelectedPanelCircuits.Count == 0
                    || SelectedCircuitElements.Count == 0) 
                    return;

                var selectedPanelCircuit = SelectedPanelCircuits.SingleOrDefault();

                if (string.IsNullOrEmpty(selectedPanelCircuit.Key))
                    return;

                foreach (var selectedCircuitElement in SelectedCircuitElements)
                    selectedPanelCircuit.Value.Remove(selectedCircuitElement);

                    AddCurrentCircuitElements(selectedPanelCircuit.Value);
            });

            SelectedApartmentElementsCommand = new RelayCommand(o =>
            {
                var apartmentElements = (o as IList<object>)?.OfType<string>();
                if (SelectedApartmentElements.Count != 0)
                    SelectedApartmentElements.Clear();

                if (apartmentElements.Count() != 0)
                    foreach (var apartmentElement in apartmentElements)
                        SelectedApartmentElements.Add(apartmentElement);
            });

            SelectedPanelCircuitCommand = new RelayCommand(o =>
            {
                SelectedCircuitElements.Clear();
                var currentCircuitElements = (o as IList<object>)
                ?.OfType<KeyValuePair<string, ObservableCollection<string>>>()
                /*?.FirstOrDefault()
                .Value*/
                ;

                if (currentCircuitElements.Count() != 0)
                {
                    SelectedPanelCircuits.Clear();
                    foreach (var currentCircuitElement in currentCircuitElements)
                    {
                        SelectedPanelCircuits.Add(currentCircuitElement);
                        if (currentCircuitElements.Count() == 1)
                            AddCurrentCircuitElements(currentCircuitElement.Value);
                        else
                            CircuitElements.Clear();
                    }
                }
            });

            SelectedCircuitElementCommand = new RelayCommand(o =>
            {
                var circuitElements = (o as IList<object>)?.OfType<string>();
                if (SelectedCircuitElements.Count != 0)
                    SelectedCircuitElements.Clear();

                if (circuitElements.Count() != 0)
                    foreach (var circuitElement in circuitElements)
                        SelectedCircuitElements.Add(circuitElement);
            });

            OkCommand = new RelayCommand(o =>
            {
                var close = (Action)o;
                _okApplyCancelActions(PanelCircuits, OkApplyCancel.Ok);
                close();
            });

            ApplyCommand = new RelayCommand(o => 
                _okApplyCancelActions(PanelCircuits, OkApplyCancel.Apply));

            _addElementToApartment = newElement =>
            {
                if (!ApartmentElements.Contains(newElement.Name))
                    ApartmentElements.Add(newElement.Name);
            };

            _okApplyCancelActions = okApplyCancelActions;
        }

        private void AddCurrentCircuitElements(ObservableCollection<string> currentCircuitElements)
        {
            if (CircuitElements.Count != 0)
                CircuitElements.Clear();

            foreach (var item in currentCircuitElements)
            {
                CircuitElements.Add(item);
            }
        }

        public List<object> OldState { get; set; }

        private ObservableCollection<string> _apartmentElements;

        public ObservableCollection<string> ApartmentElements
        {
            get => _apartmentElements;
            set => Set(ref _apartmentElements, value);
        }

        /*private ObservableCollection<string> _panelCircuits;

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
        }*/

        private ObservableDictionary<string, ObservableCollection<string>> _panelCircuits;

        public ObservableDictionary<string, ObservableCollection<string>> PanelCircuits
        {
            get => _panelCircuits;
            set => Set(ref _panelCircuits, value);
        }

        private ObservableCollection<string> _selectedApartmentElements;

        public ObservableCollection<string> SelectedApartmentElements
        {
            get => _selectedApartmentElements;
            set => Set(ref _selectedApartmentElements, value);
        }

        private ObservableCollection
            <KeyValuePair<string, ObservableCollection<string>>> _selectedPanelCircuits;

        public ObservableCollection
            <KeyValuePair<string, ObservableCollection<string>>> SelectedPanelCircuits
        {
            get => _selectedPanelCircuits;
            set => Set(ref _selectedPanelCircuits, value);
        }

        private ObservableCollection<string> _selectedCircuitElements;

        public ObservableCollection<string> SelectedCircuitElements
        {
            get => _selectedCircuitElements;
            set => Set(ref _selectedCircuitElements, value);
        }

        private ObservableCollection<string> _circuitElements;

        public ObservableCollection<string> CircuitElements
        {
            get => _circuitElements;
            set => Set(ref _circuitElements, value);
        }

        private string _newCircuit;

        public string NewCircuit { get => _newCircuit; set => Set(ref _newCircuit, value); }

        private string _testProp;

        public string TestProp { get => _testProp; set => Set(ref _testProp, value); }


        public ICommand AddApartmentElementCommand { get; set; }
        public ICommand RemoveApartmentElementsCommand { get; set; }
        public ICommand AddPanelCircuitCommand { get; set; }
        public ICommand EditPanelCircuit { get; set; }
        public ICommand RemovePanelCircuitsCommand { get; set; }
        public ICommand AddElementToCircuitCommand { get; set; }
        public ICommand RemoveElementsFromCircuitCommand { get; set; }
        public ICommand SelectedApartmentElementsCommand { get; set; }
        public ICommand SelectedPanelCircuitCommand { get; set; }
        public ICommand SelectedCircuitElementCommand { get; set; }
        public ICommand OkCommand { get; set; }
        public ICommand ApplyCommand { get; set; }

        private void MakeRequest(RequestId request, object props = null)
        {
            _handler.Props = props;
            _handler.Request.Make(request);
            _exEvent.Raise();
        }

        private RelayCommand selectedCircuitsCommand;


        /*private void SelectedCircuits(object commandParameter)
        {
            CircuitElementsDictionary;
        }*/
    }
}

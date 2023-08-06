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
        #region Mock Data
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
        #endregion

        private readonly Action<FamilySymbol> _addElementToApartment;
        private readonly Action<object, OkApplyCancel> _okApplyCancelActions;

        public EditPanelVM(ExternalEvent exEvent, RequestHandler handler, 
            Action<object, OkApplyCancel> okApplyCancelActions) : base(exEvent, handler)
        {
            ApartmentElements = new ObservableCollection<ApartmentElement>
            {
                new ApartmentElement {Name = TRISSA_SWITCH, Category = ELECTRICAL_FIXTURES},
                new ApartmentElement {Name = USB, Category = COMMUNICATION_DEVICES}   
            };
            /*PanelCircuits = new ObservableCollection<string>
            {
                "1", "2", "3"
            };
            CircuitElements = new ObservableCollection<string>
            {
                TRISSA_SWITCH, USB, BLOCK1, SINGLE_SOCKET, THROUGH_SWITCH
            };*/
            PanelCircuits = new ObservableDictionary<string, ObservableCollection<ApartmentElement>>
            {
                {"1", new ObservableCollection<ApartmentElement>
                    { 
                        new ApartmentElement {Name = TRISSA_SWITCH, Category = ELECTRICAL_FIXTURES},
                        new ApartmentElement {Name = USB, Category = COMMUNICATION_DEVICES},
                        new ApartmentElement {Name = BLOCK1, Category = COMMUNICATION_DEVICES},
                    }
                },
                {"2", new ObservableCollection<ApartmentElement>
                    {
                        new ApartmentElement {Name = SINGLE_SOCKET, Category = ELECTRICAL_FIXTURES},
                        new ApartmentElement {Name = THROUGH_SWITCH, Category = LIGHTING_DEVICES},
                        new ApartmentElement {Name = LAMP, Category = LIGHTING_FIXTURES},                     
                    }
                },
            };
            
            CircuitElements = new ObservableCollection<ApartmentElement>();
            
            SelectedApartmentElements = new ObservableCollection<ApartmentElement>();
            
            SelectedPanelCircuits = 
                new ObservableCollection<KeyValuePair<string, ObservableCollection<ApartmentElement>>>();
            
            SelectedCircuitElements = new ObservableCollection<ApartmentElement>();
            
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
                    PanelCircuits.Add(NewCircuit, new ObservableCollection<ApartmentElement>());
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
                    if (selectedApartmentElement == null 
                    || string.IsNullOrEmpty(selectedPanelCircuit.Key))
                        return;

                    var IsElementExist = PanelCircuits
                    .Where(e => e.Key == selectedPanelCircuit.Key)
                    .First()
                    .Value
                    .Select(ae => ae.Name)
                    .Contains(selectedApartmentElement.Name);

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
                var apartmentElements = (o as IList<object>)?.OfType<ApartmentElement>();
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
                ?.OfType<KeyValuePair<string, ObservableCollection<ApartmentElement>>>()
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
                var circuitElements = (o as IList<object>)?.OfType<ApartmentElement>();
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
                if (!ApartmentElements.Select(ae => ae.Name).Contains(newElement.Name))
                    ApartmentElements.Add(new ApartmentElement { Name= newElement.Name , Category= newElement.Category.Name});
            };

            _okApplyCancelActions = okApplyCancelActions;
        }

        private void AddCurrentCircuitElements(ObservableCollection<ApartmentElement> currentCircuitElements)
        {
            if (CircuitElements.Count != 0)
                CircuitElements.Clear();

            foreach (var item in currentCircuitElements)
                CircuitElements.Add(item);
        }

        public List<object> OldState { get; set; }

        private ObservableCollection<ApartmentElement> _apartmentElements;

        public ObservableCollection<ApartmentElement> ApartmentElements
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

        private ObservableDictionary<string, ObservableCollection<ApartmentElement>> _panelCircuits;

        public ObservableDictionary<string, ObservableCollection<ApartmentElement>> PanelCircuits
        {
            get => _panelCircuits;
            set => Set(ref _panelCircuits, value);
        }

        private ObservableCollection<ApartmentElement> _selectedApartmentElements;

        public ObservableCollection<ApartmentElement> SelectedApartmentElements
        {
            get => _selectedApartmentElements;
            set => Set(ref _selectedApartmentElements, value);
        }

        private ObservableCollection
            <KeyValuePair<string, ObservableCollection<ApartmentElement>>> _selectedPanelCircuits;

        public ObservableCollection
            <KeyValuePair<string, ObservableCollection<ApartmentElement>>> SelectedPanelCircuits
        {
            get => _selectedPanelCircuits;
            set => Set(ref _selectedPanelCircuits, value);
        }

        private ObservableCollection<ApartmentElement> _selectedCircuitElements;

        public ObservableCollection<ApartmentElement> SelectedCircuitElements
        {
            get => _selectedCircuitElements;
            set => Set(ref _selectedCircuitElements, value);
        }

        private ObservableCollection<ApartmentElement> _circuitElements;

        public ObservableCollection<ApartmentElement> CircuitElements
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
    }
}

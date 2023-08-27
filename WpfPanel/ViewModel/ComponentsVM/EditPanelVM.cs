using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfPanel.Domain;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Models.RevitMockupModels;
using WpfPanel.Domain.Services.AnnotationService;
using WpfPanel.Domain.Services.Commands;
using WpfPanel.Utilities;

namespace WpfPanel.ViewModel.ComponentsVM
{
    public class EditPanelVM : ViewModelBase
    {
        private readonly Action<FamilySymbol> _addElementToApartment;

        public EditPanelVM(ExternalEvent exEvent, RequestHandler handler,
            Action<object, OkApplyCancel> okApplyCancelActions) : base(exEvent, handler)
        {
            ApartmentElements = new ObservableCollection<ApartmentElement>
            {
                new ApartmentElement {Name = StaticData.TRISSA_SWITCH, Category = StaticData.ELECTRICAL_FIXTURES},
                new ApartmentElement {Name = StaticData.USB, Category = StaticData.COMMUNICATION_DEVICES}
            };

            PanelCircuits = new ObservableDictionary<string, ObservableCollection<ApartmentElement>>
            {
                {"1", new ObservableCollection<ApartmentElement>
                    {
                        new ApartmentElement {Name = StaticData.TRISSA_SWITCH, Category = StaticData.ELECTRICAL_FIXTURES},
                        new ApartmentElement {Name = StaticData.USB, Category = StaticData.COMMUNICATION_DEVICES},
                        new ApartmentElement {Name = StaticData.BLOCK1, Category = StaticData.COMMUNICATION_DEVICES},
                    }
                },
                {"2", new ObservableCollection<ApartmentElement>
                    {
                        new ApartmentElement {Name = StaticData.SINGLE_SOCKET, Category = StaticData.ELECTRICAL_FIXTURES},
                        new ApartmentElement {Name = StaticData.THROUGH_SWITCH, Category = StaticData.LIGHTING_DEVICES},
                        new ApartmentElement {Name = StaticData.LAMP, Category = StaticData.LIGHTING_FIXTURES},
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
                this.OkApplyCancelActions(PanelCircuits, OkApplyCancel.Ok);
                close();
            });

            ApplyCommand = new RelayCommand(o =>
                this.OkApplyCancelActions(PanelCircuits, OkApplyCancel.Apply));

            SetAnnotationToElementCommand = new RelayCommand(o =>
            {
                if (SelectedApartmentElements.Count == 1)
                {
                    ApartmentElement apartmentElement = SelectedApartmentElements.FirstOrDefault();
                    var annotationService = new AnnotationService(
                        new FileAnnotationCommunicatorFactory(apartmentElement.Name));

                    apartmentElement.Annotation = annotationService.Save(AnnotationPreview);
                }
            });

            SetAnnotationPreviewCommand = new RelayCommand(o =>
            {
                var bitmapSource = o as BitmapSource;
                AnnotationPreview = bitmapSource;
            });

            _addElementToApartment = newElement =>
            {
                if (!ApartmentElements.Select(ae => ae.Name).Contains(newElement.Name))
                {
                    var annotationService = new AnnotationService(
                        new FileAnnotationCommunicatorFactory(newElement.Name));

                    ImageSource annotation = annotationService.IsAnnotationExists()
                        ? annotationService.Get() : null;

                    ApartmentElements.Add(
                        new ApartmentElement
                        {
                            Name = newElement.Name,
                            Category = newElement.Category.Name,
                            Annotation = annotation
                        });
                }
            };

            OkApplyCancelActions = okApplyCancelActions;
        }

        private void AddCurrentCircuitElements(ObservableCollection<ApartmentElement> currentCircuitElements)
        {
            if (CircuitElements.Count != 0)
                CircuitElements.Clear();

            foreach (var item in currentCircuitElements)
                CircuitElements.Add(item);
        }

        private ObservableCollection<ApartmentElement> _apartmentElements;

        public ObservableCollection<ApartmentElement> ApartmentElements
        {
            get => _apartmentElements;
            set => Set(ref _apartmentElements, value);
        }

        private ObservableDictionary<string, ObservableCollection<ApartmentElement>> _panelCircuits;

        public ObservableDictionary<string, ObservableCollection<ApartmentElement>> PanelCircuits
        {
            get => _panelCircuits;
            set => Set(ref _panelCircuits, value);
        }

        private ObservableCollection<ApartmentElement> _selectedApartmentElements;

        [JsonIgnore]
        public ObservableCollection<ApartmentElement> SelectedApartmentElements
        {
            get => _selectedApartmentElements;
            set => Set(ref _selectedApartmentElements, value);
        }

        private ObservableCollection
            <KeyValuePair<string, ObservableCollection<ApartmentElement>>> _selectedPanelCircuits;

        [JsonIgnore]
        public ObservableCollection
            <KeyValuePair<string, ObservableCollection<ApartmentElement>>> SelectedPanelCircuits
        {
            get => _selectedPanelCircuits;
            set => Set(ref _selectedPanelCircuits, value);
        }

        private ObservableCollection<ApartmentElement> _selectedCircuitElements;
        [JsonIgnore]
        public ObservableCollection<ApartmentElement> SelectedCircuitElements
        {
            get => _selectedCircuitElements;
            set => Set(ref _selectedCircuitElements, value);
        }

        private ObservableCollection<ApartmentElement> _circuitElements;

        [JsonIgnore]
        public ObservableCollection<ApartmentElement> CircuitElements
        {
            get => _circuitElements;
            set => Set(ref _circuitElements, value);
        }

        private BitmapSource _annotationPreview;

        [JsonIgnore]
        public BitmapSource AnnotationPreview { get => _annotationPreview; set => Set(ref _annotationPreview, value); }

        private string _newCircuit;
        [JsonIgnore]
        public string NewCircuit { get => _newCircuit; set => Set(ref _newCircuit, value); }
        [JsonIgnore]
        public ICommand AddApartmentElementCommand { get; set; }
        [JsonIgnore]
        public ICommand RemoveApartmentElementsCommand { get; set; }
        [JsonIgnore]
        public ICommand AddPanelCircuitCommand { get; set; }
        [JsonIgnore]
        public ICommand RemovePanelCircuitsCommand { get; set; }
        [JsonIgnore]
        public ICommand AddElementToCircuitCommand { get; set; }
        [JsonIgnore]
        public ICommand RemoveElementsFromCircuitCommand { get; set; }
        [JsonIgnore]
        public ICommand SelectedApartmentElementsCommand { get; set; }
        [JsonIgnore]
        public ICommand SelectedPanelCircuitCommand { get; set; }
        [JsonIgnore]
        public ICommand SelectedCircuitElementCommand { get; set; }
        [JsonIgnore]
        public ICommand OkCommand { get; set; }
        [JsonIgnore]
        public ICommand ApplyCommand { get; set; }
        [JsonIgnore]
        public ICommand SetAnnotationToElementCommand { get; set; }
        [JsonIgnore]
        public ICommand SetAnnotationPreviewCommand { get; set; }

        [JsonIgnore]
        public Action<object, OkApplyCancel> OkApplyCancelActions { get; }

        public EditPanelVM ApplyLatestConfiguration(EditPanelVM latestConfiguration)
        {
            ApartmentElements = latestConfiguration.ApartmentElements;
            PanelCircuits = latestConfiguration.PanelCircuits;

            foreach (var apartmentElement in ApartmentElements)
            {
                var annService = new AnnotationService(
                    new FileAnnotationCommunicatorFactory(apartmentElement.Name));

                apartmentElement.Annotation = annService.IsAnnotationExists()
                    ? annService.Get() : null;
            };

            for (int i = 0; i < PanelCircuits.Count; i++)
            {
                var newCircuitElements = new ObservableCollection<ApartmentElement>();
                var circuitElements = PanelCircuits[i].Value;

                foreach (var apartmentElement in ApartmentElements)
                {
                    var matchingCircuitElement = circuitElements
                        .FirstOrDefault(c => c.Name == apartmentElement.Name);

                    if (matchingCircuitElement != null)
                    {
                        int index = circuitElements.IndexOf(matchingCircuitElement);
                        circuitElements[index] = apartmentElement;
                    }
                }
                PanelCircuits[i] =
                    new KeyValuePair<string, ObservableCollection<ApartmentElement>>(
                        PanelCircuits[i].Key, circuitElements);
            }
            return this;
        }

        private void MakeRequest(RequestId request, object props = null)
        {
            Handler.Props = props;
            Handler.Request.Make(request);
            ExEvent.Raise();
        }

    }
}

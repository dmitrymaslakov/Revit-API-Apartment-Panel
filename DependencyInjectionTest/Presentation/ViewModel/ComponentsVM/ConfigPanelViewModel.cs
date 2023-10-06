using DependencyInjectionTest.Core.Services.AnnotationService;
using DependencyInjectionTest.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DependencyInjectionTest.Presentation.Commands;
using DependencyInjectionTest.Presentation.ViewModel.Interfaces;
using DependencyInjectionTest.Core.Services.Interfaces;
using DependencyInjectionTest.Core.Models.Interfaces;

namespace DependencyInjectionTest.Presentation.ViewModel.ComponentsVM
{
    public class ConfigPanelViewModel : ViewModelBase, IConfigPanelViewModel//, IConfigPanelPropsForCommandsCreater
    {
        private readonly IApartmentElementService _apartmentElementService;
        private readonly IApartmentPanelService _apartmentPanelService;
        private readonly ConfigPanelCommandsCreater _commandCreater;

        //public ConfigPanelVM(ExternalEvent exEvent, RequestHandler handler) : base(exEvent, handler)
        public ConfigPanelViewModel(IApartmentElementService apartmentElementService, 
            IApartmentPanelService apartmentPanelService)
        {
            _apartmentElementService = apartmentElementService;
            _apartmentPanelService = apartmentPanelService;
            _commandCreater = new ConfigPanelCommandsCreater(this, _apartmentElementService, _apartmentPanelService);
            ApartmentElements = new ObservableCollection<IApartmentElement>();
            PanelCircuits = new ObservableDictionary<string, ObservableCollection<IApartmentElement>>();
            CircuitElements = new ObservableCollection<IApartmentElement>();
            SelectedApartmentElements = new ObservableCollection<IApartmentElement>();
            SelectedPanelCircuits =
                new ObservableCollection<KeyValuePair<string, ObservableCollection<IApartmentElement>>>();
            SelectedCircuitElements = new ObservableCollection<IApartmentElement>();
            LatestConfigPath = FileUtility.GetAssemblyPath() + "\\LatestConfig.json";
            IsCancelEnabled = false;
            AddApartmentElementCommand = _commandCreater.CreateAddApartmentElementCommand();
            RemoveApartmentElementsCommand = _commandCreater.CreateRemoveApartmentElementsCommand();
            AddPanelCircuitCommand = _commandCreater.CreateAddPanelCircuitCommand();
            RemovePanelCircuitsCommand = _commandCreater.CreateRemovePanelCircuitsCommand();
            AddElementToCircuitCommand = _commandCreater.CreateAddElementToCircuitCommand();
            RemoveElementsFromCircuitCommand = _commandCreater.CreateRemoveElementsFromCircuitCommand();
            SelectedApartmentElementsCommand = _commandCreater.CreateSelectedApartmentElementsCommand();
            SelectedPanelCircuitCommand = _commandCreater.CreateSelectedPanelCircuitCommand();
            SelectedCircuitElementCommand = _commandCreater.CreateSelectedCircuitElementCommand();
            OkCommand = _commandCreater.CreateOkCommand();
            ApplyCommand = _commandCreater.CreateApplyCommand();
            CancelCommand = _commandCreater.CreateCancelCommand();
            SetAnnotationToElementCommand = _commandCreater.CreateSetAnnotationToElementCommand();
            SetAnnotationPreviewCommand = _commandCreater.CreateSetAnnotationPreviewCommand();
            LoadLatestConfigCommand = _commandCreater.CreateLoadLatestConfigCommand();
            SaveLatestConfigCommand = _commandCreater.CreateSaveLatestConfigCommand();
        }

        public string LatestConfigPath { get; }

        private ObservableCollection<IApartmentElement> _apartmentElements;

        public ObservableCollection<IApartmentElement> ApartmentElements
        {
            get => _apartmentElements;
            set => Set(ref _apartmentElements, value);
        }

        private ObservableDictionary<string, ObservableCollection<IApartmentElement>> _panelCircuits;

        public ObservableDictionary<string, ObservableCollection<IApartmentElement>> PanelCircuits
        {
            get => _panelCircuits;
            set => Set(ref _panelCircuits, value);
        }

        private ObservableCollection<IApartmentElement> _selectedApartmentElements;

        [JsonIgnore]
        public ObservableCollection<IApartmentElement> SelectedApartmentElements
        {
            get => _selectedApartmentElements;
            set => Set(ref _selectedApartmentElements, value);
        }

        private ObservableCollection
            <KeyValuePair<string, ObservableCollection<IApartmentElement>>> _selectedPanelCircuits;

        [JsonIgnore]
        public ObservableCollection
            <KeyValuePair<string, ObservableCollection<IApartmentElement>>> SelectedPanelCircuits
        {
            get => _selectedPanelCircuits;
            set => Set(ref _selectedPanelCircuits, value);
        }

        private ObservableCollection<IApartmentElement> _selectedCircuitElements;
        [JsonIgnore]
        public ObservableCollection<IApartmentElement> SelectedCircuitElements
        {
            get => _selectedCircuitElements;
            set => Set(ref _selectedCircuitElements, value);
        }

        private ObservableCollection<IApartmentElement> _circuitElements;

        [JsonIgnore]
        public ObservableCollection<IApartmentElement> CircuitElements
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

        private bool _isCancelEnabled;
        [JsonIgnore]
        public bool IsCancelEnabled { get => _isCancelEnabled; set => Set(ref _isCancelEnabled, value); }

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
        public ICommand CancelCommand { get; set; }
        [JsonIgnore]
        public ICommand SetAnnotationToElementCommand { get; set; }
        [JsonIgnore]
        public ICommand SetAnnotationPreviewCommand { get; set; }
        [JsonIgnore]
        public ICommand SaveLatestConfigCommand { get; set; }
        [JsonIgnore]
        public ICommand LoadLatestConfigCommand { get; set; }
        [JsonIgnore]
        public Action<object, OkApplyCancel> OkApplyCancelActions { get; set; }

        public ConfigPanelViewModel ApplyLatestConfiguration(ConfigPanelViewModel latestConfiguration)
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
                var newCircuitElements = new ObservableCollection<IApartmentElement>();
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
                    new KeyValuePair<string, ObservableCollection<IApartmentElement>>(
                        PanelCircuits[i].Key, circuitElements);
            }
            return this;
        }
    }
}

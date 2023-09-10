using Autodesk.Revit.UI;
using ApartmentPanel.Domain;
using ApartmentPanel.Domain.Models;
using ApartmentPanel.Domain.Services.AnnotationService;
using ApartmentPanel.Domain.Services.Commands;
using ApartmentPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.ViewModel.ComponentsVM
{
    public class EditPanelVM : ViewModelBase, IEditPanelToCommandsCreater
    {
        private readonly EditPanelCommandsCreater _commandCreater;

        public EditPanelVM(ExternalEvent exEvent, RequestHandler handler) : base(exEvent, handler)
        {
            _commandCreater = new EditPanelCommandsCreater(this);

            ApartmentElements = new ObservableCollection<ApartmentElement>();

            PanelCircuits = new ObservableDictionary<string, ObservableCollection<ApartmentElement>>();

            CircuitElements = new ObservableCollection<ApartmentElement>();

            SelectedApartmentElements = new ObservableCollection<ApartmentElement>();

            SelectedPanelCircuits =
                new ObservableCollection<KeyValuePair<string, ObservableCollection<ApartmentElement>>>();

            SelectedCircuitElements = new ObservableCollection<ApartmentElement>();

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
    }
}

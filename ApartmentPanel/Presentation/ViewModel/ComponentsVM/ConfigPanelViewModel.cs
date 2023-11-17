﻿using ApartmentPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ApartmentPanel.Presentation.Commands;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Models;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM
{
    public class ConfigPanelViewModel : ViewModelBase, IConfigPanelViewModel
    {
        #region MockFields
        private static ObservableCollection<IApartmentElement> el1 = new ObservableCollection<IApartmentElement>
        {
            new ApartmentElement { Name = "Switch"},
            new ApartmentElement { Name = "Socket"},
            new ApartmentElement { Name = "ThroughSwitch"}
        };
        private static ObservableCollection<IApartmentElement> el2 = new ObservableCollection<IApartmentElement>
        {
            new ApartmentElement { Name = "Smoke Sensor"},
            new ApartmentElement { Name = "USB"},
            new ApartmentElement { Name = "ThroughSwitch"}
        };
        #endregion

        private readonly ConfigPanelCommandsCreater _commandCreater;

        public ConfigPanelViewModel() { }

        public ConfigPanelViewModel(IElementService elementService,
            IListElementsViewModel listElementsVM) : base(elementService)
        {
            _commandCreater = new ConfigPanelCommandsCreater(this, ElementService);
            ListElementsVM = listElementsVM;
            ApartmentElements = new ObservableCollection<IApartmentElement>();
            PanelCircuits = new ObservableDictionary<string, ObservableCollection<IApartmentElement>>();
            CircuitElements = new ObservableCollection<IApartmentElement>();
            SelectedApartmentElements = new ObservableCollection<IApartmentElement>();
            SelectedPanelCircuits =
                new ObservableCollection<KeyValuePair<string, ObservableCollection<IApartmentElement>>>();
            SelectedCircuitElements = new ObservableCollection<IApartmentElement>();
            ListHeightsOK = new ObservableCollection<double>();
            ListHeightsUK = new ObservableCollection<double>();
            ListHeightsCenter = new ObservableCollection<double>();
            LatestConfigPath = FileUtility.GetAssemblyPath() + "\\LatestConfig.json";
            IsCancelEnabled = false;
            ShowListElementsCommand = _commandCreater.CreateShowListElementsCommand();
            RemoveApartmentElementsCommand = _commandCreater.CreateRemoveElementsFromApartmentCommand();
            AddPanelCircuitCommand = _commandCreater.CreateAddCircuitToPanelCommand();
            RemovePanelCircuitsCommand = _commandCreater.CreateRemoveCircuitsFromPanelCommand();
            AddElementToCircuitCommand = _commandCreater.CreateAddElementsToCircuitCommand();
            RemoveElementsFromCircuitCommand = _commandCreater.CreateRemoveElementsFromCircuitCommand();
            SelectedApartmentElementsCommand = _commandCreater.CreateCollectSelectedApartmentElementsCommand();
            SelectPanelCircuitCommand = _commandCreater.CreateCollectSelectedCircuitsCommand();
            SelectedCircuitElementCommand = _commandCreater.CreateCollectSelectedCircuitElementsCommand();
            OkCommand = _commandCreater.CreateOkCommand();
            ApplyCommand = _commandCreater.CreateApplyCommand();
            CancelCommand = _commandCreater.CreateCancelCommand();
            SetAnnotationToElementCommand = _commandCreater.CreateSetAnnotationToElementCommand();
            SetAnnotationPreviewCommand = _commandCreater.CreateSetAnnotationPreviewCommand();
            LoadLatestConfigCommand = _commandCreater.CreateLoadLatestConfigCommand();
            SaveLatestConfigCommand = _commandCreater.CreateSaveLatestConfigCommand();
        }

        #region MockProps
        public ObservableDictionary<string, ObservableCollection<IApartmentElement>> MockPanelCircuits { get; set; } =
            new ObservableDictionary<string, ObservableCollection<IApartmentElement>>
            {
                new KeyValuePair<string, ObservableCollection<IApartmentElement>>("1", el1),
                new KeyValuePair<string, ObservableCollection<IApartmentElement>>("2", el2)
            };

        #endregion
        [JsonIgnore]
        public IListElementsViewModel ListElementsVM { get; set; }

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

        #region listHeights
        private ObservableCollection<double> _listHeightsOK;

        public ObservableCollection<double> ListHeightsOK
        {
            get => _listHeightsOK;
            set => Set(ref _listHeightsOK, value);
        }

        private ObservableCollection<double> _listHeightsUK;

        public ObservableCollection<double> ListHeightsUK
        {
            get => _listHeightsUK;
            set => Set(ref _listHeightsUK, value);
        }

        private ObservableCollection<double> _listHeightsCenter;

        public ObservableCollection<double> ListHeightsCenter
        {
            get => _listHeightsCenter;
            set => Set(ref _listHeightsCenter, value);
        }
        #endregion

        [JsonIgnore]
        public ICommand ShowListElementsCommand { get; set; }
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
        public ICommand SelectPanelCircuitCommand { get; set; }
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
            if (latestConfiguration.ListHeightsOK != null)
                ListHeightsOK = latestConfiguration.ListHeightsOK;
            if (latestConfiguration.ListHeightsUK != null)
                ListHeightsUK = latestConfiguration.ListHeightsUK;
            if (latestConfiguration.ListHeightsCenter != null)
                ListHeightsCenter = latestConfiguration.ListHeightsCenter;

            foreach (var apartmentElement in ApartmentElements)
            {
                /*var annService = new AnnotationService(
                    new FileAnnotationCommunicatorFactory(apartmentElement.Name));

                apartmentElement.Annotation = annService.IsAnnotationExists()
                    ? annService.Get() : null;*/
                apartmentElement.Annotation = ElementService.GetAnnotationFor(apartmentElement.Name);
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

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
using System.Text.RegularExpressions;
using System.IO;
using ApartmentPanel.Core.Services;
using ApartmentPanel.Core.Models.Batch;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM
{
    public class ConfigPanelViewModel : ViewModelBase, IConfigPanelViewModel
    {
        private readonly ConfigPanelCommandsCreater _commandCreater;

        public ConfigPanelViewModel() { }

        public ConfigPanelViewModel(IElementService elementService,
            IListElementsViewModel listElementsVM) : base(elementService)
        {
            ElementBatch = new ElementBatch
            {
                BatchedRows = new ObservableCollection<BatchedRow>
                {
                    new BatchedRow()
                }
            };
            Batches = new ObservableCollection<ElementBatch>();
            SelectedBatchedElement = new BatchedElement();
            SelectedBatches = new ObservableCollection<ElementBatch>();
            _commandCreater = new ConfigPanelCommandsCreater(this, ElementService);
            ListElementsVM = listElementsVM;
            ApartmentElements = new ObservableCollection<IApartmentElement>();
            PanelCircuits = new ObservableCollection<Circuit>();
            CircuitElements = new ObservableCollection<IApartmentElement>();
            SelectedApartmentElements = new ObservableCollection<IApartmentElement>();
            SelectedPanelCircuits = new ObservableCollection<Circuit>();

            SelectedCircuitElements = new ObservableCollection<IApartmentElement>();
            ListHeightsOK = new ObservableCollection<double>();
            ListHeightsUK = new ObservableCollection<double>();
            ListHeightsCenter = new ObservableCollection<double>();
            if(TryFindConfigs(out ObservableCollection<string> configs))
                Configs = configs;
            else Configs = new ObservableCollection<string>();

            IsCancelEnabled = false;
            ShowListElementsCommand = _commandCreater.CreateShowListElementsCommand();
            RemoveApartmentElementsCommand = _commandCreater.CreateRemoveElementsFromApartmentCommand();
            AddPanelCircuitCommand = _commandCreater.CreateAddCircuitToPanelCommand();
            RemovePanelCircuitsCommand = _commandCreater.CreateRemoveCircuitsFromPanelCommand();
            AddElementToCircuitCommand = _commandCreater.CreateAddElementToCircuitCommand();
            RemoveElementsFromCircuitCommand = _commandCreater.CreateRemoveElementsFromCircuitCommand();
            SelectApartmentElementsCommand = _commandCreater.CreateSelectApartmentElementsCommand();
            SelectPanelCircuitCommand = _commandCreater.CreateSelectPanelCircuitCommand();
            SelectCircuitElementCommand = _commandCreater.CreateSelectCircuitElementCommand();
            OkCommand = _commandCreater.CreateOkCommand();
            ApplyCommand = _commandCreater.CreateApplyCommand();
            CancelCommand = _commandCreater.CreateCancelCommand();
            SetAnnotationToElementCommand = _commandCreater.CreateSetAnnotationToElementCommand();
            SetAnnotationToElementsBatchCommand = _commandCreater.CreateSetAnnotationToElementsBatchCommand();
            SetAnnotationPreviewCommand = _commandCreater.CreateSetAnnotationPreviewCommand();
            LoadLatestConfigCommand = _commandCreater.CreateLoadLatestConfigCommand();
            SaveLatestConfigCommand = _commandCreater.CreateSaveLatestConfigCommand();
            SetNewElementForBatchCommand = _commandCreater.CreateSetNewElementForBatchCommand();
            AddElementToRowCommand = _commandCreater.CreateAddElementToRowCommand();
            RemoveElementFromRowCommand = _commandCreater.CreateRemoveElementFromRowCommand();
            AddBatchToElementBatchesCommand = _commandCreater.CreateAddBatchToElementBatchesCommand();
            SelectedBatchesCommand = _commandCreater.CreateSelectedBatchesCommand();
            RemoveBatchCommand = _commandCreater.CreateRemoveBatchCommand();
            AddRowToBatchCommand = _commandCreater.CreateAddRowToBatchCommand();
            RemoveRowFromBatchCommand = _commandCreater.CreateRemoveRowFromBatchCommand();
            AddConfigCommand = _commandCreater.CreateAddConfigCommand(); 
            RemoveConfigCommand = _commandCreater.CreateRemoveConfigCommand();
        }

        [JsonIgnore]
        public IListElementsViewModel ListElementsVM { get; set; }
        public string LatestConfigPath { get; set; }

        private ObservableCollection<IApartmentElement> _apartmentElements;
        public ObservableCollection<IApartmentElement> ApartmentElements
        {
            get => _apartmentElements;
            set => Set(ref _apartmentElements, value);
        }

        private ObservableCollection<Circuit> _panelCircuits;
        public ObservableCollection<Circuit> PanelCircuits
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

        private ObservableCollection<Circuit> _selectedPanelCircuits;
        [JsonIgnore]
        public ObservableCollection<Circuit> SelectedPanelCircuits
        {
            get => _selectedPanelCircuits;
            set => Set(ref _selectedPanelCircuits, value);
        }

        private ObservableCollection<IApartmentElement> _circuitElements;
        [JsonIgnore]
        public ObservableCollection<IApartmentElement> CircuitElements
        {
            get => _circuitElements;
            set => Set(ref _circuitElements, value);
        }

        private ObservableCollection<IApartmentElement> _selectedCircuitElements;
        [JsonIgnore]
        public ObservableCollection<IApartmentElement> SelectedCircuitElements
        {
            get => _selectedCircuitElements;
            set => Set(ref _selectedCircuitElements, value);
        }

        private IApartmentElement _selectedCircuitElement;
        [JsonIgnore]
        public IApartmentElement SelectedCircuitElement
        {
            get => _selectedCircuitElement;
            set => Set(ref _selectedCircuitElement, value);
        }

        private BitmapImage _annotationPreview;
        [JsonIgnore]
        public BitmapImage AnnotationPreview { get => _annotationPreview; set => Set(ref _annotationPreview, value); }

        private string _newCircuit;
        [JsonIgnore]
        public string NewCircuit { get => _newCircuit; set => Set(ref _newCircuit, value); }

        private bool _isCancelEnabled;
        [JsonIgnore]
        public bool IsCancelEnabled { get => _isCancelEnabled; set => Set(ref _isCancelEnabled, value); }

        private string _responsibleForHeight;
        public string ResponsibleForHeight
        {
            get => _responsibleForHeight;
            set
            {
                Set(ref _responsibleForHeight, value);
                if (!IsCancelEnabled) IsCancelEnabled = true;
            }
        }

        private string _responsibleForCircuit;
        public string ResponsibleForCircuit
        {
            get => _responsibleForCircuit;
            set
            {
                Set(ref _responsibleForCircuit, value);
                if (!IsCancelEnabled) IsCancelEnabled = true;
            }
        }

        #region Batch
        private ElementBatch _elementBatch;
        public ElementBatch ElementBatch
        {
            get => _elementBatch;
            set => Set(ref _elementBatch, value);
        }

        private ObservableCollection<ElementBatch> _batches;
        public ObservableCollection<ElementBatch> Batches
        {
            get => _batches;
            set => Set(ref _batches, value);
        }

        private BatchedElement _newElementForBatch;
        public BatchedElement NewElementForBatch
        {
            get => _newElementForBatch;
            set => Set(ref _newElementForBatch, value);
        }

        private BatchedElement _selectedBatchedElement;
        public BatchedElement SelectedBatchedElement
        {
            get => _selectedBatchedElement;
            set => Set(ref _selectedBatchedElement, value);
        }

        private BatchedRow _selectedBatchedRow;
        public BatchedRow SelectedBatchedRow
        {
            get => _selectedBatchedRow;
            set => Set(ref _selectedBatchedRow, value);
        }

        private ObservableCollection<ElementBatch> _selectedBatches;
        [JsonIgnore]
        public ObservableCollection<ElementBatch> SelectedBatches
        {
            get => _selectedBatches;
            set => Set(ref _selectedBatches, value);
        }

        [JsonIgnore]
        public ICommand AddBatchToElementBatchesCommand { get; set; }
        [JsonIgnore]
        public ICommand SelectedBatchesCommand { get; set; }
        [JsonIgnore]
        public ICommand RemoveBatchCommand { get; set; }
        [JsonIgnore]
        public ICommand AddRowToBatchCommand { get; set; }
        [JsonIgnore]
        public ICommand RemoveRowFromBatchCommand { get; set; }
        #endregion

        #region ListHeights
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

        private ObservableCollection<string> _configs;
        [JsonIgnore] 
        public ObservableCollection<string> Configs { get => _configs; set => Set(ref _configs, value); }

        private string _selectedConfig;
        [JsonIgnore]
        public string SelectedConfig { get => _selectedConfig; set => Set(ref _selectedConfig, value); }

        private string _currentConfig; 
        [JsonIgnore]
        public string CurrentConfig { get => _currentConfig; set => Set(ref _currentConfig, value); }
        
        private string _newConfig;
        [JsonIgnore]
        public string NewConfig { get => _newConfig; set => Set(ref _newConfig, value); }

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
        public ICommand SelectApartmentElementsCommand { get; set; }
        [JsonIgnore]
        public ICommand SelectPanelCircuitCommand { get; set; }
        [JsonIgnore]
        public ICommand SelectCircuitElementCommand { get; set; }
        [JsonIgnore]
        public ICommand OkCommand { get; set; }
        [JsonIgnore]
        public ICommand ApplyCommand { get; set; }
        [JsonIgnore]
        public ICommand CancelCommand { get; set; }
        [JsonIgnore]
        public ICommand SetAnnotationToElementCommand { get; set; }
        [JsonIgnore]
        public ICommand SetAnnotationToElementsBatchCommand { get; set; }
        [JsonIgnore]
        public ICommand SetAnnotationPreviewCommand { get; set; }
        [JsonIgnore]
        public ICommand SaveLatestConfigCommand { get; set; }
        [JsonIgnore]
        public ICommand LoadLatestConfigCommand { get; set; }
        [JsonIgnore]
        public ICommand SetNewElementForBatchCommand { get; set; }
        [JsonIgnore]
        public ICommand AddElementToRowCommand { get; set; }
        [JsonIgnore]
        public ICommand RemoveElementFromRowCommand { get; set; }
        [JsonIgnore]
        public ICommand AddConfigCommand { get; set; }
        [JsonIgnore]
        public ICommand RemoveConfigCommand { get; set; }
        [JsonIgnore]
        public Action<object, OkApplyCancel> OkApplyCancelActions { get; set; }

        [JsonIgnore]
        public Action<List<string>> SetParametersToElement { get; set; }

        public ConfigPanelViewModel ApplyLatestConfiguration(ConfigPanelViewModel latestConfiguration)
        {
            ApartmentElements = latestConfiguration.ApartmentElements;
            PanelCircuits = latestConfiguration.PanelCircuits;
            Batches = latestConfiguration.Batches ?? new ObservableCollection<ElementBatch>();

            if (latestConfiguration.ListHeightsOK != null)
                ListHeightsOK = latestConfiguration.ListHeightsOK;
            if (latestConfiguration.ListHeightsUK != null)
                ListHeightsUK = latestConfiguration.ListHeightsUK;
            if (latestConfiguration.ListHeightsCenter != null)
                ListHeightsCenter = latestConfiguration.ListHeightsCenter;

            ResponsibleForHeight = latestConfiguration.ResponsibleForHeight;
            ResponsibleForCircuit = latestConfiguration.ResponsibleForCircuit;

            foreach (var apartmentElement in ApartmentElements)
            {
                string annotationName = new AnnotationNameBuilder()
                    .AddFolders(SelectedConfig)
                    .AddPartsOfName("-", apartmentElement.Family, apartmentElement.Name)
                    .Build();
                //apartmentElement.Annotation = ElementService.GetAnnotationFor(apartmentElement.Name);
                apartmentElement.Annotation = ElementService
                    .SetAnnotationName(annotationName)
                    .GetAnnotation();
            };

            foreach (var batch in Batches)
            {
                //batch.Annotation = ElementService.GetAnnotationFor(batch.Name);
                var annotationNameBuilder = new AnnotationNameBuilder();
                string annotationName = annotationNameBuilder
                    .AddFolders(SelectedConfig)
                    .AddPartsOfName("", batch.Name)
                    .Build();

                //batch.Annotation = ElementService.GetAnnotationFor(batch.Name);
                batch.Annotation = ElementService
                    .SetAnnotationName(annotationName)
                    .GetAnnotation();
                foreach (var row in batch.BatchedRows)
                {
                    foreach (BatchedElement element in row.RowElements)
                    {
                        annotationName = annotationNameBuilder
                            .AddFolders(SelectedConfig)
                            .AddPartsOfName("-", element.Family, element.Name)
                            .Build();

                        element.Annotation = ElementService
                            .SetAnnotationName(annotationName)
                            .GetAnnotation();
                        //element.Annotation = ElementService.GetAnnotationFor(element.Name);
                    }
                }
            }

            for (int i = 0; i < PanelCircuits.Count; i++)
            {
                var circuitElements = PanelCircuits[i].Elements;

                foreach (var apartmentElement in ApartmentElements)
                {
                    var matchingCircuitElement = circuitElements
                        .FirstOrDefault(c => c.Name == apartmentElement.Name && c.Family == apartmentElement.Family);

                    if (matchingCircuitElement != null)
                    {
                        matchingCircuitElement.Annotation = apartmentElement.Annotation;
                        apartmentElement.AnnotationChanged += matchingCircuitElement.AnnotationChanged_Handler;
                    }
                }
                PanelCircuits[i] =
                    new Circuit { Number = PanelCircuits[i].Number, Elements = circuitElements };
            }

            return this;
        }

        private bool TryFindConfigs(out ObservableCollection<string> configs)
        {
            configs = null;
            string assemblyPath = FileUtility.GetAssemblyPath();
            var files = Directory.GetFiles(assemblyPath, "*.json").Select(Path.GetFileName);

            string targetSubstring = "LatestConfig";
            string pattern = $"{targetSubstring}\\.json$";

            foreach (var file in files)
            {
                if (Regex.IsMatch(file, pattern))
                {
                    string configName = GetConfigName(file);
                    bool isEmpty = string.IsNullOrEmpty(configName);
                    if (!isEmpty && configs == null)
                        configs = new ObservableCollection<string> { configName };
                    else if (!isEmpty)
                        configs.Add(configName);
                }
            }
            return configs != null;
        }

        private string GetConfigName(string fileName)
        {
            string targetSubstring = "LatestConfig";

            int startIndex = fileName.IndexOf(targetSubstring);
            if (startIndex != -1)
                return fileName.Substring(0, startIndex);
            return "";
        }
    }
}

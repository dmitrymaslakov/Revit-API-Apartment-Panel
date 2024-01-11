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
using ApartmentPanel.Presentation.Models.Batch;
using ApartmentPanel.Presentation.Models;
using System.Windows;
using System.Windows.Media;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM
{
    public class ConfigPanelViewModel : ViewModelBase, IConfigPanelViewModel
    {
        #region MockFields
        //static string mockAnnotation = "d:/Programming/Revit-2023/ApartmentPanelSln/ApartmentPanel/bin/Debug/Resources/Annotations/Lamp.png";
        static string mockAnnotation = "E:\\Different\\Study\\Programming\\C-sharp\\Revit-API\\Apartment-Panel\\ApartmentPanel\\bin\\Debug\\Resources\\Annotations\\Lamp.png";
        private static ObservableCollection<IApartmentElement> el1 = new ObservableCollection<IApartmentElement>
        {
            new ApartmentElement { Name = "Switch", Annotation = new FileAnnotationReader(mockAnnotation).Get() },
            new ApartmentElement { Name = "Socket", Annotation = new FileAnnotationReader(mockAnnotation).Get() },
            new ApartmentElement { Name = "ThroughSwitch", Annotation = new FileAnnotationReader(mockAnnotation).Get() }
        };
        private static ObservableCollection<IApartmentElement> el2 = new ObservableCollection<IApartmentElement>
        {
            new ApartmentElement { Name = "Smoke Sensor", Annotation = new FileAnnotationReader(mockAnnotation).Get() },
            new ApartmentElement { Name = "USB", Annotation = new FileAnnotationReader(mockAnnotation).Get() },
            new ApartmentElement { Name = "ThroughSwitch", Annotation = new FileAnnotationReader(mockAnnotation).Get() }
        };
        private static int _elementIndex = 1;
        private static IEnumerable<Parameter> _parameters =
            Enumerable.Range(1, 3).Select(i => new Parameter
            {
                Name = $"paramName {_elementIndex++}",
                Value = $"paramValue {_elementIndex++}"
            });
        private static IEnumerable<BatchedElement> _batchedElements1 =
            Enumerable.Range(1, 3).Select(i => new BatchedElement
            {
                Name = $"elName {_elementIndex++}",
                Margin = new BatchedMargin(0, 0, i, 0),
                Annotation = new FileAnnotationReader(mockAnnotation).Get(),
                Parameters = new ObservableCollection<Parameter>(_parameters)
            });
        private static IEnumerable<BatchedRow> _rows =
            Enumerable.Range(1, 2).Select(i => new BatchedRow
            {
                Number = i,
                HeightFromFloor = new Height(),
                RowElements = new ObservableCollection<BatchedElement>(_batchedElements1)
            });

        #endregion
        #region MockProps
        [JsonIgnore]
        public ObservableCollection<IApartmentElement> MockEl1 { get; set; } = el1;
        [JsonIgnore]
        public ObservableCollection<Circuit> MockPanelCircuits { get; set; } = new ObservableCollection<Circuit>
        {
            new Circuit{ Number = "1", Elements = el1 },
            new Circuit{ Number = "2", Elements = el2 },
        };
        [JsonIgnore]
        public ObservableCollection<BatchedRow> MockBatchedRows { get; set; } =
            new ObservableCollection<BatchedRow>(_rows);
        [JsonIgnore]
        public ImageSource MockAnnotation { get; set; } = new FileAnnotationReader(mockAnnotation).Get();
        #endregion

        private readonly ConfigPanelCommandsCreater _commandCreater;

        public ConfigPanelViewModel() { }

        public ConfigPanelViewModel(IElementService elementService,
            IListElementsViewModel listElementsVM) : base(elementService)
        {
            /*ElementBatch = new ElementBatch
            {
                Name = "SomeName",
                Annotation = new FileAnnotationReader(mockAnnotation).Get(),
                BatchedRows = new ObservableCollection<BatchedRow>(_rows)
            };*/
            ElementBatch = new ElementBatch
            {
                BatchedRows = new ObservableCollection<BatchedRow>
                {
                    new BatchedRow()
                }
            };
            Batches = new ObservableCollection<ElementBatch>();
            //NewElementForBatch = new BatchedElement();
            SelectedBatchedElement = new BatchedElement();
            SelectedBatches = new ObservableCollection<ElementBatch>();
            SetParametersToBatchElement = OnSetParametersToBatchElementExecuted;
            _commandCreater = new ConfigPanelCommandsCreater(this, ElementService);
            ListElementsVM = listElementsVM;
            ApartmentElements = new ObservableCollection<IApartmentElement>();
            //PanelCircuits = new ObservableDictionary<string, ObservableCollection<IApartmentElement>>();
            PanelCircuits = new ObservableCollection<Circuit>();
            CircuitElements = new ObservableCollection<IApartmentElement>();
            SelectedApartmentElements = new ObservableCollection<IApartmentElement>();
            /*SelectedPanelCircuits =
                new ObservableCollection<KeyValuePair<string, ObservableCollection<IApartmentElement>>>();*/
            SelectedPanelCircuits = new ObservableCollection<Circuit>();

            SelectedCircuitElements = new ObservableCollection<IApartmentElement>();
            ListHeightsOK = new ObservableCollection<double>();
            ListHeightsUK = new ObservableCollection<double>();
            ListHeightsCenter = new ObservableCollection<double>();
            LatestConfigPath = FileUtility.GetAssemblyPath() + "/LatestConfig.json";
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
        }

        [JsonIgnore]
        public IListElementsViewModel ListElementsVM { get; set; }
        public string LatestConfigPath { get; }

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

        /*private ObservableDictionary<string, ObservableCollection<IApartmentElement>> _panelCircuits;
        public ObservableDictionary<string, ObservableCollection<IApartmentElement>> PanelCircuits
        {
            get => _panelCircuits;
            set => Set(ref _panelCircuits, value);
        }*/

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

        /*private ObservableCollection
            <KeyValuePair<string, ObservableCollection<IApartmentElement>>> _selectedPanelCircuits;
        [JsonIgnore]
        public ObservableCollection
            <KeyValuePair<string, ObservableCollection<IApartmentElement>>> SelectedPanelCircuits
        {
            get => _selectedPanelCircuits;
            set => Set(ref _selectedPanelCircuits, value);
        }*/

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
        public Action<object, OkApplyCancel> OkApplyCancelActions { get; set; }
        [JsonIgnore]
        public Action<List<string>> SetParametersToBatchElement { get; set; }
        private void OnSetParametersToBatchElementExecuted(List<string> parameterNames)
        {
            var parameters = parameterNames
                .Select(pn => new Parameter { Name = pn})
                .ToList();
            NewElementForBatch.Parameters = new ObservableCollection<Parameter>(parameters);
        }

        public ConfigPanelViewModel ApplyLatestConfiguration(ConfigPanelViewModel latestConfiguration)
        {
            ApartmentElements = latestConfiguration.ApartmentElements;
            PanelCircuits = latestConfiguration.PanelCircuits;
            Batches = latestConfiguration.Batches;
            if (latestConfiguration.ListHeightsOK != null)
                ListHeightsOK = latestConfiguration.ListHeightsOK;
            if (latestConfiguration.ListHeightsUK != null)
                ListHeightsUK = latestConfiguration.ListHeightsUK;
            if (latestConfiguration.ListHeightsCenter != null)
                ListHeightsCenter = latestConfiguration.ListHeightsCenter;

            foreach (var apartmentElement in ApartmentElements)
            {
                apartmentElement.Annotation = ElementService.GetAnnotationFor(apartmentElement.Name);
            };
            foreach (var batch in Batches)
            {
                batch.Annotation = ElementService.GetAnnotationFor(batch.Name);
                foreach (var row in batch.BatchedRows)
                {
                    foreach (var element in row.RowElements)
                    {
                        element.Annotation = ElementService.GetAnnotationFor(element.Name);
                    }
                }
            }

            for (int i = 0; i < PanelCircuits.Count; i++)
            {
                var newCircuitElements = new ObservableCollection<IApartmentElement>();
                var circuitElements = PanelCircuits[i].Elements;
                //var circuitElements = PanelCircuits[i].Value;

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
                    new Circuit { Number = PanelCircuits[i].Number, Elements = circuitElements };

                /*PanelCircuits[i] =
                    new KeyValuePair<string, ObservableCollection<IApartmentElement>>(
                        PanelCircuits[i].Key, circuitElements);*/
            }
            return this;
        }
    }
}

using ApartmentPanel.Utility;
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
using ApartmentPanel.Core.Models;
using System.Text.RegularExpressions;
using System.IO;
using ApartmentPanel.Core.Services;
using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM.ConfigPanelComponentsVM;
using AutoMapper;
using ApartmentPanel.Presentation.Models;
using ApartmentPanel.Utility.Mapping;
using ApartmentPanel.Utility.Comparers;
using MediatR;
using ApartmentPanel.UseCases.Configs.Dto;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Enums;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM
{
    public class ConfigPanelViewModel : ViewModelBase, IConfigPanelViewModel//, IMapWith<LatestConfiguration>
    {
        private readonly ConfigPanelCommandsCreater _commandCreater;
        //private readonly IMapper _mapper;

        public ConfigPanelViewModel() { }

        public ConfigPanelViewModel(IElementService elementService,
            IListElementsViewModel listElementsVM, IMapper mapper, IMediator mediator) : base(elementService)
        {
            //_mapper = mapper;
            _commandCreater = new ConfigPanelCommandsCreater(this, ElementService, mapper, mediator);
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
            ListElementsVM = listElementsVM;
            //ApartmentElements = new ObservableCollection<IApartmentElement>();
            /*var aeCommandCreater = new ApartmentElementsCommandCreater(this, ElementService);
            var aeVM = new ApartmentElementsViewModel(aeCommandCreater);*/
            ApartmentElementsVM = new ApartmentElementsViewModel(this, ElementService, mediator, mapper);
            PanelCircuitsVM = new PanelCircuitsViewModel(this);
            CircuitElementsVM = new CircuitElementsViewModel(this);
            /*PanelCircuits = new ObservableCollection<Circuit>();
            SelectedPanelCircuits = new ObservableCollection<Circuit>();*/
            /*CircuitElements = new ObservableCollection<IApartmentElement>();
            SelectedCircuitElements = new ObservableCollection<IApartmentElement>();*/
            ListHeightsOK = new ObservableCollection<double>();
            ListHeightsUK = new ObservableCollection<double>();
            ListHeightsCenter = new ObservableCollection<double>();
            if (TryFindConfigs(out ObservableCollection<string> configs))
                Configs = configs;
            else Configs = new ObservableCollection<string>();

            IsCancelEnabled = false;
            /*SelectedApartmentElements = new ObservableCollection<IApartmentElement>();
            ShowListElementsCommand = _commandCreater.CreateShowListElementsCommand();
            RemoveApartmentElementsCommand = _commandCreater.CreateRemoveElementsFromApartmentCommand();
            SelectApartmentElementsCommand = _commandCreater.CreateSelectApartmentElementsCommand();*/
            /*AddPanelCircuitCommand = _commandCreater.CreateAddCircuitToPanelCommand();
            RemovePanelCircuitsCommand = _commandCreater.CreateRemoveCircuitsFromPanelCommand();
            SelectPanelCircuitCommand = _commandCreater.CreateSelectPanelCircuitCommand();*/
            AddElementToCircuitCommand = _commandCreater.CreateAddElementToCircuitCommand();
            RemoveElementsFromCircuitCommand = _commandCreater.CreateRemoveElementsFromCircuitCommand();
            //SelectCircuitElementCommand = _commandCreater.CreateSelectCircuitElementCommand();
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

        /*private ObservableCollection<IApartmentElement> _apartmentElements;
        public ObservableCollection<IApartmentElement> ApartmentElements
        {
            get => _apartmentElements;
            set => Set(ref _apartmentElements, value);
        }

        private ObservableCollection<IApartmentElement> _selectedApartmentElements;
        [JsonIgnore]
        public ObservableCollection<IApartmentElement> SelectedApartmentElements
        {
            get => _selectedApartmentElements;
            set => Set(ref _selectedApartmentElements, value);
        }*/

        private ApartmentElementsViewModel _apartmentElementsVM;
        public ApartmentElementsViewModel ApartmentElementsVM
        {
            get => _apartmentElementsVM;
            set => Set(ref _apartmentElementsVM, value);
        }

        private PanelCircuitsViewModel _panelCircuitsVM;
        public PanelCircuitsViewModel PanelCircuitsVM
        {
            get => _panelCircuitsVM;
            set => Set(ref _panelCircuitsVM, value);
        }

        private CircuitElementsViewModel _circuitElementsVM;
        public CircuitElementsViewModel CircuitElementsVM
        {
            get => _circuitElementsVM;
            set => Set(ref _circuitElementsVM, value);
        }

        /*private ObservableCollection<Circuit> _panelCircuits;
        public ObservableCollection<Circuit> PanelCircuits
        {
            get => _panelCircuits;
            set => Set(ref _panelCircuits, value);
        }

        private ObservableCollection<Circuit> _selectedPanelCircuits;
        [JsonIgnore]
        public ObservableCollection<Circuit> SelectedPanelCircuits
        {
            get => _selectedPanelCircuits;
            set => Set(ref _selectedPanelCircuits, value);
        }*/

        /*private string _newCircuit;
        [JsonIgnore]
        public string NewCircuit { get => _newCircuit; set => Set(ref _newCircuit, value); }*/

        /*private ObservableCollection<IApartmentElement> _circuitElements;
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
        }*/

        private BitmapImage _annotationPreview;
        [JsonIgnore]
        public BitmapImage AnnotationPreview { get => _annotationPreview; set => Set(ref _annotationPreview, value); }

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

        /*[JsonIgnore]
        public ICommand ShowListElementsCommand { get; set; }
        [JsonIgnore]
        public ICommand RemoveApartmentElementsCommand { get; set; }
        [JsonIgnore]
        public ICommand SelectApartmentElementsCommand { get; set; }*/
        /*[JsonIgnore]
        public ICommand AddPanelCircuitCommand { get; set; }
        [JsonIgnore]
        public ICommand RemovePanelCircuitsCommand { get; set; }
        [JsonIgnore]
        public ICommand SelectPanelCircuitCommand { get; set; }*/
        [JsonIgnore]
        public ICommand AddElementToCircuitCommand { get; set; }
        [JsonIgnore]
        public ICommand RemoveElementsFromCircuitCommand { get; set; }
        /*[JsonIgnore]
        public ICommand SelectCircuitElementCommand { get; set; }*/
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

        //public ConfigPanelViewModel ApplyLatestConfiguration(ConfigPanelViewModel latestConfiguration)
        public ConfigPanelViewModel ApplyLatestConfiguration(GetConfigDto latestConfiguration)
        {
            ApartmentElementsVM.ApartmentElements =
                //latestConfiguration.ApartmentElementsVM.ApartmentElements;
                new ObservableCollection<IApartmentElement>(latestConfiguration.ApartmentElements);
            var sortedCircuits = //latestConfiguration.PanelCircuitsVM.PanelCircuits
                latestConfiguration.Circuits
                  .OrderBy(c => c.Number, new StringNumberComparer());
            PanelCircuitsVM.PanelCircuits
                = new ObservableCollection<Circuit>(sortedCircuits);
            Batches = //latestConfiguration.Batches ?? new ObservableCollection<ElementBatch>();
                new ObservableCollection<ElementBatch>(latestConfiguration.ElementBatches);
            /*if (latestConfiguration.ListHeightsOK != null)
                ListHeightsOK = latestConfiguration.ListHeightsOK;
            if (latestConfiguration.ListHeightsUK != null)
                ListHeightsUK = latestConfiguration.ListHeightsUK;
            if (latestConfiguration.ListHeightsCenter != null)
                ListHeightsCenter = latestConfiguration.ListHeightsCenter;*/
            var groupedHeight = latestConfiguration.Heights
                .GroupBy(g => g.TypeOf);

            foreach (IGrouping<TypeOfHeight, Height> group in groupedHeight)
            {
                switch (group.Key)
                {
                    case TypeOfHeight.OK:
                        ListHeightsOK = GetGroupedHeightValues(group);
                        break;
                    case TypeOfHeight.UK:
                        ListHeightsUK = GetGroupedHeightValues(group);
                        break;
                    case TypeOfHeight.Center:
                        ListHeightsCenter = GetGroupedHeightValues(group);
                        break;
                }
            }

            ResponsibleForHeight = latestConfiguration.ResponsibleForHeights.FirstOrDefault();
            ResponsibleForCircuit = latestConfiguration.ResponsibleForHeights.FirstOrDefault();

            /*foreach (var apartmentElement in ApartmentElementsVM.ApartmentElements)
            {
                string annotationName = new AnnotationNameBuilder()
                    .AddFolders(SelectedConfig)
                    .AddPartsOfName("-", apartmentElement.Family, apartmentElement.Name)
                    .Build();
                apartmentElement.Annotation = ElementService
                    .SetAnnotationName(annotationName)
                    .GetAnnotation();
            };*/

            /*foreach (var batch in Batches)
            {
                var annotationNameBuilder = new PathBuilder();
                string annotationName = annotationNameBuilder
                    .AddFolders(SelectedConfig)
                    .AddPartsOfName("", batch.Name)
                    .Build();

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
                    }
                }
            }*/

            /*for (int i = 0; i < PanelCircuitsVM.PanelCircuits.Count; i++)
            {
                var circuitElements = PanelCircuitsVM.PanelCircuits[i].Elements;

                foreach (var apartmentElement in ApartmentElementsVM.ApartmentElements)
                {
                    var matchingCircuitElement = circuitElements
                        .FirstOrDefault(c => c.Name == apartmentElement.Name && c.Family == apartmentElement.Family);

                    if (matchingCircuitElement != null)
                    {
                        matchingCircuitElement.Annotation = apartmentElement.Annotation;
                        apartmentElement.AnnotationChanged += matchingCircuitElement.AnnotationChanged_Handler;
                    }
                }
                PanelCircuitsVM.PanelCircuits[i] =
                    new Circuit { Number = PanelCircuitsVM.PanelCircuits[i].Number, Elements = circuitElements };
            }*/

            return this;
        }

        private ObservableCollection<double> GetGroupedHeightValues(IGrouping<TypeOfHeight, Height> group)
        {
            return new ObservableCollection<double>(group.Select(h => h.FromFloor));
        }
        /*public void Mapping(Profile profile)
        {
            profile.CreateMap<LatestConfiguration, ConfigPanelViewModel>()
                .ForPath(vm => vm.ApartmentElementsVM.ApartmentElements,
                    opt => opt.MapFrom(latestConf => latestConf.ApartmentElements))
                .ForPath(vm => vm.PanelCircuitsVM.PanelCircuits,
                    opt => opt.MapFrom(latestConf => latestConf.PanelCircuits))
                .ForMember(vm => vm.ResponsibleForHeight,
                    opt => opt.MapFrom(latestConf => latestConf.ResponsibleForHeight))
                .ForMember(vm => vm.ResponsibleForCircuit,
                    opt => opt.MapFrom(latestConf => latestConf.ResponsibleForCircuit))
                .ForMember(vm => vm.Batches,
                    opt => opt.MapFrom(latestConf => latestConf.Batches))
                .ForMember(vm => vm.ListHeightsOK,
                    opt => opt.MapFrom(latestConf => latestConf.ListHeightsOK))
                .ForMember(vm => vm.ListHeightsUK,
                    opt => opt.MapFrom(latestConf => latestConf.ListHeightsUK))
                .ForMember(vm => vm.ListHeightsCenter,
                    opt => opt.MapFrom(latestConf => latestConf.ListHeightsCenter));
        }*/

        private bool TryFindConfigs(out ObservableCollection<string> configs)
        {
            configs = null;
            string assemblyPath = FileUtility.GetAssemblyFolder();
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

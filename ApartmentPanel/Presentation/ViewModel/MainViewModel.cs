using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ApartmentPanel.Presentation.Commands;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.Services;
using System;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.Core.Enums;

namespace ApartmentPanel.Presentation.ViewModel
{
    public enum OkApplyCancel
    {
        Ok, Apply, Cancel
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        #region MockFields
        private readonly ModelAnalizing _modelAnalizing;
        #endregion
        #region MockProps
        public ICommand AnalizeCommand { get; set; }
        #endregion

        private readonly ViewCommandsCreater _viewCommandsCreater;

        public MainViewModel()
        {

        }
        public MainViewModel(IElementService elementService,
            IConfigPanelViewModel configPanelVM, ModelAnalizing modelAnalizing) : base(elementService)
        {
            #region MockInitializing
            _modelAnalizing = modelAnalizing;
            AnalizeCommand = new RelayCommand(o =>
            {
                modelAnalizing.AnalizeElement();
            });
            #endregion

            _viewCommandsCreater = new ViewCommandsCreater(this, ElementService);
            ConfigPanelVM = configPanelVM;
            ConfigPanelVM.OkApplyCancelActions = ExecuteOkApplyCancelActions;
            Circuits = GetCircuits(ConfigPanelVM.PanelCircuits);
            ElementBatches = GetBatches(ConfigPanelVM.Batches);
            ListHeightsUK = GetHeights(ConfigPanelVM.ListHeightsUK);
            ListHeightsOK = GetHeights(ConfigPanelVM.ListHeightsOK);
            ListHeightsCenter = GetHeights(ConfigPanelVM.ListHeightsCenter);
            Direction = Direction.None;
            ConfigureCommand = _viewCommandsCreater.CreateConfigureCommand();
            InsertElementCommand = _viewCommandsCreater.CreateInsertElementCommand();
            InsertBatchCommand = _viewCommandsCreater.CreateInsertBatchCommand();
            SetCurrentSuffixCommand = _viewCommandsCreater.CreateSetCurrentSuffixCommand();
            ClearCurrentSuffixCommand = _viewCommandsCreater.CreateClearCurrentSuffixCommand();
            SetHeightCommand = _viewCommandsCreater.CreateSetHeightCommand();
            SetStatusCommand = _viewCommandsCreater.CreateSetStatusCommand();
            ResetHeightCommand = _viewCommandsCreater.CreateResetHeightCommand();
            SetDirectionCommand = _viewCommandsCreater.CreateSetDirectionCommand();
        }

        public IConfigPanelViewModel ConfigPanelVM { get; set; }

        private ObservableCollection<Circuit> _circuits;
        public ObservableCollection<Circuit> Circuits
        {
            get => _circuits;
            set => Set(ref _circuits, value);
        }

        private ObservableCollection<ElementBatch> _elementBatches;
        public ObservableCollection<ElementBatch> ElementBatches
        {
            get => _elementBatches;
            set => Set(ref _elementBatches, value);
        }

        private string _currentSuffix;
        public string CurrentSuffix
        {
            get => _currentSuffix;
            set => Set(ref _currentSuffix, value);
        }

        private Height _elementHeight;
        public Height ElementHeight
        {
            get => _elementHeight;
            set => Set(ref _elementHeight, value);
        }
        
        private double _floorHeight;
        public double FloorHeight
        {
            get => _floorHeight;
            set => Set(ref _floorHeight, value);
        }

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

        private bool _isResetHeight;
        public bool IsResetHeight
        {
            get => _isResetHeight;
            set => Set(ref _isResetHeight, value);
        }
        #endregion

        private string _status;
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        private Direction _direction;
        public Direction Direction
        {
            get => _direction;
            set => Set(ref _direction, value);
        }

        public ICommand ConfigureCommand { get; set; }
        public ICommand InsertElementCommand { get; set; }
        public ICommand InsertBatchCommand { get; set; }
        public ICommand SetCurrentSuffixCommand { get; set; }
        public ICommand ClearCurrentSuffixCommand { get; set; }
        public ICommand SetHeightCommand { get; set; }
        public ICommand SetStatusCommand { get; set; }
        public ICommand ResetHeightCommand { get; set; }
        public ICommand SetDirectionCommand { get; set; }

        private void ExecuteOkApplyCancelActions(object obj, OkApplyCancel okApplyCancel)
        {
            if (string.IsNullOrEmpty(ConfigPanelVM.LatestConfigPath)) return;
            ObservableCollection<Circuit> panelCircuits = null;
            ObservableCollection<ElementBatch> batches = null;
            ObservableCollection<double> heightsOk = null;
            ObservableCollection<double> heightsUk = null;
            ObservableCollection<double> heightsCenter = null;
            if (obj != null)
                (panelCircuits, batches, heightsOk, heightsUk, heightsCenter) =
                (ValueTuple<ObservableCollection<Circuit>,
                ObservableCollection<ElementBatch>,
                ObservableCollection<double>,
                ObservableCollection<double>,
                ObservableCollection<double>>)obj;

            switch (okApplyCancel)
            {
                case OkApplyCancel.Ok:
                case OkApplyCancel.Apply:
                    Circuits.Clear();
                    Circuits = GetCircuits(panelCircuits);
                    ElementBatches.Clear();
                    ElementBatches = GetBatches(batches);
                    ListHeightsOK = GetHeights(heightsOk);
                    ListHeightsUK = GetHeights(heightsUk);
                    ListHeightsCenter = GetHeights(heightsCenter);
                    ConfigPanelVM.SaveLatestConfigCommand?.Execute(ConfigPanelVM);
                    break;
                case OkApplyCancel.Cancel:
                    ConfigPanelVM.LoadLatestConfigCommand?.Execute(null);
                    break;
            }
        }

        private ObservableCollection<Circuit> GetCircuits(ObservableCollection<Circuit> panelCircuits)
        {
            var result = new ObservableCollection<Circuit>();
            foreach (var circuit in panelCircuits)
            {
                result.Add(new Circuit
                {
                    Number = circuit.Number,
                    Elements = new ObservableCollection<IApartmentElement>(
                            circuit.Elements.Select(ap => ap.Clone()).ToList())
                });
            }
            return result;
        }

        private ObservableCollection<ElementBatch> GetBatches(ObservableCollection<ElementBatch> batches)
        {
            var result = new ObservableCollection<ElementBatch>();
            if (batches != null)
            {
                foreach (var batch in batches)
                {
                    result.Add(batch.Clone());
                }
            }
            return result;
        }

        private ObservableCollection<double> GetHeights(ObservableCollection<double> heights) =>
            new ObservableCollection<double>(heights.ToList());
    }
}

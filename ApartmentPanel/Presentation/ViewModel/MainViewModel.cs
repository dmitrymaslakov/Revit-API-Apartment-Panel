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
using ApartmentPanel.Presentation.Models.Batch;

namespace ApartmentPanel.Presentation.ViewModel
{
    public enum OkApplyCancel
    {
        Ok, Apply, Cancel
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        #region MockFields
        private static ObservableCollection<IApartmentElement> el1 = new ObservableCollection<IApartmentElement>
        {
            new ApartmentElement { Name = "Switch", Category = "LightDev", },
            new ApartmentElement { Name = "Socket", Category = "ElFix"},
            new ApartmentElement { Name = "ThroughSwitch", Category= "LightDev"}
        };
        private static ObservableCollection<IApartmentElement> el2 = new ObservableCollection<IApartmentElement>
        {
            new ApartmentElement { Name = "Smoke Sensor", Category = "LightDev"},
            new ApartmentElement { Name = "USB", Category = "LightDev"},
            new ApartmentElement { Name = "ThroughSwitch", Category = "LightDev"}
        };
        private readonly ModelAnalizing _modelAnalizing;
        #endregion
        #region MockProps
        public ObservableCollection<Circuit> MockCircuits { get; set; } = new ObservableCollection<Circuit>
        {
            new Circuit{Number="1", Elements=el1},
            new Circuit{Number="2", Elements=el2},
        };
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
            ConfigPanelVM.LoadLatestConfigCommand?.Execute(null);
            Circuits = GetCircuits(ConfigPanelVM.PanelCircuits);
            ElementBatches = GetBatches(ConfigPanelVM.Batches);
            ListHeightsUK = GetHeights(ConfigPanelVM.ListHeightsUK);
            ListHeightsOK = GetHeights(ConfigPanelVM.ListHeightsOK);
            ListHeightsCenter = GetHeights(ConfigPanelVM.ListHeightsCenter);
            ConfigureCommand = _viewCommandsCreater.CreateConfigureCommand();
            InsertElementCommand = _viewCommandsCreater.CreateInsertElementCommand();
            InsertBatchCommand = _viewCommandsCreater.CreateInsertBatchCommand();
            SetCurrentSuffixCommand = _viewCommandsCreater.CreateSetCurrentSuffixCommand();
            SetHeightCommand = _viewCommandsCreater.CreateSetHeightCommand();
            SetStatusCommand = _viewCommandsCreater.CreateSetStatusCommand();
            ResetHeightCommand = _viewCommandsCreater.CreateResetHeightCommand();
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

        public ICommand ConfigureCommand { get; set; }
        public ICommand InsertElementCommand { get; set; }
        public ICommand InsertBatchCommand { get; set; }
        public ICommand SetCurrentSuffixCommand { get; set; }
        public ICommand SetHeightCommand { get; set; }
        public ICommand SetStatusCommand { get; set; }
        public ICommand ResetHeightCommand { get; set; }

        private void ExecuteOkApplyCancelActions(object obj, OkApplyCancel okApplyCancel)
        {
            //(ObservableDictionary<string, ObservableCollection<IApartmentElement>> panelCircuits,
            (ObservableCollection<Circuit> panelCircuits,
                ObservableCollection<ElementBatch> batches,
                ObservableCollection<double> heightsOk,
                ObservableCollection<double> heightsUk,
                ObservableCollection<double> heightsCenter) =
                //(ValueTuple<ObservableDictionary<string, ObservableCollection<IApartmentElement>>,
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
                    //var panelCircuits =
                    //    (ObservableDictionary<string, ObservableCollection<IApartmentElement>>)obj;
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
        //ObservableDictionary<string, ObservableCollection<IApartmentElement>> panelCircuits)
        {
            var result = new ObservableCollection<Circuit>();
            foreach (var circuit in panelCircuits)
            {
                result.Add(new Circuit
                {
                    Number = circuit.Number,
                    Elements = new ObservableCollection<IApartmentElement>(
                            circuit.Elements.Select(ap => ap.Clone()).ToList())
                    /*Number = circuit.Key,
                    Elements = new ObservableCollection<IApartmentElement>(
                            circuit.Value.Select(ap => ap.Clone()).ToList())*/
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

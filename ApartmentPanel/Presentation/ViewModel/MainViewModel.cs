using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ApartmentPanel.Utility;
using ApartmentPanel.Presentation.Commands;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.Services;
using ApartmentPanel.Presentation.Models;
using System;
using ApartmentPanel.Core.Models;
using System.Collections.Generic;
using System.Windows.Media;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using Autodesk.Revit.DB;
using ApartmentPanel.Utility.AnnotationUtility;

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
        #endregion
        #region MockProps
        public ObservableCollection<Circuit> MockCircuits { get; set; } = new ObservableCollection<Circuit>
        {
            new Circuit{Number="1", Elements=el1},
            new Circuit{Number="2", Elements=el2},
        };

        #endregion

        private readonly ViewCommandsCreater _viewCommandsCreater;
        //private readonly ModelAnalizing _modelAnalizing;

        public MainViewModel()
        {

        }
        public MainViewModel(IElementService elementService,
            IConfigPanelViewModel configPanelVM, ModelAnalizing modelAnalizing) : base(elementService)
        {
            _viewCommandsCreater = new ViewCommandsCreater(this, ElementService);

            ConfigPanelVM = configPanelVM;

            //_modelAnalizing = modelAnalizing;

            ConfigPanelVM.OkApplyCancelActions = ExecuteOkApplyCancelActions;

            ConfigPanelVM.LoadLatestConfigCommand?.Execute(null);

            Circuits = GetCircuits(ConfigPanelVM.PanelCircuits);

            ListHeightsUK = GetHeights(ConfigPanelVM.ListHeightsUK);

            ListHeightsOK = GetHeights(ConfigPanelVM.ListHeightsOK);

            ListHeightsCenter = GetHeights(ConfigPanelVM.ListHeightsCenter);

            ConfigureCommand = _viewCommandsCreater.CreateConfigureCommand();

            InsertElementCommand = _viewCommandsCreater.CreateInsertElementCommand();

            SetCurrentSuffixCommand = _viewCommandsCreater.CreateSetCurrentSuffixCommand();

            SetHeightCommand = _viewCommandsCreater.CreateSetHeightCommand();

            SetStatusCommand = _viewCommandsCreater.CreateSetStatusCommand();

            AnalizeCommand = new RelayCommand(o =>
            {
                modelAnalizing.AnalizeElement();
            });
        }

        public IConfigPanelViewModel ConfigPanelVM { get; set; }

        private ObservableCollection<Circuit> _circuits;

        public ObservableCollection<Circuit> Circuits
        {
            get => _circuits;
            set => Set(ref _circuits, value);
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
        #endregion

        private string _status;

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        public ICommand ConfigureCommand { get; set; }

        public ICommand InsertElementCommand { get; set; }

        public ICommand SetCurrentSuffixCommand { get; set; }

        public ICommand SetHeightCommand { get; set; }

        public ICommand SetStatusCommand { get; set; }

        public ICommand AnalizeCommand { get; set; }

        private void ExecuteOkApplyCancelActions(object obj, OkApplyCancel okApplyCancel)
        {
            (ObservableDictionary<string, ObservableCollection<IApartmentElement>> panelCircuits,
                ObservableCollection<double> heightsOk,
                ObservableCollection<double> heightsUk,
                ObservableCollection<double> heightsCenter) =
                (ValueTuple<ObservableDictionary<string, ObservableCollection<IApartmentElement>>,
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

        private ObservableCollection<Circuit> GetCircuits(
            ObservableDictionary<string, ObservableCollection<IApartmentElement>> panelCircuits)
        {
            var result = new ObservableCollection<Circuit>();
            foreach (var circuit in panelCircuits)
            {
                result.Add(new Circuit
                {
                    Number = circuit.Key,
                    Elements = new ObservableCollection<IApartmentElement>(
                            circuit.Value.Select(ap => ap.Clone()).ToList())
                });
            }
            return result;
        }

        private ObservableCollection<double> GetHeights(ObservableCollection<double> heights)
        {
            return new ObservableCollection<double>(heights.ToList());
        }
    }
}

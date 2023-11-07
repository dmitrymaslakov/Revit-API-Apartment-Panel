using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ApartmentPanel.Utility;
using ApartmentPanel.Presentation.Commands;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using System.Windows;
using ApartmentPanel.Presentation.Services;

namespace ApartmentPanel.Presentation.ViewModel
{
    public enum OkApplyCancel
    {
        Ok, Apply, Cancel
    }

    public class MainViewModel : ViewModelBase, IMainViewModel 
    {
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

            ConfigureCommand = _viewCommandsCreater.CreateConfigureCommand();

            InsertElementCommand = _viewCommandsCreater.CreateInsertElementCommand();

            SetCurrentSuffixCommand = _viewCommandsCreater.CreateSetCurrentSuffixCommand();

            HeightTypeOfUK = 40.0;

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

        #region Heights
        private double _heightTypeOfUK;

        public double HeightTypeOfUK
        {
            get => _heightTypeOfUK;
            set => Set(ref _heightTypeOfUK, value);
        }

        private double _heightTypeOfOK;

        public double HeightTypeOfOK
        {
            get => _heightTypeOfOK;
            set => Set(ref _heightTypeOfOK, value);
        }

        private double _heightTypeOfCenter;

        public double HeightTypeOfCenter
        {
            get => _heightTypeOfCenter;
            set => Set(ref _heightTypeOfCenter, value);
        }
        #endregion

        private ObservableCollection<double> _defaultHeightsTypeOfUK;

        public ObservableCollection<double> DefaultHeightsTypeOfUK
        {
            get => _defaultHeightsTypeOfUK;
            set => Set(ref _defaultHeightsTypeOfUK, value);
        }

        private ObservableCollection<double> _defaultHeightsTypeOfOK;

        public ObservableCollection<double> DefaultHeightsTypeOfOK
        {
            get => _defaultHeightsTypeOfOK;
            set => Set(ref _defaultHeightsTypeOfOK, value);
        }

        private ObservableCollection<double> _defaultHeightsTypeOfCenter;

        public ObservableCollection<double> DefaultHeightsTypeOfCenter
        {
            get => _defaultHeightsTypeOfCenter;
            set => Set(ref _defaultHeightsTypeOfCenter, value);
        }

        private string _status;

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        public ICommand ConfigureCommand { get; set; }

        public ICommand InsertElementCommand { get; set; }

        public ICommand SetCurrentSuffixCommand { get; set; }

        public ICommand AnalizeCommand { get; set; }

        /*public ICommand SaveLatestConfigCommand { get; set; }

        public ICommand LoadLatestConfigCommand { get; set; }*/

        private void ExecuteOkApplyCancelActions(object obj, OkApplyCancel okApplyCancel)
        {
            switch (okApplyCancel)
            {
                case OkApplyCancel.Ok:
                case OkApplyCancel.Apply:
                    Circuits.Clear();
                    var panelCircuits =
                        (ObservableDictionary<string, ObservableCollection<IApartmentElement>>)obj;
                    Circuits = GetCircuits(panelCircuits);
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

    }
}

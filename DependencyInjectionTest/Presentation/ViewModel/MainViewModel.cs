using Autodesk.Revit.UI;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using DependencyInjectionTest.Utility;
using DependencyInjectionTest.Presentation.ViewModel.ComponentsVM;
using DependencyInjectionTest.Infrastructure;
using DependencyInjectionTest.Presentation.Commands;
using DependencyInjectionTest.Core.Models;
using DependencyInjectionTest.Core.Services.Interfaces;
using DependencyInjectionTest.Presentation.ViewModel.Interfaces;
using DependencyInjectionTest.Core.Models.Interfaces;

namespace DependencyInjectionTest.Presentation.ViewModel
{
    public enum OkApplyCancel
    {
        Ok, Apply, Cancel
    }

    public class MainViewModel : ViewModelBase, IMainViewModel//, IViewPropsForCommandsCreater, 
    {
        private readonly ViewCommandsCreater _viewCommandsCreater;
        private readonly IElementService _apartmentElementService;
        private readonly IPanelService _apartmentPanelService;

        public MainViewModel(IElementService apartmentElementService,
            IPanelService apartmentPanelService,
            IConfigPanelViewModel configPanelVM)
        /*public ViewModel(ExternalEvent exEvent, RequestHandler handler)
            : base(exEvent, handler)*/
        {
            _apartmentElementService = apartmentElementService;

            _apartmentPanelService = apartmentPanelService;

            _viewCommandsCreater = new ViewCommandsCreater(this, _apartmentElementService, _apartmentPanelService);

            ConfigPanelVM = configPanelVM;

            ConfigPanelVM.OkApplyCancelActions = ExecuteOkApplyCancelActions;
            //ConfigPanelVM = new ConfigPanelVM(exEvent, handler)
            /*ConfigPanelVM = new ConfigPanelViewModel()
            {
                OkApplyCancelActions = ExecuteOkApplyCancelActions
            };*/

            ConfigPanelVM.LoadLatestConfigCommand?.Execute(null);

            Circuits = GetCircuits(ConfigPanelVM.PanelCircuits);

            ConfigureCommand = _viewCommandsCreater.CreateConfigureCommand();

            InsertElementCommand = _viewCommandsCreater.CreateInsertElementCommand();

            SetCurrentSuffixCommand = _viewCommandsCreater.CreateSetCurrentSuffixCommand();

            Height = 40.0;
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

        private double _height;

        public double Height
        {
            get => _height;
            set => Set(ref _height, value);
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

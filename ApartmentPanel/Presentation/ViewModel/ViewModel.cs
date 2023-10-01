using Autodesk.Revit.UI;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Utility;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Infrastructure;
using ApartmentPanel.Presentation.Commands;

namespace ApartmentPanel.Presentation.ViewModel
{
    public enum OkApplyCancel
    {
        Ok, Apply, Cancel
    }

    public class ViewModel : ViewModelBase, IViewForCommandsCreater
    {
        private readonly ViewCommandsCreater _uICommandsCreater;

        public ViewModel() { }

        public ViewModel(ExternalEvent exEvent, RequestHandler handler)
            : base(exEvent, handler)
        {
            _uICommandsCreater = new ViewCommandsCreater(this);

            EditPanelVM = new ConfigPanelVM(exEvent, handler)
            {
                OkApplyCancelActions = ExecuteOkApplyCancelActions
            };

            EditPanelVM.LoadLatestConfigCommand?.Execute(null);

            Circuits = GetCircuits(EditPanelVM.PanelCircuits);

            ConfigureCommand = _uICommandsCreater.CreateConfigureCommand();

            InsertElementCommand = _uICommandsCreater.CreateInsertElementCommand();

            SetCurrentSuffixCommand = _uICommandsCreater.CreateSetCurrentSuffixCommand();

            Height = 40.0;
        }

        public ConfigPanelVM EditPanelVM { get; set; }

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
                        (ObservableDictionary<string, ObservableCollection<ApartmentElement>>)obj;
                    Circuits = GetCircuits(panelCircuits);
                    EditPanelVM.SaveLatestConfigCommand?.Execute(EditPanelVM);
                    break;
                case OkApplyCancel.Cancel:
                    EditPanelVM.LoadLatestConfigCommand?.Execute(null);
                    break;
            }
        }

        private ObservableCollection<Circuit> GetCircuits(
            ObservableDictionary<string, ObservableCollection<ApartmentElement>> panelCircuits)
        {
            var result = new ObservableCollection<Circuit>();
            foreach (var circuit in panelCircuits)
            {
                result.Add(new Circuit
                {
                    Number = circuit.Key,
                    ApartmentElements = new ObservableCollection<ApartmentElement>(
                            circuit.Value.Select(ap => ap.Clone()).ToList())
                });
            }
            return result;
        }
    }
}

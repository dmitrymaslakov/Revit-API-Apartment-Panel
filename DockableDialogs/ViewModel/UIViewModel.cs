using Autodesk.Revit.UI;
using System.Linq;
using System.Windows.Input;
using DockableDialogs.Domain;
using System.Collections.ObjectModel;
using DockableDialogs.Domain.Models;
using DockableDialogs.Domain.Services.Commands;
using DockableDialogs.ViewModel.ComponentsVM;
using DockableDialogs.Utility;

namespace DockableDialogs.ViewModel
{
    public enum OkApplyCancel
    {
        Ok, Apply, Cancel
    }

    public class UIViewModel : ViewModelBase, IUIToCommandsCreater
    {
        private readonly UICommandsCreater _uICommandsCreater;

        public UIViewModel(ExternalEvent exEvent, RequestHandler handler)
            : base(exEvent, handler)
        {
            _uICommandsCreater = new UICommandsCreater(this);

            EditPanelVM = new EditPanelVM(exEvent, handler)
            {
                OkApplyCancelActions = ExecuteOkApplyCancelActions
            };

            LatestConfigPath = FileUtility.GetAssemblyPath() + "\\LatestConfig.json";

            LoadLatestConfigCommand = _uICommandsCreater.CreateLoadLatestConfigCommand();

            LoadLatestConfigCommand.Execute(null);

            Circuits = GetCircuits(EditPanelVM.PanelCircuits);

            ConfigureCommand = _uICommandsCreater.CreateConfigureCommand();

            InsertElementCommand = _uICommandsCreater.CreateInsertElementCommand();

            SetCurrentSuffixCommand = _uICommandsCreater.CreateSetCurrentSuffixCommand();

            SaveLatestConfigCommand = _uICommandsCreater.CreateSaveLatestConfigCommand();

            Height = 40.0;
        }

        public string LatestConfigPath { get; }

        public EditPanelVM EditPanelVM { get; set; }

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

        public ICommand SaveLatestConfigCommand { get; set; }

        public ICommand LoadLatestConfigCommand { get; set; }

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
                    break;
                case OkApplyCancel.Cancel:
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

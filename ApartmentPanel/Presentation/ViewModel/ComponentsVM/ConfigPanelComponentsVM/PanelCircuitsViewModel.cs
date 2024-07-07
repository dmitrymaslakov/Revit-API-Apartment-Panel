using ApartmentPanel.Core.Models;
using ApartmentPanel.Presentation.Commands.ConfigPanelCommands;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM.ConfigPanelComponentsVM
{
    public class PanelCircuitsViewModel : ViewModelBase
    {
        private readonly PanelCircuitsCommandCreater _commandCreater;
        public PanelCircuitsViewModel() { }
        public PanelCircuitsViewModel(IConfigPanelViewModel configPanelVM) 
        {
            _commandCreater = new PanelCircuitsCommandCreater(configPanelVM);
            PanelCircuits = new ObservableCollection<Circuit>();
            SelectedPanelCircuits = new ObservableCollection<Circuit>();
            AddPanelCircuitCommand = _commandCreater.CreateAddPanelCircuitCommand();
            RemovePanelCircuitsCommand = _commandCreater.CreateRemovePanelCircuitsCommand();
            SelectPanelCircuitCommand = _commandCreater.CreateSelectPanelCircuitCommand();
        }

        private ObservableCollection<Circuit> _panelCircuits;
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
        }

        private string _newCircuit;
        [JsonIgnore]
        public string NewCircuit 
        { 
            get => _newCircuit; 
            set => Set(ref _newCircuit, value); 
        }

        [JsonIgnore]
        public ICommand AddPanelCircuitCommand { get; }
        [JsonIgnore]
        public ICommand RemovePanelCircuitsCommand { get; }
        [JsonIgnore]
        public ICommand SelectPanelCircuitCommand { get; }
    }
}

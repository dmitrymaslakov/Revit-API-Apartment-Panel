using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Presentation.Commands;
using ApartmentPanel.Presentation.Commands.ConfigPanelCommands;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM.ConfigPanelComponentsVM
{
    public class CircuitElementsViewModel : ViewModelBase
    {
        private readonly CircuitElementsCommandCreater _commandCreater;
        public CircuitElementsViewModel() { }
        public CircuitElementsViewModel(IConfigPanelViewModel configPanelVM) 
        {
            _commandCreater = new CircuitElementsCommandCreater(configPanelVM);
            CircuitElements = new ObservableCollection<IApartmentElement>();
            SelectedCircuitElements = new ObservableCollection<IApartmentElement>();
            SelectCircuitElementCommand = _commandCreater.CreateSelectCircuitElementCommand();
        }

        private ObservableCollection<IApartmentElement> _circuitElements;
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
        }

        [JsonIgnore]
        public ICommand SelectCircuitElementCommand { get; }
    }
}

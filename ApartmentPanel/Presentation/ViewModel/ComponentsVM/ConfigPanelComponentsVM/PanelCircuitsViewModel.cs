using ApartmentPanel.Core.Models;
using ApartmentPanel.Presentation.Commands.ConfigPanelCommands;
using ApartmentPanel.Presentation.Services.ValidationServices;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using MediatR;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM.ConfigPanelComponentsVM
{
    public class PanelCircuitsViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly PanelCircuitsCommandCreater _commandCreater;
        private Func<object, bool> _canExecuteCommand;

        private IDataValidationStrategy _validationStrategy;

        public PanelCircuitsViewModel() { }

        public PanelCircuitsViewModel(IConfigPanelViewModel configPanelVM, IMediator mediator) 
        {
            _commandCreater = new PanelCircuitsCommandCreater(configPanelVM, mediator);
            PanelCircuits = new ObservableCollection<Circuit>();
            SelectedPanelCircuits = new ObservableCollection<Circuit>();
            AddPanelCircuitCommand = _commandCreater.CreateAddPanelCircuitCommand(_canExecuteCommand);
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
            set
            {
                _validationStrategy = new CircuitNumberValidation(value, PanelCircuits);
                Set(ref _newCircuit, value);
            }
        }

        [JsonIgnore]
        public ICommand AddPanelCircuitCommand { get; }
        [JsonIgnore]
        public ICommand RemovePanelCircuitsCommand { get; }
        [JsonIgnore]
        public ICommand SelectPanelCircuitCommand { get; }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string result = _validationStrategy?.Validate();
                bool canExecute = string.IsNullOrEmpty(result);
                //_canExecuteCommand = (object p) => canExecute;
                _canExecuteCommand = (object p) => false;
                /*if (columnName == nameof(NewCircuit))
                {
                    if (!string.IsNullOrEmpty(NewCircuit) 
                        && PanelCircuits.ToList().Exists(c => c.Number == NewCircuit))
                    {
                        result = $"The circuit '{NewCircuit}' is already existed";
                    }
                }*/
                return result;
            }
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Presentation.Services;
using ApartmentPanel.Utility.Comparers;

namespace ApartmentPanel.Presentation.Commands.ConfigPanelCommands
{
    public class PanelCircuitsCommandCreater : BaseCommandsCreater
    {
        private readonly IConfigPanelViewModel _configPanelVM;
        private readonly CircuitService _circuitService;

        /*public PanelCircuitsCommandCreater(IConfigPanelViewModel configPanelVM,
            IElementService elementService) : base(elementService)*/
        public PanelCircuitsCommandCreater(IConfigPanelViewModel configPanelVM)
        {
            _configPanelVM = configPanelVM;
            _circuitService = new CircuitService(_configPanelVM);
        }

        public ICommand CreateAddPanelCircuitCommand() => new RelayCommand(o =>
        {
            var newCircuitNumber = _configPanelVM.PanelCircuitsVM.NewCircuit;
            if (!string.IsNullOrEmpty(newCircuitNumber)
              && !_configPanelVM.PanelCircuitsVM.PanelCircuits
                  .ToList()
                  .Exists(c => c.Number == newCircuitNumber))
            {
                Circuit newCircuit = await _mediator.Send(new CreateCircuitRequest{ CircuitNumber = newCircuitNumber});
                var panelCircuits = _configPanelVM.PanelCircuitsVM.PanelCircuits;
                /*var newCircuit = new Circuit
                {
                    Number = _configPanelVM.PanelCircuitsVM.NewCircuit,
                    Elements = new ObservableCollection<IApartmentElement>()
                };*/
                panelCircuits.Add(newCircuit);
                var sortedPanelCircuits 
                    = panelCircuits.OrderBy(c => c.Number, new StringNumberComparer()).ToList();
                _configPanelVM.PanelCircuitsVM.PanelCircuits
                    = new ObservableCollection<Circuit> (sortedPanelCircuits);

                if (!_configPanelVM.IsCancelEnabled)
                    _configPanelVM.IsCancelEnabled = true;
            }

            _configPanelVM.PanelCircuitsVM.NewCircuit = string.Empty;
        });

        public ICommand CreateRemovePanelCircuitsCommand() => new RelayCommand(o =>
        {
            _configPanelVM.CircuitElementsVM.CircuitElements.Clear();
            _configPanelVM.CircuitElementsVM.SelectedCircuitElements.Clear();
            foreach (var circuit in _configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.ToArray())
            {
                bool isDeleted = await _mediator.Send(new DeleteCircuitRequest { CircuitNumber=circuit.Number });
                if (isDeleted)
                    _configPanelVM.PanelCircuitsVM.PanelCircuits.Remove(circuit);
            }

            _configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.Clear();

            if (!_configPanelVM.IsCancelEnabled)
                _configPanelVM.IsCancelEnabled = true;
        });

        public ICommand CreateSelectPanelCircuitCommand() => new RelayCommand(o =>
        {
            _configPanelVM.CircuitElementsVM.SelectedCircuitElements.Clear();
            var currentCircuits = (o as IList<object>)
                ?.OfType<Circuit>();
            if (currentCircuits.Count() != 0)
            {
                _configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.Clear();

                foreach (var currentCircuit in currentCircuits)
                {
                    _configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.Add(currentCircuit);
                    if (currentCircuits.Count() == 1)
                        _circuitService.AddCurrentCircuitElements(currentCircuit.Elements);
                    else _configPanelVM.CircuitElementsVM.CircuitElements.Clear();
                }
            }
        });
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Models.RevitMockupModels;
using WpfPanel.Domain.Services.AnnotationService;
using WpfPanel.ViewModel;
using WpfPanel.ViewModel.ComponentsVM;

namespace WpfPanel.Domain.Services.Commands
{
    public class CommandCreater
    {
        private readonly EditPanelVM _editPanelVM;
        private readonly Action<FamilySymbol> _addElementToApartment;

        public CommandCreater(EditPanelVM editPanelVM) => _editPanelVM = editPanelVM;

        public ICommand CreateAddApartmentElementCommand() => new RelayCommand(o
                => MakeRequest(RequestId.AddElement, _addElementToApartment));

        public ICommand CreateRemoveApartmentElementsCommand() => new RelayCommand(o =>
            {
                if (_editPanelVM.SelectedApartmentElements.Count != 0)
                    foreach (var element in _editPanelVM.SelectedApartmentElements.ToArray())
                        _editPanelVM.ApartmentElements.Remove(element);
            });

        public ICommand CreateAddPanelCircuitCommand() => new RelayCommand(o =>
            {
                if (!string.IsNullOrEmpty(_editPanelVM.NewCircuit)
                    && !_editPanelVM.PanelCircuits.ContainsKey(_editPanelVM.NewCircuit))
                {
                    _editPanelVM.PanelCircuits.Add(_editPanelVM.NewCircuit,
                        new ObservableCollection<ApartmentElement>());
                }

                _editPanelVM.NewCircuit = string.Empty;
            });

        public ICommand CreateRemovePanelCircuitsCommand() => new RelayCommand(o =>
            {
                _editPanelVM.CircuitElements.Clear();
                _editPanelVM.SelectedCircuitElements.Clear();
                foreach (var circuit in _editPanelVM.SelectedPanelCircuits.ToArray())
                    _editPanelVM.PanelCircuits.Remove(circuit.Key);

                _editPanelVM.SelectedPanelCircuits.Clear();
            });

        public ICommand CreateAddElementToCircuitCommand() => new RelayCommand(o =>
            {
                if (_editPanelVM.SelectedPanelCircuits.Count > 1
                    || _editPanelVM.SelectedPanelCircuits.Count == 0
                    || _editPanelVM.SelectedApartmentElements.Count == 0)
                    return;

                var selectedPanelCircuit = _editPanelVM.SelectedPanelCircuits.SingleOrDefault();

                foreach (var selectedApartmentElement in _editPanelVM.SelectedApartmentElements)
                {
                    if (selectedApartmentElement == null
                    || string.IsNullOrEmpty(selectedPanelCircuit.Key))
                        return;

                    var IsElementExist = _editPanelVM.PanelCircuits
                    .Where(e => e.Key == selectedPanelCircuit.Key)
                    .First()
                    .Value
                    .Select(ae => ae.Name)
                    .Contains(selectedApartmentElement.Name);

                    if (!IsElementExist)
                        _editPanelVM.PanelCircuits[selectedPanelCircuit.Key].Add(selectedApartmentElement);

                    AddCurrentCircuitElements(_editPanelVM.PanelCircuits[selectedPanelCircuit.Key]);
                }
            });

        public ICommand CreateRemoveElementsFromCircuitCommand() => new RelayCommand(o => 
        { 
            if (_editPanelVM.SelectedPanelCircuits.Count > 1 || _editPanelVM.SelectedPanelCircuits.Count == 0 
            || _editPanelVM.SelectedCircuitElements.Count == 0) return; 

            var selectedPanelCircuit = _editPanelVM.SelectedPanelCircuits.SingleOrDefault(); 
            if (string.IsNullOrEmpty(selectedPanelCircuit.Key)) return; 

            foreach (var selectedCircuitElement in _editPanelVM.SelectedCircuitElements) 
                selectedPanelCircuit.Value.Remove(selectedCircuitElement); 
            
            AddCurrentCircuitElements(selectedPanelCircuit.Value); 
        });

        public ICommand CreateSelectedApartmentElementsCommand() => new RelayCommand(o =>
            {
                var apartmentElements = (o as IList<object>)?.OfType<ApartmentElement>();
                if (_editPanelVM.SelectedApartmentElements.Count != 0)
                    _editPanelVM.SelectedApartmentElements.Clear();

                if (apartmentElements.Count() != 0)
                    foreach (var apartmentElement in apartmentElements)
                        _editPanelVM.SelectedApartmentElements.Add(apartmentElement);
            });

        public ICommand CreateSelectedPanelCircuitCommand() => new RelayCommand(o =>
            {
                _editPanelVM.SelectedCircuitElements.Clear();
                var currentCircuitElements = (o as IList<object>)
                    ?.OfType<KeyValuePair<string, ObservableCollection<ApartmentElement>>>();
                if (currentCircuitElements.Count() != 0)
                {
                    _editPanelVM.SelectedPanelCircuits.Clear();

                    foreach (var currentCircuitElement in currentCircuitElements)
                    {
                        _editPanelVM.SelectedPanelCircuits.Add(currentCircuitElement);
                        if (currentCircuitElements.Count() == 1)
                            AddCurrentCircuitElements(currentCircuitElement.Value);
                        else _editPanelVM.CircuitElements.Clear();
                    }
                }
            });

        public ICommand CreateSelectedCircuitElementCommand() => new RelayCommand(o =>
            {
                var circuitElements = (o as IList<object>)?.OfType<ApartmentElement>();
                if (_editPanelVM.SelectedCircuitElements.Count != 0)
                    _editPanelVM.SelectedCircuitElements.Clear();

                if (circuitElements.Count() != 0)
                    foreach (var circuitElement in circuitElements)
                        _editPanelVM.SelectedCircuitElements.Add(circuitElement);
            });

        public ICommand CreateOkCommand() => new RelayCommand(o =>
            {
                var close = (Action)o;
                _editPanelVM.OkApplyCancelActions(_editPanelVM.PanelCircuits, OkApplyCancel.Ok);
                close();
            });

        public ICommand CreateApplyCommand() => new RelayCommand(o =>
                _editPanelVM.OkApplyCancelActions(_editPanelVM.PanelCircuits, OkApplyCancel.Apply));

        public ICommand CreateSetAnnotationToElementCommand() => new RelayCommand(o =>
            {
                if (_editPanelVM.SelectedApartmentElements.Count == 1)
                {
                    ApartmentElement apartmentElement = _editPanelVM.SelectedApartmentElements.FirstOrDefault();
                    var annotationService = new AnnotationService.AnnotationService(
                        new FileAnnotationCommunicatorFactory(apartmentElement.Name));

                    apartmentElement.Annotation = annotationService.Save(_editPanelVM.AnnotationPreview);
                }
            });

        public ICommand CreateSetAnnotationPreviewCommand() => new RelayCommand(o =>
            {
                var bitmapSource = o as BitmapSource;
                _editPanelVM.AnnotationPreview = bitmapSource;
            });

        private void AddCurrentCircuitElements(ObservableCollection<ApartmentElement> currentCircuitElements)
        {
            if (_editPanelVM.CircuitElements.Count != 0)
                _editPanelVM.CircuitElements.Clear();

            foreach (var item in currentCircuitElements)
                _editPanelVM.CircuitElements.Add(item);
        }

        private void MakeRequest(RequestId request, object props = null)
        {
            _editPanelVM.Handler.Props = props;
            _editPanelVM.Handler.Request.Make(request);
            _editPanelVM.ExEvent.Raise();
        }

    }
}

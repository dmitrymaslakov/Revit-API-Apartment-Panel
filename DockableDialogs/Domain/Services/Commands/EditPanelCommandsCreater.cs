using Autodesk.Revit.DB;
using DockableDialogs.Domain.Models;
using DockableDialogs.Domain.Services.AnnotationService;
using DockableDialogs.ViewModel;
using DockableDialogs.ViewModel.ComponentsVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DockableDialogs.Domain.Services.Commands
{
    public class EditPanelCommandsCreater
    {
        private readonly IEditPanelToCommandsCreater _editPanelProperties;
        private readonly Action<FamilySymbol> _addElementToApartment;

        public EditPanelCommandsCreater(IEditPanelToCommandsCreater editPanelProperties) => _editPanelProperties = editPanelProperties;

        public ICommand CreateAddApartmentElementCommand() => new RelayCommand(o
                => MakeRequest(RequestId.AddElement, _addElementToApartment));

        public ICommand CreateRemoveApartmentElementsCommand() => new RelayCommand(o =>
        {
            if (_editPanelProperties.SelectedApartmentElements.Count != 0)
                foreach (var element in _editPanelProperties.SelectedApartmentElements.ToArray())
                    _editPanelProperties.ApartmentElements.Remove(element);
        });

        public ICommand CreateAddPanelCircuitCommand() => new RelayCommand(o =>
        {
            if (!string.IsNullOrEmpty(_editPanelProperties.NewCircuit)
                && !_editPanelProperties.PanelCircuits.ContainsKey(_editPanelProperties.NewCircuit))
            {
                _editPanelProperties.PanelCircuits.Add(_editPanelProperties.NewCircuit,
                    new ObservableCollection<ApartmentElement>());
            }

            _editPanelProperties.NewCircuit = string.Empty;
        });

        public ICommand CreateRemovePanelCircuitsCommand() => new RelayCommand(o =>
        {
            _editPanelProperties.CircuitElements.Clear();
            _editPanelProperties.SelectedCircuitElements.Clear();
            foreach (var circuit in _editPanelProperties.SelectedPanelCircuits.ToArray())
                _editPanelProperties.PanelCircuits.Remove(circuit.Key);

            _editPanelProperties.SelectedPanelCircuits.Clear();
        });

        public ICommand CreateAddElementToCircuitCommand() => new RelayCommand(o =>
        {
            if (_editPanelProperties.SelectedPanelCircuits.Count > 1
                || _editPanelProperties.SelectedPanelCircuits.Count == 0
                || _editPanelProperties.SelectedApartmentElements.Count == 0)
                return;

            var selectedPanelCircuit = _editPanelProperties.SelectedPanelCircuits.SingleOrDefault();

            foreach (var selectedApartmentElement in _editPanelProperties.SelectedApartmentElements)
            {
                if (selectedApartmentElement == null
                || string.IsNullOrEmpty(selectedPanelCircuit.Key))
                    return;

                var IsElementExist = _editPanelProperties.PanelCircuits
                .Where(e => e.Key == selectedPanelCircuit.Key)
                .First()
                .Value
                .Select(ae => ae.Name)
                .Contains(selectedApartmentElement.Name);

                if (!IsElementExist)
                    _editPanelProperties.PanelCircuits[selectedPanelCircuit.Key].Add(selectedApartmentElement);

                AddCurrentCircuitElements(_editPanelProperties.PanelCircuits[selectedPanelCircuit.Key]);
            }
        });

        public ICommand CreateRemoveElementsFromCircuitCommand() => new RelayCommand(o =>
        {
            if (_editPanelProperties.SelectedPanelCircuits.Count > 1 || _editPanelProperties.SelectedPanelCircuits.Count == 0
            || _editPanelProperties.SelectedCircuitElements.Count == 0) return;

            var selectedPanelCircuit = _editPanelProperties.SelectedPanelCircuits.SingleOrDefault();
            if (string.IsNullOrEmpty(selectedPanelCircuit.Key)) return;

            foreach (var selectedCircuitElement in _editPanelProperties.SelectedCircuitElements)
                selectedPanelCircuit.Value.Remove(selectedCircuitElement);

            AddCurrentCircuitElements(selectedPanelCircuit.Value);
        });

        public ICommand CreateSelectedApartmentElementsCommand() => new RelayCommand(o =>
        {
            var apartmentElements = (o as IList<object>)?.OfType<ApartmentElement>();
            if (_editPanelProperties.SelectedApartmentElements.Count != 0)
                _editPanelProperties.SelectedApartmentElements.Clear();

            if (apartmentElements.Count() != 0)
                foreach (var apartmentElement in apartmentElements)
                    _editPanelProperties.SelectedApartmentElements.Add(apartmentElement);
        });

        public ICommand CreateSelectedPanelCircuitCommand() => new RelayCommand(o =>
        {
            _editPanelProperties.SelectedCircuitElements.Clear();
            var currentCircuitElements = (o as IList<object>)
                ?.OfType<KeyValuePair<string, ObservableCollection<ApartmentElement>>>();
            if (currentCircuitElements.Count() != 0)
            {
                _editPanelProperties.SelectedPanelCircuits.Clear();

                foreach (var currentCircuitElement in currentCircuitElements)
                {
                    _editPanelProperties.SelectedPanelCircuits.Add(currentCircuitElement);
                    if (currentCircuitElements.Count() == 1)
                        AddCurrentCircuitElements(currentCircuitElement.Value);
                    else _editPanelProperties.CircuitElements.Clear();
                }
            }
        });

        public ICommand CreateSelectedCircuitElementCommand() => new RelayCommand(o =>
        {
            var circuitElements = (o as IList<object>)?.OfType<ApartmentElement>();
            if (_editPanelProperties.SelectedCircuitElements.Count != 0)
                _editPanelProperties.SelectedCircuitElements.Clear();

            if (circuitElements.Count() != 0)
                foreach (var circuitElement in circuitElements)
                    _editPanelProperties.SelectedCircuitElements.Add(circuitElement);
        });

        public ICommand CreateOkCommand() => new RelayCommand(o =>
        {
            var close = (Action)o;
            _editPanelProperties.OkApplyCancelActions(_editPanelProperties.PanelCircuits, OkApplyCancel.Ok);
            close();
        });

        public ICommand CreateApplyCommand() => new RelayCommand(o =>
                _editPanelProperties.OkApplyCancelActions(_editPanelProperties.PanelCircuits, OkApplyCancel.Apply));

        public ICommand CreateSetAnnotationToElementCommand() => new RelayCommand(o =>
        {
            if (_editPanelProperties.SelectedApartmentElements.Count == 1)
            {
                ApartmentElement apartmentElement = _editPanelProperties.SelectedApartmentElements.FirstOrDefault();
                var annotationService = new AnnotationService.AnnotationService(
                    new FileAnnotationCommunicatorFactory(apartmentElement.Name));

                apartmentElement.Annotation = annotationService.Save(_editPanelProperties.AnnotationPreview);
            }
        });

        public ICommand CreateSetAnnotationPreviewCommand() => new RelayCommand(o =>
        {
            var bitmapSource = o as BitmapSource;
            _editPanelProperties.AnnotationPreview = bitmapSource;
        });

        private void AddCurrentCircuitElements(ObservableCollection<ApartmentElement> currentCircuitElements)
        {
            if (_editPanelProperties.CircuitElements.Count != 0)
                _editPanelProperties.CircuitElements.Clear();

            foreach (var item in currentCircuitElements)
                _editPanelProperties.CircuitElements.Add(item);
        }

        private void MakeRequest(RequestId request, object props = null)
        {
            _editPanelProperties.Handler.Props = props;
            _editPanelProperties.Handler.Request.Make(request);
            _editPanelProperties.ExEvent.Raise();
        }
    }
}

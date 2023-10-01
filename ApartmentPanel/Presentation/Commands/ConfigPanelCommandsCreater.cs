using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Services.AnnotationService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ApartmentPanel.Presentation.ViewModel;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Infrastructure;
using ApartmentPanel.Core.Services.Commands;

namespace ApartmentPanel.Presentation.Commands
{
    public class ConfigPanelCommandsCreater
    {
        private readonly IConfigPanelForCommandsCreater _editPanelProperties;
        private readonly Action<ApartmentElement> _addElementToApartment;

        public ConfigPanelCommandsCreater(IConfigPanelForCommandsCreater editPanelProperties)
        {
            _editPanelProperties = editPanelProperties;
            _addElementToApartment = newElement =>
            {
                if (!_editPanelProperties.ApartmentElements.Select(ae => ae.Name).Contains(newElement.Name))
                {
                    var annotationService = new AnnotationService.AnnotationService(
                        new FileAnnotationCommunicatorFactory(newElement.Name));

                    ImageSource annotation = annotationService.IsAnnotationExists()
                        ? annotationService.Get() : null;

                    ApartmentElement newApartmentElement = newElement.Clone();
                    newApartmentElement.Annotation = annotation;
                    _editPanelProperties.ApartmentElements.Add(newApartmentElement);
                }
            };
        }

        public ICommand CreateAddApartmentElementCommand() => new RelayCommand(o =>
        {
            MakeRequest(RequestId.AddElement, _addElementToApartment);
            if (!_editPanelProperties.IsCancelEnabled)
                _editPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateRemoveApartmentElementsCommand() => new RelayCommand(o =>
        {
            if (_editPanelProperties.SelectedApartmentElements.Count != 0)
            {
                foreach (var element in _editPanelProperties.SelectedApartmentElements.ToArray())
                    _editPanelProperties.ApartmentElements.Remove(element);

                if (!_editPanelProperties.IsCancelEnabled)
                    _editPanelProperties.IsCancelEnabled = true;
            }
        });

        public ICommand CreateAddPanelCircuitCommand() => new RelayCommand(o =>
        {
            if (!string.IsNullOrEmpty(_editPanelProperties.NewCircuit)
                && !_editPanelProperties.PanelCircuits.ContainsKey(_editPanelProperties.NewCircuit))
            {
                _editPanelProperties.PanelCircuits.Add(_editPanelProperties.NewCircuit,
                    new ObservableCollection<ApartmentElement>());

                if (!_editPanelProperties.IsCancelEnabled)
                    _editPanelProperties.IsCancelEnabled = true;
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

            if (!_editPanelProperties.IsCancelEnabled)
                _editPanelProperties.IsCancelEnabled = true;
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

                if (!_editPanelProperties.IsCancelEnabled)
                    _editPanelProperties.IsCancelEnabled = true;
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
            if (!_editPanelProperties.IsCancelEnabled)
                _editPanelProperties.IsCancelEnabled = true;
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

            if (_editPanelProperties.IsCancelEnabled)
                _editPanelProperties.IsCancelEnabled = false;

            close();
        });

        public ICommand CreateApplyCommand() => new RelayCommand(o =>
        {
            _editPanelProperties.OkApplyCancelActions(_editPanelProperties.PanelCircuits, OkApplyCancel.Apply);

            if (_editPanelProperties.IsCancelEnabled)
                _editPanelProperties.IsCancelEnabled = false;
        });

        public ICommand CreateCancelCommand() => new RelayCommand(o =>
        {
            _editPanelProperties.OkApplyCancelActions(_editPanelProperties.PanelCircuits, OkApplyCancel.Cancel);
            if (_editPanelProperties.IsCancelEnabled)
                _editPanelProperties.IsCancelEnabled = false;
        });

        public ICommand CreateSetAnnotationToElementCommand() => new RelayCommand(o =>
        {
            if (_editPanelProperties.SelectedApartmentElements.Count == 1)
            {
                ApartmentElement apartmentElement =
                    _editPanelProperties.SelectedApartmentElements.FirstOrDefault();

                var annotationService = new AnnotationService.AnnotationService(
                    new FileAnnotationCommunicatorFactory(apartmentElement.Name));

                apartmentElement.Annotation =
                    annotationService.Save(_editPanelProperties.AnnotationPreview);
            }
        });

        public ICommand CreateSetAnnotationPreviewCommand() => new RelayCommand(o =>
        {
            var bitmapSource = o as BitmapSource;
            _editPanelProperties.AnnotationPreview = bitmapSource;
        });

        public ICommand CreateLoadLatestConfigCommand() => new RelayCommand(o =>
        {
            if (File.Exists(_editPanelProperties.LatestConfigPath))
            {
                string json = File.ReadAllText(_editPanelProperties.LatestConfigPath);
                ConfigPanelVM deso = JsonSerializer.Deserialize<ConfigPanelVM>(json);
                _editPanelProperties.ApplyLatestConfiguration(deso);
            }
            else
            {
                _editPanelProperties.ApplyLatestConfiguration(
                    new ConfigPanelVM(_editPanelProperties.ExEvent, _editPanelProperties.Handler));
            }
        });

        public ICommand CreateSaveLatestConfigCommand() => new RelayCommand(o =>
        {
            var editPanel = o as ConfigPanelVM;
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(editPanel, options);
            File.WriteAllText(_editPanelProperties.LatestConfigPath, json);
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

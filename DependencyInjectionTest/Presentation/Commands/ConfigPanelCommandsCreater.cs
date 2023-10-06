using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DependencyInjectionTest.Core.Services.AnnotationService;
using DependencyInjectionTest.Presentation.ViewModel;
using DependencyInjectionTest.Presentation.ViewModel.ComponentsVM;
using DependencyInjectionTest.Core.Models;
using DependencyInjectionTest.Presentation.ViewModel.Interfaces;
using DependencyInjectionTest.Core.Services.Interfaces;
using DependencyInjectionTest.Core.Models.Interfaces;
using Autodesk.Revit.DB;
using DependencyInjectionTest.Core.Services;

namespace DependencyInjectionTest.Presentation.Commands
{
    public class ConfigPanelCommandsCreater : BaseCommandsCreater
    {
        //private readonly IConfigPanelPropsForCommandsCreater _configPanelProperties;
        private readonly IConfigPanelViewModel _configPanelProperties;
        private readonly Action<IApartmentElement> _addElementToApartment;

        //public ConfigPanelCommandsCreater(IConfigPanelPropsForCommandsCreater editPanelProperties)
        public ConfigPanelCommandsCreater(IConfigPanelViewModel configPanelProperties,
            IApartmentElementService apartmentElementService,
            IApartmentPanelService apartmentPanelService) : base(apartmentElementService, apartmentPanelService)
        {
            _configPanelProperties = configPanelProperties;
            _addElementToApartment = newElement =>
            {
                if (!_configPanelProperties.ApartmentElements.Select(ae => ae.Name).Contains(newElement.Name))
                {
                    var annotationService = new AnnotationService(
                        new FileAnnotationCommunicatorFactory(newElement.Name));

                    ImageSource annotation = annotationService.IsAnnotationExists()
                        ? annotationService.Get() : null;

                    IApartmentElement newApartmentElement = newElement.Clone();
                    newApartmentElement.Annotation = annotation;
                    _configPanelProperties.ApartmentElements.Add(newApartmentElement);
                }
            };
        }

        public ICommand CreateAddApartmentElementCommand() => new RelayCommand(o =>
        {
            _apartmentElementService.AddToApartment(_addElementToApartment);
            /*MakeRequest(RequestId.AddElement, _addElementToApartment);
            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;*/
        });

        public ICommand CreateRemoveApartmentElementsCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedApartmentElements.Count != 0)
            {
                _apartmentElementService.RemoveFromApartment();

                /*foreach (var element in _configPanelProperties.SelectedApartmentElements.ToArray())
                    _configPanelProperties.ApartmentElements.Remove(element);*/

                if (!_configPanelProperties.IsCancelEnabled)
                    _configPanelProperties.IsCancelEnabled = true;
            }
        });

        public ICommand CreateAddPanelCircuitCommand() => new RelayCommand(o =>
        {
            if (!string.IsNullOrEmpty(_configPanelProperties.NewCircuit)
                && !_configPanelProperties.PanelCircuits.ContainsKey(_configPanelProperties.NewCircuit))
            {
                _apartmentPanelService.AddCircuit();
                /*_configPanelProperties.PanelCircuits.Add(_configPanelProperties.NewCircuit,
                    new ObservableCollection<IApartmentElement>());*/

                if (!_configPanelProperties.IsCancelEnabled)
                    _configPanelProperties.IsCancelEnabled = true;
            }

            _configPanelProperties.NewCircuit = string.Empty;
        });

        public ICommand CreateRemovePanelCircuitsCommand() => new RelayCommand(o =>
        {
            _configPanelProperties.CircuitElements.Clear();
            _configPanelProperties.SelectedCircuitElements.Clear();
            _apartmentPanelService.RemoveCircuits();
            /*foreach (var circuit in _configPanelProperties.SelectedPanelCircuits.ToArray())
                _configPanelProperties.PanelCircuits.Remove(circuit.Key);*/

            _configPanelProperties.SelectedPanelCircuits.Clear();

            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateAddElementToCircuitCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedPanelCircuits.Count > 1
                || _configPanelProperties.SelectedPanelCircuits.Count == 0
                || _configPanelProperties.SelectedApartmentElements.Count == 0)
                return;

            var selectedPanelCircuit = _configPanelProperties.SelectedPanelCircuits.SingleOrDefault();

            foreach (var selectedApartmentElement in _configPanelProperties.SelectedApartmentElements)
            {
                if (selectedApartmentElement == null
                || string.IsNullOrEmpty(selectedPanelCircuit.Key))
                    return;

                var IsElementExist = _configPanelProperties.PanelCircuits
                .Where(e => e.Key == selectedPanelCircuit.Key)
                .First()
                .Value
                .Select(ae => ae.Name)
                .Contains(selectedApartmentElement.Name);

                if (!IsElementExist)
                    _apartmentElementService.AddToCircuit(selectedApartmentElement);
                    //_configPanelProperties.PanelCircuits[selectedPanelCircuit.Key].Add(selectedApartmentElement);

                    //Проверить метод AddCurrentCircuitElements. Возможно его нужно переместить в 
                    //
                AddCurrentCircuitElements(_configPanelProperties.PanelCircuits[selectedPanelCircuit.Key]);

                if (!_configPanelProperties.IsCancelEnabled)
                    _configPanelProperties.IsCancelEnabled = true;
            }
        });

        public ICommand CreateRemoveElementsFromCircuitCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedPanelCircuits.Count > 1 || _configPanelProperties.SelectedPanelCircuits.Count == 0
            || _configPanelProperties.SelectedCircuitElements.Count == 0) return;

            var selectedPanelCircuit = _configPanelProperties.SelectedPanelCircuits.SingleOrDefault();
            if (string.IsNullOrEmpty(selectedPanelCircuit.Key)) return;

            foreach (var selectedCircuitElement in _configPanelProperties.SelectedCircuitElements)
                selectedPanelCircuit.Value.Remove(selectedCircuitElement);

            AddCurrentCircuitElements(selectedPanelCircuit.Value);
            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateSelectedApartmentElementsCommand() => new RelayCommand(o =>
        {
            var apartmentElements = (o as IList<object>)?.OfType<ApartmentElement>();
            if (_configPanelProperties.SelectedApartmentElements.Count != 0)
                _configPanelProperties.SelectedApartmentElements.Clear();

            if (apartmentElements.Count() != 0)
                foreach (var apartmentElement in apartmentElements)
                    _configPanelProperties.SelectedApartmentElements.Add(apartmentElement);
        });

        public ICommand CreateSelectedPanelCircuitCommand() => new RelayCommand(o =>
        {
            _configPanelProperties.SelectedCircuitElements.Clear();
            var currentCircuitElements = (o as IList<object>)
                ?.OfType<KeyValuePair<string, ObservableCollection<ApartmentElement>>>();
            if (currentCircuitElements.Count() != 0)
            {
                _configPanelProperties.SelectedPanelCircuits.Clear();

                foreach (var currentCircuitElement in currentCircuitElements)
                {
                    _configPanelProperties.SelectedPanelCircuits.Add(currentCircuitElement);
                    if (currentCircuitElements.Count() == 1)
                        AddCurrentCircuitElements(currentCircuitElement.Value);
                    else _configPanelProperties.CircuitElements.Clear();
                }
            }
        });

        public ICommand CreateSelectedCircuitElementCommand() => new RelayCommand(o =>
        {
            var circuitElements = (o as IList<object>)?.OfType<ApartmentElement>();
            if (_configPanelProperties.SelectedCircuitElements.Count != 0)
                _configPanelProperties.SelectedCircuitElements.Clear();

            if (circuitElements.Count() != 0)
                foreach (var circuitElement in circuitElements)
                    _configPanelProperties.SelectedCircuitElements.Add(circuitElement);
        });

        public ICommand CreateOkCommand() => new RelayCommand(o =>
        {
            var close = (Action)o;
            _configPanelProperties.OkApplyCancelActions(_configPanelProperties.PanelCircuits, OkApplyCancel.Ok);

            if (_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = false;

            close();
        });

        public ICommand CreateApplyCommand() => new RelayCommand(o =>
        {
            _configPanelProperties.OkApplyCancelActions(_configPanelProperties.PanelCircuits, OkApplyCancel.Apply);

            if (_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = false;
        });

        public ICommand CreateCancelCommand() => new RelayCommand(o =>
        {
            _configPanelProperties.OkApplyCancelActions(_configPanelProperties.PanelCircuits, OkApplyCancel.Cancel);
            if (_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = false;
        });

        public ICommand CreateSetAnnotationToElementCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedApartmentElements.Count == 1)
            {
                ApartmentElement apartmentElement =
                    _configPanelProperties.SelectedApartmentElements.FirstOrDefault();

                var annotationService = new AnnotationService(
                    new FileAnnotationCommunicatorFactory(apartmentElement.Name));

                apartmentElement.Annotation =
                    annotationService.Save(_configPanelProperties.AnnotationPreview);
            }
        });

        public ICommand CreateSetAnnotationPreviewCommand() => new RelayCommand(o =>
        {
            var bitmapSource = o as BitmapSource;
            _configPanelProperties.AnnotationPreview = bitmapSource;
        });

        public ICommand CreateLoadLatestConfigCommand() => new RelayCommand(o =>
        {
            if (File.Exists(_configPanelProperties.LatestConfigPath))
            {
                string json = File.ReadAllText(_configPanelProperties.LatestConfigPath);
                ConfigPanelViewModel deso = JsonSerializer.Deserialize<ConfigPanelViewModel>(json);
                _configPanelProperties.ApplyLatestConfiguration(deso);
            }
            else
            {
                _configPanelProperties.ApplyLatestConfiguration(
                    new ConfigPanelViewModel());
                    //new ConfigPanelVM(_editPanelProperties.ExEvent, _editPanelProperties.Handler));
    }
        });

        public ICommand CreateSaveLatestConfigCommand() => new RelayCommand(o =>
        {
            var editPanel = o as ConfigPanelViewModel;
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(editPanel, options);
            File.WriteAllText(_configPanelProperties.LatestConfigPath, json);
        });

        private void AddCurrentCircuitElements(ObservableCollection<IApartmentElement> currentCircuitElements)
        {
            if (_configPanelProperties.CircuitElements.Count != 0)
                _configPanelProperties.CircuitElements.Clear();

            foreach (var item in currentCircuitElements)
                _configPanelProperties.CircuitElements.Add(item);
        }

        /*private void MakeRequest(RequestId request, object props = null)
        {
            _configPanelProperties.Handler.Props = props;
            _configPanelProperties.Handler.Request.Make(request);
            _configPanelProperties.ExEvent.Raise();
        }*/
    }
}

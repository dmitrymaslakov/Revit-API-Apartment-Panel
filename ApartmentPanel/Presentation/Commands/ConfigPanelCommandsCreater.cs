﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ApartmentPanel.Core.Services.AnnotationService;
using ApartmentPanel.Presentation.ViewModel;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.Services;

namespace ApartmentPanel.Presentation.Commands
{
    public class ConfigPanelCommandsCreater : BaseCommandsCreater
    {
        private readonly IConfigPanelViewModel _configPanelProperties;
        private readonly Action<IApartmentElement> _addElementToApartment;
        private readonly CircuitService _circuitService;

        public ConfigPanelCommandsCreater(IConfigPanelViewModel configPanelProps,
            IElementService elementService) : base(elementService)
        {
            _configPanelProperties = configPanelProps;
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
            _circuitService = new CircuitService(_configPanelProperties);
        }

        public ICommand CreateAddElementToApartmentCommand() => new RelayCommand(o =>
        {
            _elementService.AddToApartment(_addElementToApartment);
        });

        public ICommand CreateRemoveElementsFromApartmentCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedApartmentElements.Count != 0)
            {
                foreach (var element in _configPanelProperties.SelectedApartmentElements.ToArray())
                    _configPanelProperties.ApartmentElements.Remove(element);
                if (!_configPanelProperties.IsCancelEnabled)
                    _configPanelProperties.IsCancelEnabled = true;
            }
        });

        public ICommand CreateAddCircuitToPanelCommand() => new RelayCommand(o =>
        {
            if (!string.IsNullOrEmpty(_configPanelProperties.NewCircuit)
                && !_configPanelProperties.PanelCircuits.ContainsKey(_configPanelProperties.NewCircuit))
            {
                _configPanelProperties.PanelCircuits.Add(_configPanelProperties.NewCircuit,
                    new ObservableCollection<IApartmentElement>());

                if (!_configPanelProperties.IsCancelEnabled)
                    _configPanelProperties.IsCancelEnabled = true;
            }

            _configPanelProperties.NewCircuit = string.Empty;
        });

        public ICommand CreateRemoveCircuitsFromPanelCommand() => new RelayCommand(o =>
        {
            _configPanelProperties.CircuitElements.Clear();
            _configPanelProperties.SelectedCircuitElements.Clear();
            foreach (var circuit in _configPanelProperties.SelectedPanelCircuits.ToArray())
                _configPanelProperties.PanelCircuits.Remove(circuit.Key);

            _configPanelProperties.SelectedPanelCircuits.Clear();

            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateAddElementsToCircuitCommand() => new RelayCommand(o =>
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
                    _configPanelProperties.PanelCircuits[selectedPanelCircuit.Key].Add(selectedApartmentElement);

                _circuitService.AddCurrentCircuitElements(_configPanelProperties.PanelCircuits[selectedPanelCircuit.Key]);
            }
            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateRemoveElementsFromCircuitCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedPanelCircuits.Count > 1 || _configPanelProperties.SelectedPanelCircuits.Count == 0
            || _configPanelProperties.SelectedCircuitElements.Count == 0) return;

            var selectedPanelCircuit = _configPanelProperties.SelectedPanelCircuits.SingleOrDefault();
            if (string.IsNullOrEmpty(selectedPanelCircuit.Key)) return;

            foreach (var selectedCircuitElement in _configPanelProperties.SelectedCircuitElements)
                selectedPanelCircuit.Value.Remove(selectedCircuitElement);

            _circuitService.AddCurrentCircuitElements(selectedPanelCircuit.Value);
            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateCollectSelectedApartmentElementsCommand() => new RelayCommand(o =>
        {
            var selectedElements = (o as IList<object>)?.OfType<IApartmentElement>();
            if (_configPanelProperties.SelectedApartmentElements.Count != 0)
                _configPanelProperties.SelectedApartmentElements.Clear();

            if (selectedElements.Count() != 0)
                foreach (var apartmentElement in selectedElements)
                    _configPanelProperties.SelectedApartmentElements.Add(apartmentElement);
        });

        public ICommand CreateCollectSelectedCircuitsCommand() => new RelayCommand(o =>
        {
            _configPanelProperties.SelectedCircuitElements.Clear();
            var currentCircuits = (o as IList<object>)
                ?.OfType<KeyValuePair<string, ObservableCollection<IApartmentElement>>>();
            if (currentCircuits.Count() != 0)
            {
                _configPanelProperties.SelectedPanelCircuits.Clear();

                foreach (var currentCircuit in currentCircuits)
                {
                    _configPanelProperties.SelectedPanelCircuits.Add(currentCircuit);
                    if (currentCircuits.Count() == 1)
                        _circuitService.AddCurrentCircuitElements(currentCircuit.Value);
                    else _configPanelProperties.CircuitElements.Clear();
                }
            }
        });

        public ICommand CreateCollectSelectedCircuitElementsCommand() => new RelayCommand(o =>
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
                IApartmentElement apartmentElement =
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
                var deso = JsonSerializer.Deserialize<ConfigPanelViewModel>(
                    json, _elementService.GetSerializationOptions());

                _configPanelProperties.ApplyLatestConfiguration(deso);
            }
            /*else
            {
                _configPanelProperties.ApplyLatestConfiguration(
                    new ConfigPanelViewModel());
            }*/
        });

        public ICommand CreateSaveLatestConfigCommand() => new RelayCommand(o =>
        {
            var editPanel = o as ConfigPanelViewModel;
            string json = JsonSerializer.Serialize(editPanel, 
                _elementService.GetSerializationOptions());

            File.WriteAllText(_configPanelProperties.LatestConfigPath, json);
        });
    }
}

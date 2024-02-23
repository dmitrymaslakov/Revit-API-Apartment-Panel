using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ApartmentPanel.Presentation.ViewModel;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.Services;
using ApartmentPanel.Presentation.View.Components;
using ApartmentPanel.Presentation.Models.Batch;
using ApartmentPanel.Utility.AnnotationUtility;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using System.Windows.Media;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using System.Security.Cryptography;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Presentation.Commands
{
    public class ConfigPanelCommandsCreater : BaseCommandsCreater
    {
        private readonly IConfigPanelViewModel _configPanelProperties;
        private readonly Action<List<(string name, string category)>> _showListElements;
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
                    /*_configPanelProperties.SetParametersToElement = null;
                    _configPanelProperties.SetParametersToElement = (List<string> parameterNames) =>
                        {
                            var parameters = parameterNames
                                .Select(pn => new Parameter { Name = pn })
                                .ToList();
                            newElement.Parameters = new ObservableCollection<Parameter>(parameters);
                        };
                    SetParamsDTO setParamsDTO = new SetParamsDTO
                    {
                        ElementName = newElement.Name,
                        SetInstanceParameters = _configPanelProperties.SetParametersToElement
                    };
                    _elementService.SetElementParameters(setParamsDTO);*/

                    IApartmentElement newApartmentElement = elementService.CloneFrom(newElement);
                    _configPanelProperties.ApartmentElements.Add(newApartmentElement);
                }
            };
            _showListElements = props =>
            {
                IListElementsViewModel listElementsVM = _configPanelProperties.ListElementsVM;
                listElementsVM.AddElementToApartment = _addElementToApartment;
                listElementsVM.AllElements =
                    new ObservableCollection<IApartmentElement>(_elementService.GetAll(props));
                new ElementList(listElementsVM).ShowDialog();
            };
            _circuitService = new CircuitService(_configPanelProperties);
        }

        public ICommand CreateShowListElementsCommand() => new RelayCommand(o =>
        {
            _elementService.AddToApartment(_showListElements);
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
              && !_configPanelProperties.PanelCircuits.ToList().Exists(c => c.Number == _configPanelProperties.NewCircuit))
            {
                _configPanelProperties.PanelCircuits.Add(new Circuit
                {
                    Number = _configPanelProperties.NewCircuit,
                    Elements = new ObservableCollection<IApartmentElement>()
                });

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
                _configPanelProperties.PanelCircuits.Remove(circuit);

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
                || string.IsNullOrEmpty(selectedPanelCircuit.Number))
                    return;

                var IsElementExist = _configPanelProperties.PanelCircuits
                .Where(e => e.Number == selectedPanelCircuit.Number)
                .First()
                .Elements
                .Select(ae => ae.Name)
                .Contains(selectedApartmentElement.Name);

                if (!IsElementExist)
                {
                    IApartmentElement elementClone = selectedApartmentElement.Clone();
                    _configPanelProperties.SetParametersToElement = null;
                    _configPanelProperties.SetParametersToElement = (List<string> parameterNames) =>
                        {
                            var parameters = parameterNames
                                .Select(pn => new Parameter { Name = pn })
                                .ToList();
                            elementClone.Parameters = new ObservableCollection<Parameter>(parameters);
                        };
                    SetParamsDTO setParamsDTO = new SetParamsDTO
                    {
                        ElementName = elementClone.Name,
                        SetInstanceParameters = _configPanelProperties.SetParametersToElement
                    };
                    _elementService.SetElementParameters(setParamsDTO);

                    _configPanelProperties.PanelCircuits
                    .First(c => c.Number == selectedPanelCircuit.Number).Elements.Add(elementClone);
                }

                _circuitService.AddCurrentCircuitElements(_configPanelProperties.PanelCircuits.First(c => c.Number == selectedPanelCircuit.Number).Elements);
            }
            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateRemoveElementsFromCircuitCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedPanelCircuits.Count > 1 || _configPanelProperties.SelectedPanelCircuits.Count == 0
            || _configPanelProperties.SelectedCircuitElements.Count == 0) return;

            var selectedPanelCircuit = _configPanelProperties.SelectedPanelCircuits.SingleOrDefault();
            if (string.IsNullOrEmpty(selectedPanelCircuit.Number)) return;

            foreach (var selectedCircuitElement in _configPanelProperties.SelectedCircuitElements)
                selectedPanelCircuit.Elements.Remove(selectedCircuitElement);

            _circuitService.AddCurrentCircuitElements(selectedPanelCircuit.Elements);
            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateSelectApartmentElementsCommand() => new RelayCommand(o =>
        {
            var selectedElements = (o as IList<object>)?.OfType<IApartmentElement>();
            if (_configPanelProperties.SelectedApartmentElements.Count != 0)
                _configPanelProperties.SelectedApartmentElements.Clear();

            if (selectedElements.Count() != 0)
                foreach (var apartmentElement in selectedElements)
                    _configPanelProperties.SelectedApartmentElements.Add(apartmentElement);
        });

        public ICommand CreateSelectPanelCircuitCommand() => new RelayCommand(o =>
        {
            _configPanelProperties.SelectedCircuitElements.Clear();
            var currentCircuits = (o as IList<object>)
                ?.OfType<Circuit>();
            if (currentCircuits.Count() != 0)
            {
                _configPanelProperties.SelectedPanelCircuits.Clear();

                foreach (var currentCircuit in currentCircuits)
                {
                    _configPanelProperties.SelectedPanelCircuits.Add(currentCircuit);
                    if (currentCircuits.Count() == 1)
                        _circuitService.AddCurrentCircuitElements(currentCircuit.Elements);
                    else _configPanelProperties.CircuitElements.Clear();
                }
            }
        });

        public ICommand CreateSelectCircuitElementCommand() => new RelayCommand(o =>
        {
            var circuitElements = (o as IList<object>)?.OfType<ApartmentElement>();
            if (_configPanelProperties.SelectedCircuitElements.Count != 0)
                _configPanelProperties.SelectedCircuitElements.Clear();

            if (circuitElements.Count() != 0)
                foreach (var circuitElement in circuitElements)
                    _configPanelProperties.SelectedCircuitElements.Add(circuitElement);

            /*var element = _configPanelProperties.SelectedCircuitElements.FirstOrDefault();
            if (element == null)
                return;

            _configPanelProperties.SetParametersToElement = OnSetParametersToCircuitElementExecuted;
            SetParamsDTO setParamsDTO = new SetParamsDTO
            {
                ElementName = element.Name,
                SetInstanceParameters = _configPanelProperties.SetParametersToElement
            };
            _elementService.SetElementParameters(setParamsDTO);
            _configPanelProperties.CircuitElementParameters = element.Parameters;*/
        });

        public ICommand CreateOkCommand() => new RelayCommand(o =>
        {
            var close = (Action)o;
            CreateApplyCommand()?.Execute(null);
            close();
        });

        public ICommand CreateApplyCommand() => new RelayCommand(o =>
        {
            ObservableCollection<Circuit> circuits = _configPanelProperties.PanelCircuits;
            ObservableCollection<ElementBatch> batches = _configPanelProperties.Batches;
            ObservableCollection<double> heightsOk = _configPanelProperties.ListHeightsOK;
            ObservableCollection<double> heightsUk = _configPanelProperties.ListHeightsUK;
            ObservableCollection<double> heightsCenter = _configPanelProperties.ListHeightsCenter;

            _configPanelProperties.OkApplyCancelActions(
                (circuits, batches, heightsOk, heightsUk, heightsCenter), OkApplyCancel.Apply);

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

                _elementService.SetAnnotationTo(apartmentElement, _configPanelProperties.AnnotationPreview);
            }
        });

        public ICommand CreateSetAnnotationToElementsBatchCommand() => new RelayCommand(o =>
        {
            var annotationService = new AnnotationService(
                new FileAnnotationCommunicatorFactory(_configPanelProperties.ElementBatch.Name));

            _configPanelProperties.ElementBatch.Annotation =
                annotationService.Save(_configPanelProperties.AnnotationPreview);
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
        });

        public ICommand CreateSaveLatestConfigCommand() => new RelayCommand(o =>
        {
            var configPanel = o as ConfigPanelViewModel;
            string json = JsonSerializer.Serialize(configPanel,
                _elementService.GetSerializationOptions());

            File.WriteAllText(_configPanelProperties.LatestConfigPath, json);
        });

        public ICommand CreateSetNewElementForBatchCommand() => new RelayCommand(o =>
        {
            /*(string circuit, string elementName, string elementCategory, ImageSource annotation) =
                (ValueTuple<string, string, string, ImageSource>)o;*/
            (string circuit, IApartmentElement element) = (ValueTuple<string, IApartmentElement>)o;


            _configPanelProperties.NewElementForBatch = new BatchedElement
            {
                Circuit = circuit,
                Category = element.Category,
                Name = element.Name,
                Annotation = element.Annotation
            };

            _configPanelProperties.SetParametersToElement = OnSetParametersToBatchElementExecuted;
            SetParamsDTO setParamsDTO = new SetParamsDTO
            {
                ElementName = element.Name,
                SetInstanceParameters = _configPanelProperties.SetParametersToElement
            };
            _elementService.SetElementParameters(setParamsDTO);
        });

        public ICommand CreateAddElementToRowCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.NewElementForBatch == null) return;

            var row = (BatchedRow)o;
            row.RowElements.Add(_configPanelProperties.NewElementForBatch);
            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateRemoveElementFromRowCommand() => new RelayCommand(o =>
        {
            var row = (BatchedRow)o;
            row.RowElements.Remove(_configPanelProperties.SelectedBatchedElement);
            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateAddBatchToElementBatchesCommand() => new RelayCommand(o =>
        {
            var clone = _configPanelProperties.ElementBatch.Clone();

            if (_configPanelProperties.Batches.Any(b => string.Equals(b.Name, clone.Name)))
            {
                var replacedEl = _configPanelProperties.Batches
                    .FirstOrDefault(b => string.Equals(b.Name, clone.Name));
                int index = _configPanelProperties.Batches.IndexOf(replacedEl);

                if (index != -1) _configPanelProperties.Batches[index] = clone;
            }
            else
            {
                _configPanelProperties.Batches.Add(clone);
            }
            if (!_configPanelProperties.IsCancelEnabled)
                _configPanelProperties.IsCancelEnabled = true;
        });

        public ICommand CreateSelectedBatchesCommand() => new RelayCommand(o =>
        {
            var selectedBatches = (o as IList<object>)?.OfType<ElementBatch>();
            if (_configPanelProperties.SelectedBatches.Count != 0)
                _configPanelProperties.SelectedBatches.Clear();

            if (selectedBatches.Count() != 0)
                foreach (var batch in selectedBatches)
                    _configPanelProperties.SelectedBatches.Add(batch);

            if (_configPanelProperties.SelectedBatches.Count == 1)
                _configPanelProperties.ElementBatch = _configPanelProperties.SelectedBatches
                    .Single().Clone();
        });

        public ICommand CreateRemoveBatchCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedBatches.Count != 0)
            {
                foreach (var batch in _configPanelProperties.SelectedBatches.ToArray())
                    _configPanelProperties.Batches.Remove(batch);
                if (!_configPanelProperties.IsCancelEnabled)
                    _configPanelProperties.IsCancelEnabled = true;
            }
        });

        public ICommand CreateAddRowToBatchCommand() => new RelayCommand(o =>
        {
            _configPanelProperties.ElementBatch.BatchedRows.Add(new BatchedRow());
        });

        public ICommand CreateRemoveRowFromBatchCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedBatchedRow != null)
            {
                bool isRemoved = _configPanelProperties.ElementBatch.BatchedRows.
                    Remove(_configPanelProperties.SelectedBatchedRow);
                if (isRemoved) _configPanelProperties.SelectedBatchedRow = null;
            }
        });

        public ICommand CreateAddConfigCommand() => new RelayCommand(o =>
        {
            _configPanelProperties.Configs.Add()
        }

        private void OnSetParametersToBatchElementExecuted(List<string> parameterNames)
        {
            var parameters = parameterNames
                .Select(pn => new Parameter { Name = pn })
                .ToList();
            _configPanelProperties.NewElementForBatch.Parameters = new ObservableCollection<Parameter>(parameters);
            TaskDialog.Show("OnSetParametersToBatchElementExecuted", "is executed");
        }

        private void OnSetParametersToCircuitElementExecuted(List<string> parameterNames)
        {
            var parameters = parameterNames
                .Select(pn => new Parameter { Name = pn })
                .ToList();
            var circuitElement = _configPanelProperties.SelectedCircuitElements.FirstOrDefault();
            if (circuitElement != null)
                circuitElement.Parameters = new ObservableCollection<Parameter>(parameters);
        }

    }
}

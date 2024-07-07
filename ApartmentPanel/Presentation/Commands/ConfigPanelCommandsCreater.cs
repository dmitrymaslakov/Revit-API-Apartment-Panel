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
using ApartmentPanel.Utility.AnnotationUtility;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using System.Text;
using ApartmentPanel.Core.Services;
using ApartmentPanel.Core.Models.Batch;
using AutoMapper;
using ApartmentPanel.Presentation.Models;
using MediatR;
using ApartmentPanel.UseCases.Configs.Queries.GetConfig;
using ApartmentPanel.UseCases.Configs.Dto;
using ApartmentPanel.Utility;
using System.Collections;
using ApartmentPanel.Core.Enums;
using ApartmentPanel.UseCases.Configs.Commands.UpdateConfig;
using ApartmentPanel.UseCases.Configs.Commands.CreateConfig;
using ApartmentPanel.UseCases.ApartmentElementAnnotations.Commands.CreateElementAnnotation;

namespace ApartmentPanel.Presentation.Commands
{
    public class ConfigPanelCommandsCreater : BaseCommandsCreater
    {
        private readonly IConfigPanelViewModel _configPanelVM;
        /*private readonly Action<List<(string name, string category, string family)>> _showListElements;
        private readonly Action<IApartmentElement> _addElementToApartment;*/
        private readonly CircuitService _circuitService;
        //private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ConfigPanelCommandsCreater(IConfigPanelViewModel configPanelVM,
            IElementService elementService, IMapper mapper, IMediator mediator) : base(elementService)
        {
            _configPanelVM = configPanelVM;
            //_mapper = mapper;
            _mediator = mediator;
            /*_addElementToApartment = newElement =>
{
bool doesNewElementExistInApartment =
//_configPanelProperties.ApartmentElements
_configPanelVM.ApartmentElementsViewModel.ApartmentElements
.Any(ae => ae.Name.Contains(newElement.Name) && ae.Family.Contains(newElement.Family));

if (!doesNewElementExistInApartment)
{
IApartmentElement newClonedElement = newElement.Clone();
string annotationName = new AnnotationNameBuilder()
.AddFolders(_configPanelVM.CurrentConfig)
.AddPartsOfName("-", newClonedElement.Family, newClonedElement.Name)
.Build();

newClonedElement.Annotation = elementService
.SetAnnotationName(annotationName)
.GetAnnotation();

//_configPanelProperties.ApartmentElements.Add(newClonedElement);
_configPanelVM.ApartmentElementsViewModel
.ApartmentElements.Add(newClonedElement);
}
};
_showListElements = props =>
{
IListElementsViewModel listElementsVM = _configPanelVM.ListElementsVM;
listElementsVM.AddElementToApartment = _addElementToApartment;
listElementsVM.AllElements =
new ObservableCollection<IApartmentElement>(_elementService.GetAll(props));
new ElementList(listElementsVM).ShowDialog();
};*/
            _circuitService = new CircuitService(_configPanelVM);
        }

        /*public ICommand CreateShowListElementsCommand() => new RelayCommand(o =>
        {
            _elementService.AddToApartment(_showListElements);
        });

        public ICommand CreateRemoveElementsFromApartmentCommand() => new RelayCommand(o =>
        {
            if (_configPanelProperties.SelectedApartmentElements.Count != 0)
            {
                foreach (var element in _configPanelProperties.SelectedApartmentElements.ToArray())
                    //_configPanelProperties.ApartmentElements.Remove(element);
                    _configPanelProperties.ApartmentElementsViewModel.ApartmentElements.Remove(element);
                if (!_configPanelProperties.IsCancelEnabled)
                    _configPanelProperties.IsCancelEnabled = true;
            }
        });*/

        /*public ICommand CreateAddCircuitToPanelCommand() => new RelayCommand(o =>
        {
            if (!string.IsNullOrEmpty(_configPanelVM.NewCircuit)
              && !_configPanelVM.PanelCircuits
                  .ToList()
                  .Exists(c => c.Number == _configPanelVM.NewCircuit))
            {
                _configPanelVM.PanelCircuits.Add(new Circuit
                {
                    Number = _configPanelVM.NewCircuit,
                    Elements = new ObservableCollection<IApartmentElement>()
                });

                if (!_configPanelVM.IsCancelEnabled)
                    _configPanelVM.IsCancelEnabled = true;
            }

            _configPanelVM.NewCircuit = string.Empty;
        });

        public ICommand CreateRemoveCircuitsFromPanelCommand() => new RelayCommand(o =>
        {
            _configPanelVM.CircuitElements.Clear();
            _configPanelVM.SelectedCircuitElements.Clear();
            foreach (var circuit in _configPanelVM.SelectedPanelCircuits.ToArray())
                _configPanelVM.PanelCircuits.Remove(circuit);

            _configPanelVM.SelectedPanelCircuits.Clear();

            if (!_configPanelVM.IsCancelEnabled)
                _configPanelVM.IsCancelEnabled = true;
        });*/

        public ICommand CreateAddElementToCircuitCommand() => new RelayCommand(o =>
        {
            if (_configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.Count > 1
                || _configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.Count == 0
                || _configPanelVM.ApartmentElementsVM.SelectedApartmentElements.Count == 0)
                return;
            var selectedPanelCircuit = _configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.SingleOrDefault();

            foreach (var selectedApartmentElement in _configPanelVM.ApartmentElementsVM.SelectedApartmentElements)
            {
                if (selectedApartmentElement == null
                || string.IsNullOrEmpty(selectedPanelCircuit.Number))
                    return;

                var IsElementExist = _configPanelVM.PanelCircuitsVM.PanelCircuits
                .Where(e => e.Number == selectedPanelCircuit.Number)
                .First()
                .Elements
                .Any(ae => ae.Name.Contains(selectedApartmentElement.Name) 
                    && ae.Family.Contains(selectedApartmentElement.Family));
                /*.Select(ae => ae.Name)
                .Contains(selectedApartmentElement.Name);*/

                if (!IsElementExist)
                {
                    IApartmentElement elementClone = selectedApartmentElement.Clone();
                    selectedApartmentElement.AnnotationChanged += elementClone.AnnotationChanged_Handler;
                    elementClone.IsSubscriber = true;
                    _configPanelVM.SetParametersToElement = null;
                    _configPanelVM.SetParametersToElement = (List<string> parameterNames) =>
                        {
                            var parameters = parameterNames
                                .Select(pn => new Parameter { Name = pn })
                                .ToList();
                            elementClone.Parameters = new ObservableCollection<Parameter>(parameters);
                        };
                    SetParamsDTO setParamsDTO = new SetParamsDTO
                    {
                        ElementName = elementClone.Name,
                        SetInstanceParameters = _configPanelVM.SetParametersToElement
                    };
                    _elementService.SetElementParameters(setParamsDTO);

                    _configPanelVM.PanelCircuitsVM.PanelCircuits
                    .First(c => c.Number == selectedPanelCircuit.Number).Elements.Add(elementClone);
                }

                _circuitService.AddCurrentCircuitElements(_configPanelVM.PanelCircuitsVM.PanelCircuits.First(c => c.Number == selectedPanelCircuit.Number).Elements);
            }
            if (!_configPanelVM.IsCancelEnabled)
                _configPanelVM.IsCancelEnabled = true;
        });

        public ICommand CreateRemoveElementsFromCircuitCommand() => new RelayCommand(o =>
        {
            if (_configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.Count > 1 || _configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.Count == 0
            || _configPanelVM.CircuitElementsVM.SelectedCircuitElements.Count == 0) return;

            var selectedPanelCircuit = _configPanelVM.PanelCircuitsVM.SelectedPanelCircuits.SingleOrDefault();
            if (string.IsNullOrEmpty(selectedPanelCircuit.Number)) return;

            foreach (var selectedCircuitElement in _configPanelVM.CircuitElementsVM.SelectedCircuitElements)
            {
                selectedPanelCircuit.Elements.Remove(selectedCircuitElement);
                UnsubscribeFromAnnotationChanged(selectedCircuitElement);
            }

            _circuitService.AddCurrentCircuitElements(selectedPanelCircuit.Elements);
            if (!_configPanelVM.IsCancelEnabled)
                _configPanelVM.IsCancelEnabled = true;
        });

        private void UnsubscribeFromAnnotationChanged(IApartmentElement subscriber)
        {
            //IApartmentElement sender = _configPanelProperties.ApartmentElements
            IApartmentElement sender = _configPanelVM.ApartmentElementsVM.ApartmentElements
                .FirstOrDefault(ae => ae.Name.Contains(subscriber.Name));
            if (sender != null) sender.AnnotationChanged -= subscriber.AnnotationChanged_Handler;
        }

        /*public ICommand CreateSelectApartmentElementsCommand() => new RelayCommand(o =>
        {
            var selectedElements = o as List<IApartmentElement>;
            if (_configPanelVM.ApartmentElementsViewModel.SelectedApartmentElements.Count != 0)
                _configPanelVM.ApartmentElementsViewModel.SelectedApartmentElements.Clear();

            if (selectedElements.Count() != 0)
                foreach (var apartmentElement in selectedElements)
                    _configPanelVM.ApartmentElementsViewModel.SelectedApartmentElements.Add(apartmentElement);
        });*/

        /*public ICommand CreateSelectPanelCircuitCommand() => new RelayCommand(o =>
        {
            _configPanelVM.SelectedCircuitElements.Clear();
            var currentCircuits = (o as IList<object>)
                ?.OfType<Circuit>();
            if (currentCircuits.Count() != 0)
            {
                _configPanelVM.SelectedPanelCircuits.Clear();

                foreach (var currentCircuit in currentCircuits)
                {
                    _configPanelVM.SelectedPanelCircuits.Add(currentCircuit);
                    if (currentCircuits.Count() == 1)
                        _circuitService.AddCurrentCircuitElements(currentCircuit.Elements);
                    else _configPanelVM.CircuitElements.Clear();
                }
            }
        });*/

        /*public ICommand CreateSelectCircuitElementCommand() => new RelayCommand(o =>
        {
            var circuitElements = (o as IList<object>)?.OfType<ApartmentElement>();
            if (_configPanelVM.SelectedCircuitElements.Count != 0)
                _configPanelVM.SelectedCircuitElements.Clear();

            if (circuitElements.Count() != 0)
                foreach (var circuitElement in circuitElements)
                    _configPanelVM.SelectedCircuitElements.Add(circuitElement);
        });*/

        public ICommand CreateOkCommand() => new RelayCommand(o =>
        {
            var close = (Action)o;
            CreateApplyCommand()?.Execute(null);
            close();
        });

        public ICommand CreateApplyCommand() => new RelayCommand(o =>
        {
            ObservableCollection<Circuit> circuits = _configPanelVM.PanelCircuitsVM.PanelCircuits;
            ObservableCollection<ElementBatch> batches = _configPanelVM.Batches;
            ObservableCollection<double> heightsOk = _configPanelVM.ListHeightsOK;
            ObservableCollection<double> heightsUk = _configPanelVM.ListHeightsUK;
            ObservableCollection<double> heightsCenter = _configPanelVM.ListHeightsCenter;

            _configPanelVM.OkApplyCancelActions(
                (circuits, batches, heightsOk, heightsUk, heightsCenter), OkApplyCancel.Apply);

            if (_configPanelVM.IsCancelEnabled)
                _configPanelVM.IsCancelEnabled = false;
        });

        public ICommand CreateCancelCommand() => new RelayCommand(o =>
        {
            //_configPanelProperties.OkApplyCancelActions(_configPanelProperties.PanelCircuits, OkApplyCancel.Cancel);
            _configPanelVM.OkApplyCancelActions(null, OkApplyCancel.Cancel);
            if (_configPanelVM.IsCancelEnabled)
                _configPanelVM.IsCancelEnabled = false;
        });

        public ICommand CreateSetAnnotationToElementCommand() => new RelayCommand(async o =>
        {
            if (_configPanelVM.ApartmentElementsVM.SelectedApartmentElements.Count == 1
                && _configPanelVM.AnnotationPreview != null)
            {
                IApartmentElement apartmentElement =
                    _configPanelVM.ApartmentElementsVM.SelectedApartmentElements.FirstOrDefault();
                var request = new CreateElementAnnotationRequest
                {
                    Name = apartmentElement.Name,
                    Family = apartmentElement.Family,
                    Config = _configPanelVM.CurrentConfig,
                    Annotation = _configPanelVM.AnnotationPreview
                };
                var annotation = await _mediator.Send(request);
                apartmentElement.Annotation = annotation;
                /*string annotationName = new PathBuilder()
                .AddFolders(_configPanelVM.CurrentConfig)
                .AddPartsOfName("-", apartmentElement.Family, apartmentElement.Name)
                .Build();

                _elementService
                .SetAnnotationName(annotationName)
                .SetAnnotationTo(apartmentElement, _configPanelVM.AnnotationPreview);*/
            }
        });

        public ICommand CreateSetAnnotationToElementsBatchCommand() => new RelayCommand(o =>
        {
            string annotationName = new PathBuilder()
                .AddFolders(_configPanelVM.CurrentConfig)
                .AddPartsOfName("", _configPanelVM.ElementBatch.Name)
                .Build();

            var annotationService = new AnnotationService(
                new FileAnnotationCommunicatorFactory(annotationName));

            _configPanelVM.ElementBatch.Annotation =
                annotationService.Save(_configPanelVM.AnnotationPreview);
        });

        public ICommand CreateSetAnnotationPreviewCommand() => new RelayCommand(o =>
        {
            if (!(o is BitmapSource bitmapSource)) return;

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            memoryStream.Position = 0;
            bImg.BeginInit();
            bImg.StreamSource = memoryStream;
            bImg.EndInit();
            _configPanelVM.AnnotationPreview = bImg;
        });

        public ICommand CreateLoadLatestConfigCommand() => new RelayCommand(async o =>
        {
            GetConfigDto dto = await _mediator.Send(new GetConfigRequest(_configPanelVM.SelectedConfig));
            _configPanelVM.ApplyLatestConfiguration(dto);
            /*var stringBuilder = new StringBuilder(FileUtility.GetAssemblyPath());
            stringBuilder
            .Append($"\\{_configPanelVM.SelectedConfig}")
            .Append("LatestConfig")
            .Append(".json");

            _configPanelVM.LatestConfigPath = stringBuilder.ToString();
            if (File.Exists(_configPanelVM.LatestConfigPath))
            {
                string json = File.ReadAllText(_configPanelVM.LatestConfigPath);
                //var configPanelVM = JsonSerializer.Deserialize<ConfigPanelViewModel>(
                var latestConfiguration = JsonSerializer.Deserialize<LatestConfiguration>(
                    json, _elementService.GetSerializationOptions());
                var configPanelVM = _mapper.Map<ConfigPanelViewModel>(latestConfiguration);
                _configPanelVM.ApplyLatestConfiguration(configPanelVM);
            }
            else
            {
                var configPanel = ResetConfiguration();
                string json = JsonSerializer.Serialize(configPanel,
                    _elementService.GetSerializationOptions());
                File.WriteAllText(_configPanelVM.LatestConfigPath, json);
            }*/
            _configPanelVM.CurrentConfig = _configPanelVM.SelectedConfig;
        });

        private ConfigPanelViewModel ResetConfiguration()
        {
            //_configPanelProperties.ApartmentElements = new ObservableCollection<IApartmentElement>();
            _configPanelVM.ApartmentElementsVM.ApartmentElements = new ObservableCollection<IApartmentElement>();
            _configPanelVM.PanelCircuitsVM.PanelCircuits = new ObservableCollection<Circuit>();
            _configPanelVM.Batches = new ObservableCollection<ElementBatch>();

            _configPanelVM.ListHeightsOK = new ObservableCollection<double>();
            _configPanelVM.ListHeightsUK = new ObservableCollection<double>();
            _configPanelVM.ListHeightsCenter = new ObservableCollection<double>();

            _configPanelVM.ResponsibleForHeight = string.Empty;
            _configPanelVM.ResponsibleForCircuit = string.Empty;

            return _configPanelVM as ConfigPanelViewModel;
        }

        public ICommand CreateSaveLatestConfigCommand() => new RelayCommand(async o =>
        {
            var configPanel = o as ConfigPanelViewModel;
            var ae = configPanel.ApartmentElementsVM.ApartmentElements as ICollection<ApartmentElement>;
            var eb = configPanel.Batches as ICollection<ElementBatch>;
            var circ = configPanel.PanelCircuitsVM.PanelCircuits as ICollection<Circuit>;
            var hCenter = configPanel.ListHeightsCenter.Select(h => new Height { FromFloor = h, TypeOf = TypeOfHeight.Center });
            var hOK = configPanel.ListHeightsOK.Select(h => new Height { FromFloor = h, TypeOf = TypeOfHeight.OK });
            var hUK = configPanel.ListHeightsUK.Select(h => new Height { FromFloor = h, TypeOf = TypeOfHeight.UK });
            var rfh = new List<string> { configPanel.ResponsibleForHeight };
            var rfc = new List<string> { configPanel.ResponsibleForCircuit };
            var requiest = new GetConfigDto
            {
                ApartmentElements = ae,
                ElementBatches = eb,
                Circuits = circ,
                Heights = hCenter.Concat(hOK).Concat(hUK).ToList(),
                ResponsibleForHeights = rfh,
                ResponsibleForCircuits = rfc,
            };
            await _mediator.Send(new UpdateConfigRequest(requiest));
            /*var latestConfig = _mapper.Map<LatestConfiguration>(configPanel);
            string json = JsonSerializer.Serialize(latestConfig,
                _elementService.GetSerializationOptions());

            File.WriteAllText(_configPanelVM.LatestConfigPath, json);*/
        });

        public ICommand CreateSetNewElementForBatchCommand() => new RelayCommand(o =>
        {
            (string circuit, IApartmentElement element) = (ValueTuple<string, IApartmentElement>)o;

            IApartmentElement cloneElement = element.Clone();

            /*var p1 = element.Parameters.FirstOrDefault();
            var p2 = cloneElement.Parameters.FirstOrDefault();

            bool b = Equals(p1, p2);
            var hc1 = p1.GetHashCode();
            var hc2 = p2.GetHashCode();*/

            _configPanelVM.NewElementForBatch = new BatchedElement
            {
                Circuit = circuit,
                Category = cloneElement.Category,
                Family = cloneElement.Family,
                Name = cloneElement.Name,
                Annotation = cloneElement.Annotation,
                Parameters = cloneElement.Parameters
            };
        });

        public ICommand CreateAddElementToRowCommand() => new RelayCommand(o =>
        {
            if (_configPanelVM.NewElementForBatch == null) return;

            var row = (BatchedRow)o;
            var newElementForBatchClone = _configPanelVM.NewElementForBatch.Clone();
            row.RowElements.Add(newElementForBatchClone);
            if (!_configPanelVM.IsCancelEnabled)
                _configPanelVM.IsCancelEnabled = true;
        });

        public ICommand CreateRemoveElementFromRowCommand() => new RelayCommand(o =>
        {
            var row = (BatchedRow)o;
            row.RowElements.Remove(_configPanelVM.SelectedBatchedElement);
            if (!_configPanelVM.IsCancelEnabled)
                _configPanelVM.IsCancelEnabled = true;
        });

        public ICommand CreateAddBatchToElementBatchesCommand() => new RelayCommand(o =>
        {
            var clone = _configPanelVM.ElementBatch.Clone();

            if (_configPanelVM.Batches.Any(b => string.Equals(b.Name, clone.Name)))
            {
                var replacedEl = _configPanelVM.Batches
                    .FirstOrDefault(b => string.Equals(b.Name, clone.Name));
                int index = _configPanelVM.Batches.IndexOf(replacedEl);

                if (index != -1) _configPanelVM.Batches[index] = clone;
            }
            else
            {
                _configPanelVM.Batches.Add(clone);
            }
            if (!_configPanelVM.IsCancelEnabled)
                _configPanelVM.IsCancelEnabled = true;
        });

        public ICommand CreateSelectedBatchesCommand() => new RelayCommand(o =>
        {
            var selectedBatches = (o as IList<object>)?.OfType<ElementBatch>();
            if (_configPanelVM.SelectedBatches.Count != 0)
                _configPanelVM.SelectedBatches.Clear();

            if (selectedBatches.Count() != 0)
                foreach (var batch in selectedBatches)
                    _configPanelVM.SelectedBatches.Add(batch);

            if (_configPanelVM.SelectedBatches.Count == 1)
                _configPanelVM.ElementBatch = _configPanelVM.SelectedBatches
                    .Single().Clone();
        });

        public ICommand CreateRemoveBatchCommand() => new RelayCommand(o =>
        {
            if (_configPanelVM.SelectedBatches.Count != 0)
            {
                foreach (var batch in _configPanelVM.SelectedBatches.ToArray())
                    _configPanelVM.Batches.Remove(batch);
                if (!_configPanelVM.IsCancelEnabled)
                    _configPanelVM.IsCancelEnabled = true;
            }
        });

        public ICommand CreateAddRowToBatchCommand() => new RelayCommand(o =>
        {
            _configPanelVM.ElementBatch.BatchedRows.Add(new BatchedRow());
        });

        public ICommand CreateRemoveRowFromBatchCommand() => new RelayCommand(o =>
        {
            if (_configPanelVM.SelectedBatchedRow != null)
            {
                bool isRemoved = _configPanelVM.ElementBatch.BatchedRows.
                    Remove(_configPanelVM.SelectedBatchedRow);
                if (isRemoved) _configPanelVM.SelectedBatchedRow = null;
            }
        });

        public ICommand CreateAddConfigCommand() => new RelayCommand(async o =>
        {
            string newConfig = _configPanelVM.NewConfig;
            if (!string.IsNullOrEmpty(newConfig)
                && !_configPanelVM.Configs
                .ToList()
                .Exists(c => c == newConfig))
            {
                bool isCreated = await _mediator.Send(new CreateConfigRequest(newConfig));
                if (!isCreated) return;
                _configPanelVM.Configs.Add(_configPanelVM.NewConfig);

                if (!_configPanelVM.IsCancelEnabled)
                    _configPanelVM.IsCancelEnabled = true;
            }
            _configPanelVM.NewConfig = string.Empty;
        });

        public ICommand CreateRemoveConfigCommand() => new RelayCommand(o =>
        {
            string selectedConfig = _configPanelVM.SelectedConfig;
            if (!string.IsNullOrEmpty(selectedConfig))
            {
                var stringBuilder = new StringBuilder(FileUtility.GetAssemblyFolder());
                stringBuilder
                .Append($"\\{selectedConfig}")
                .Append("LatestConfig")
                .Append(".json");

                string selectedConfigPath = stringBuilder.ToString();
                if (File.Exists(selectedConfigPath))
                    File.Delete(selectedConfigPath);

                _configPanelVM.Configs.Remove(selectedConfig);
                if (!_configPanelVM.IsCancelEnabled)
                    _configPanelVM.IsCancelEnabled = true;
            }
        });
    }
}

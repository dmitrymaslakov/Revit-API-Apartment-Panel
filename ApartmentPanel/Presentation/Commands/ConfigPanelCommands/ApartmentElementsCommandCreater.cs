using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Core.Models.Interfaces;
using MediatR;
using ApartmentPanel.UseCases.ApartmentElements.Queries.GetApartmentElements;
using System.Collections.ObjectModel;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Presentation.View.Components;
using AutoMapper;
using ApartmentPanel.UseCases.ElectricalElements.Queries.GetElectricalElements;
using ApartmentPanel.Presentation.Models;
using ApartmentPanel.UseCases.ApartmentElements.Commands.AddElementToApartment;
using ApartmentPanel.UseCases.ElectricalElements.Dto;
using ApartmentPanel.Core.Services;
using ApartmentPanel.UseCases.ApartmentElements.Dto;
using ApartmentPanel.UseCases.ApartmentElements.Commands.CreateApartmentElement;

namespace ApartmentPanel.Presentation.Commands.ConfigPanelCommands
{
    public class ApartmentElementsCommandCreater : BaseCommandsCreater
    {
        private readonly IConfigPanelViewModel _configPanelVM;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        //private readonly Action<List<(string name, string category, string family)>> _showElementList;
        private readonly Action<ElectricalElement> _addElementToApartment;

        public ApartmentElementsCommandCreater(IConfigPanelViewModel configPanelVM,
            IElementService elementService, IMediator mediator, IMapper mapper) : base(elementService)
        {
            _configPanelVM = configPanelVM;
            _mediator = mediator;
            _mapper = mapper;
            /*_showElementList = props =>
{
IListElementsViewModel listElementsVM = _configPanelVM.ListElementsVM;
listElementsVM.AddElementToApartment = _addElementToApartment;
listElementsVM.AllElements =
new ObservableCollection<IApartmentElement>(_elementService.GetAll(props));
new ElementList(listElementsVM).ShowDialog();
};*/
            _addElementToApartment = async newElement =>
            {
                var apartmentElements = _configPanelVM.ApartmentElementsVM.ApartmentElements;
                bool doesNewElementExistInApartment = apartmentElements
                    .Any(ae => ae.Name.Contains(newElement.Name) && ae.Family.Contains(newElement.Family));

                if (!doesNewElementExistInApartment)
                {
                    //var dto = _mapper.Map<ElectricalElementDto>(newElement);
                    var dto = new ApartmentElementCreateDto
                    {
                        Name = newElement.Name,
                        Family = newElement.Family,
                        Category = newElement.Category,
                        Config = _configPanelVM.CurrentConfig
                    };
                    var apartmentElement = await _mediator.Send(new CreateApartmentElementRequest(dto));
                    apartmentElements.Add(apartmentElement);

                    /*IApartmentElement newClonedElement = newElement.Clone();
                    string annotationName = new AnnotationNameBuilder()
                        .AddFolders(_configPanelVM.CurrentConfig)
                        .AddPartsOfName("-", newClonedElement.Family, newClonedElement.Name)
                        .Build();

                    newClonedElement.Annotation = _elementService
                        .SetAnnotationName(annotationName)
                        .GetAnnotation();

                    apartmentElements.Add(newClonedElement);*/
                }
            };
        }

        /*public ICommand CreateShowElementListCommand() => new RelayCommand(o =>
            _elementService.AddToApartment(_showElementList));*/

        public ICommand CreateShowElementListCommand() => new RelayCommand(async o =>
        {
            var electricalElements = await _mediator.Send(new GetElectricalElementsRequest());
            IListElementsViewModel listElementsVM = _configPanelVM.ListElementsVM;
            var mappedElectricalElements = electricalElements
            .Select(ee => _mapper.Map<ElectricalElement>(ee))
            .ToList();
            listElementsVM.AddElementToApartment = _addElementToApartment;
            listElementsVM.AllElements =
                new ObservableCollection<ElectricalElement>(mappedElectricalElements);
            new ElementList(listElementsVM).Show();
        });

        public ICommand CreateRemoveApartmentElementCommand() => new RelayCommand(o =>
        {
            if (_configPanelVM.ApartmentElementsVM.SelectedApartmentElements.Count != 0)
            {
                foreach (var element in _configPanelVM.ApartmentElementsVM.SelectedApartmentElements.ToArray())
                    _configPanelVM.ApartmentElementsVM.ApartmentElements.Remove(element);
                if (!_configPanelVM.IsCancelEnabled)
                    _configPanelVM.IsCancelEnabled = true;
            }
        });

        public ICommand CreateSelectApartmentElementCommand() => new RelayCommand(o =>
        {
            var selectedElements = o as List<IApartmentElement>;
            if (_configPanelVM.ApartmentElementsVM.SelectedApartmentElements.Count != 0)
                _configPanelVM.ApartmentElementsVM.SelectedApartmentElements.Clear();

            if (selectedElements.Count() != 0)
                foreach (var apartmentElement in selectedElements)
                    _configPanelVM.ApartmentElementsVM.SelectedApartmentElements.Add(apartmentElement);
        });
    }
}

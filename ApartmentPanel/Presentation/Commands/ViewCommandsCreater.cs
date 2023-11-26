using ApartmentPanel.Core.Services.Interfaces;
using System;
using System.Windows.Input;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Presentation.View.Components;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Core.Enums;
using ApartmentPanel.Core.Models;

namespace ApartmentPanel.Presentation.Commands
{
    public class ViewCommandsCreater : BaseCommandsCreater
    {
        private readonly IMainViewModel _viewProperties;

        public ViewCommandsCreater(IMainViewModel viewProperties,
            IElementService apartmentElementService) : base(apartmentElementService) =>
            _viewProperties = viewProperties;

        public ICommand CreateConfigureCommand() => new RelayCommand(o =>
        {
            new ConfigPanel(_viewProperties.ConfigPanelVM).Show();
        });

        public ICommand CreateInsertElementCommand() => new RelayCommand(o =>
        {
            (string circuit, string elementName, string elementCategory) =
                (ValueTuple<string, string, string>)o;

            string insertingMode = "single";

            if (Keyboard.Modifiers == ModifierKeys.Control)
                insertingMode = "multiple";

            var elementDTO = new InsertElementDTO()
            {
                Name = elementName,
                Category = elementCategory,
                Circuit = circuit,
                /*Height = _viewProperties.ElementHeight != null
                    ? _viewProperties.ElementHeight.Value
                    : default,
                TypeOfHeight = _viewProperties.ElementHeight?.TypeOf,*/
                CurrentSuffix = _viewProperties.CurrentSuffix,
                InsertingMode = insertingMode
            };
            if (_viewProperties.ElementHeight != null)
            {
                elementDTO.Height.Value = _viewProperties.ElementHeight.Value;
                elementDTO.Height.TypeOf = _viewProperties.ElementHeight.TypeOf;
            }
            _elementService.InsertToModel(elementDTO);
        });

        public ICommand CreateSetCurrentSuffixCommand()
            => new RelayCommand(o => _viewProperties.CurrentSuffix = o as string);

        public ICommand CreateSetHeightCommand() => new RelayCommand(o =>
        {
            (TypeOfHeight typeOfHeight, double height) = (ValueTuple<TypeOfHeight, double>)o;
            _viewProperties.ElementHeight = new Height { TypeOf = typeOfHeight, Value = height };
        });

        public ICommand CreateSetStatusCommand() => new RelayCommand(o =>
        {
            switch (o as string)
            {
                case "MouseEnter":
                    _viewProperties.Status = "You should select the lamp(s) before inserting the switch";
                    break;
                case "MouseLeave":
                    _viewProperties.Status = null;
                    break;
            }
        });
    }
}

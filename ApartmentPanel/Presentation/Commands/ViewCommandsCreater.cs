using ApartmentPanel.Core.Services;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Infrastructure;
using ApartmentPanel.Infrastructure.Repositories;
using ApartmentPanel.Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.Commands
{
    public class ViewCommandsCreater
    {
        private readonly IViewPropsForCommandsCreater _viewProperties;
        private readonly IApartmentElementService _apartmentElementService;

        public ViewCommandsCreater(IViewPropsForCommandsCreater viewProperties, IApartmentElementService apartmentElementService)
        {
            _viewProperties = viewProperties;
            _apartmentElementService = apartmentElementService;
        }

        public ICommand CreateConfigureCommand() => new RelayCommand(o =>
        {
            _viewProperties.Handler.Props = _viewProperties.ConfigPanelVM;
            _viewProperties.Handler.Request.Make(RequestId.Configure);
            _viewProperties.ExEvent.Raise();
        });

        public ICommand CreateInsertElementCommand() => new RelayCommand(o =>
        {
            (string circuit, string elementName, string elementCategory) =
                (ValueTuple<string, string, string>)o;

            string insertingMode = "single";

            if (Keyboard.Modifiers == ModifierKeys.Control)
                insertingMode = "multiple";

            var props = new Dictionary<string, string>
                {
                    { nameof(circuit), circuit },
                    { nameof(elementName), elementName },
                    { nameof(elementCategory), elementCategory },
                    { "lampSuffix", _viewProperties.CurrentSuffix },
                    { "height", _viewProperties.Height.ToString() },
                    { nameof(insertingMode), insertingMode },
                };
            _apartmentElementService.Insert(props);

            /*_viewProperties.Handler.Props = new Dictionary<string, string>
                {
                    { nameof(circuit), circuit },
                    { nameof(elementName), elementName },
                    { nameof(elementCategory), elementCategory },
                    { "lampSuffix", _viewProperties.CurrentSuffix },
                    { "height", _viewProperties.Height.ToString() },
                    { nameof(insertingMode), insertingMode },
                };
            _viewProperties.Handler.Request.Make(RequestId.Insert);
            _viewProperties.ExEvent.Raise();*/
        });

        public ICommand CreateSetCurrentSuffixCommand()
            => new RelayCommand(o => _viewProperties.CurrentSuffix = o as string);
    }
}

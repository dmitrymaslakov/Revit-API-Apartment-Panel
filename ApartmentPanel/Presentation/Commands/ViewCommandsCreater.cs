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
        private readonly IViewForCommandsCreater _uiProperties;
        private readonly IApartmentElementService _apartmentElementService;

        public ViewCommandsCreater(IViewForCommandsCreater uiProperties)
        {
            _uiProperties = uiProperties;
            _apartmentElementService = new ApartmentElementService(new ApartmentElementRepository());
        }

        public ICommand CreateConfigureCommand() => new RelayCommand(o =>
        {
            _uiProperties.Handler.Props = _uiProperties.EditPanelVM;
            _uiProperties.Handler.Request.Make(RequestId.Configure);
            _uiProperties.ExEvent.Raise();
        });

        public ICommand CreateInsertElementCommand() => new RelayCommand(o =>
        {
            (string circuit, string elementName, string elementCategory) =
                (ValueTuple<string, string, string>)o;

            string insertingMode = "single";

            if (Keyboard.Modifiers == ModifierKeys.Control)
                insertingMode = "multiple";

            _uiProperties.Handler.Props = new Dictionary<string, string>
                {
                    { nameof(circuit), circuit },
                    { nameof(elementName), elementName },
                    { nameof(elementCategory), elementCategory },
                    { "lampSuffix", _uiProperties.CurrentSuffix },
                    { "height", _uiProperties.Height.ToString() },
                    { nameof(insertingMode), insertingMode },
                };
            _uiProperties.Handler.Request.Make(RequestId.Insert);
            _uiProperties.Handler.Handle = _apartmentElementService.Insert;
            _uiProperties.ExEvent.Raise();
        });

        public ICommand CreateSetCurrentSuffixCommand()
            => new RelayCommand(o => _uiProperties.CurrentSuffix = o as string);
    }
}

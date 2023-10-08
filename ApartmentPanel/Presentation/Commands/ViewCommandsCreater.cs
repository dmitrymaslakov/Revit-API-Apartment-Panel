using ApartmentPanel.Core.Services;
using ApartmentPanel.Infrastructure;
using ApartmentPanel.Infrastructure.Repositories;
using ApartmentPanel.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using ApartmentPanel.Presentation.ViewModel.Interfaces;

namespace ApartmentPanel.Presentation.Commands
{
    public class ViewCommandsCreater : BaseCommandsCreater
    {
        //private readonly IViewPropsForCommandsCreater _viewProperties;
        private readonly IMainViewModel _viewProperties;

        //public ViewCommandsCreater(IViewPropsForCommandsCreater viewProperties, IApartmentElementService apartmentElementService)
        public ViewCommandsCreater(IMainViewModel viewProperties,
            IElementService apartmentElementService,
            IPanelService apartmentPanelService) : base(apartmentElementService, apartmentPanelService)
        {
            _viewProperties = viewProperties;
        }

        public ICommand CreateConfigureCommand() => new RelayCommand(o =>
        {
            /*_viewProperties.Handler.Props = _viewProperties.ConfigPanelVM;
            _viewProperties.Handler.Request.Make(RequestId.Configure);
            _viewProperties.ExEvent.Raise();*/
            _panelService.Configure();
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
            _elementService.InsertToModel(props);

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

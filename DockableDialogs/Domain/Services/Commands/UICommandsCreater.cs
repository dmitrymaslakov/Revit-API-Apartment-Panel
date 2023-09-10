using Autodesk.Revit.UI;
using DockableDialogs.ViewModel;
using DockableDialogs.ViewModel.ComponentsVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DockableDialogs.Domain.Services.Commands
{
    public class UICommandsCreater
    {
        private readonly IUIToCommandsCreater _uiProperties;

        public UICommandsCreater(IUIToCommandsCreater uiProperties) => _uiProperties = uiProperties;

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
            _uiProperties.ExEvent.Raise();
        });

        public ICommand CreateSetCurrentSuffixCommand()
            => new RelayCommand(o => _uiProperties.CurrentSuffix = o as string);
    }
}

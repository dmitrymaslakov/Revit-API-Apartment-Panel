using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using WpfPanel.ViewModel;
using WpfPanel.ViewModel.ComponentsVM;

namespace WpfPanel.Domain.Services.Commands
{
    public class UICommandsCreater
    {
        private readonly IUIToCommandsCreater _uiProperties;

        public UICommandsCreater(IUIToCommandsCreater uiProperties) => _uiProperties = uiProperties;

        public ICommand CreateLoadLatestConfigCommand() => new RelayCommand(o =>
        {
            if (File.Exists(_uiProperties.LatestConfigPath))
            {
                string json = File.ReadAllText(_uiProperties.LatestConfigPath);
                EditPanelVM deso = JsonSerializer.Deserialize<EditPanelVM>(json);
                _uiProperties.EditPanelVM.ApplyLatestConfiguration(deso);
            }
        });

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

            _uiProperties.Handler.Props = new Dictionary<string, string>
                {
                    { nameof(circuit), circuit },
                    { nameof(elementName), elementName },
                    { nameof(elementCategory), elementCategory },
                { "lampSuffix", _uiProperties.CurrentSuffix },
                    { "height", _uiProperties.Height.ToString() },
                };
            _uiProperties.Handler.Request.Make(RequestId.Insert);
            _uiProperties.ExEvent.Raise();
        });

        public ICommand CreateSetCurrentSuffixCommand() 
            => new RelayCommand(o => _uiProperties.CurrentSuffix = o as string);

        public ICommand CreateSaveLatestConfigCommand() => new RelayCommand(o =>
        {
            try
            {
                string json = JsonSerializer.Serialize(_uiProperties.EditPanelVM);
                File.WriteAllText(_uiProperties.LatestConfigPath, json);
            }
            catch (NotSupportedException)
            {

            }
        });
    }
}

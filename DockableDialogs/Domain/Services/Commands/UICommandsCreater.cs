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

        public ICommand CreateLoadLatestConfigCommand() => new RelayCommand(o =>
        {
            if (File.Exists(_uiProperties.LatestConfigPath))
            {
                string json = File.ReadAllText(_uiProperties.LatestConfigPath);
                EditPanelVM deso = JsonSerializer.Deserialize<EditPanelVM>(json);
                //WeatherForecast deso = JsonSerializer.Deserialize<WeatherForecast>(json);
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

        public ICommand CreateSaveLatestConfigCommand() => new RelayCommand(o =>
        {
            /*var weatherForecast = new WeatherForecast
            {
                Date = DateTime.Parse("2019-08-01"),
                TemperatureCelsius = 25,
                Summary = "Hot",
                SummaryField = "Hot",
                DatesAvailable = new List<DateTimeOffset>()
                    { DateTime.Parse("2019-08-01"), DateTime.Parse("2019-08-02") },
                TemperatureRanges = new Dictionary<string, HighLowTemps>
                {
                    ["Cold"] = new HighLowTemps { High = 20, Low = -10 },
                    ["Hot"] = new HighLowTemps { High = 60, Low = 20 }
                },
                SummaryWords = new[] { "Cool", "Windy", "Humid" }
            };*/

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_uiProperties.EditPanelVM, options);
            //string json = JsonSerializer.Serialize(weatherForecast, options);
            File.WriteAllText(_uiProperties.LatestConfigPath, json);
        });
    }
}

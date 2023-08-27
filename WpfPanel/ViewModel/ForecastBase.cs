using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfPanel.ViewModel
{
    public abstract class ForecastBase
    {
        private int temperatureC;
        private string summary;

        public ForecastBase(int temperatureC, string summary)
        {
            TemperatureC = temperatureC;
            Summary = summary;
        }

        public int TemperatureC { get => temperatureC; set => temperatureC = value; }

        public string Summary { get => summary; set => summary = value; }
    }
}

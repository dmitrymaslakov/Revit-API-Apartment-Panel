using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfPanel.ViewModel
{
    public class Forecast : ForecastBase
    {
        /*private int temperatureC;
        private string summary;*/

        /*public Forecast(int temperatureC, string summary)
        {
            TemperatureC = temperatureC;
            Summary = summary;
        }*/
        public Forecast(int temperatureC, string summary) : base(temperatureC, summary)
        {
        }

        /*public int TemperatureC { get => temperatureC; set => temperatureC = value; }

        public string Summary { get => summary; set => summary = value; }*/
    }
}

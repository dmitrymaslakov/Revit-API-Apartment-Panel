using ApartmentPanel.Core.Models.Interfaces;
using System.Collections.ObjectModel;

namespace ApartmentPanel.Core.Models
{
    /// <summary>
    /// Represents a circuit of the apartment panel
    /// </summary>
    public class Circuit
    {
        public string Number { get; set; }
        public ObservableCollection<IApartmentElement> Elements { get; set; }
    }
}

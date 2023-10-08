using ApartmentPanel.Core.Models.Interfaces;
using System.Collections.ObjectModel;

namespace ApartmentPanel.Core.Models
{
    public class CategorizedFamilySymbols
    {
        public string Category { get; set; }
        public ObservableCollection<IApartmentElement> CategorizedElements { get; set; }
    }
}

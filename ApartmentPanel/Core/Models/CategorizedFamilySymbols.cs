using ApartmentPanel.Core.Models;
using System.Collections.ObjectModel;

namespace ApartmentPanel.Core.Models
{
    public class CategorizedFamilySymbols
    {
        public string Category { get; set; }
        public ObservableCollection<ApartmentElement> CategorizedElements { get; set; }
    }
}

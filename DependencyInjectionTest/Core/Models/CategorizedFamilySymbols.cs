using System.Collections.ObjectModel;

namespace DependencyInjectionTest.Core.Models
{
    public class CategorizedFamilySymbols
    {
        public string Category { get; set; }
        public ObservableCollection<ApartmentElement> CategorizedElements { get; set; }
    }
}

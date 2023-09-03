//using Autodesk.Revit.DB;
using System.Collections.ObjectModel;

namespace DockableDialogs.Domain.Models
{
    public class CategorizedFamilySymbols
    {
        public string Category { get; set; }
        //public ObservableCollection<FamilySymbol> CategorizedElements { get; set; }
        public ObservableCollection<ApartmentElement> CategorizedElements { get; set; }
    }
}

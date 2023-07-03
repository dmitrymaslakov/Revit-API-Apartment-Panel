using System.Collections.ObjectModel;
using WpfPanel.Domain.Models.RevitMockupModels;

namespace WpfPanel.Domain.Models
{
    public class CategorizedFamilySymbols
    {
        public string Category { get; set; }
        public ObservableCollection<FamilySymbol> CategorizedElements { get; set; }
    }
}

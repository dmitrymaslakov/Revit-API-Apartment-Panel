using DependencyInjectionTest.Core.Models.Interfaces;
using System.Collections.ObjectModel;

namespace DependencyInjectionTest.Core.Models
{
    public class Circuit
    {
        public string Number { get; set; }
        public ObservableCollection<IApartmentElement> Elements { get; set; }
    }
}

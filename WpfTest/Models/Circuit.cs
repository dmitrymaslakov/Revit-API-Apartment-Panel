using System.Collections.ObjectModel;

namespace WpfTest.Models
{
    public class Circuit
    {
        public string Number { get; set; }
        public ObservableCollection<ApartmentElement> Elements { get; set; }
    }
}

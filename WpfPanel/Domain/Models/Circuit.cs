using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPanel.Domain.Models
{
    public class Circuit
    {
        public string Number { get; set; }
        public ObservableCollection<ApartmentElement> ApartmentElements { get; set; }
    }
}

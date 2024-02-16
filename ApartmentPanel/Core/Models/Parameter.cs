using ApartmentPanel.Utility;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ApartmentPanel.Core.Models
{
    public class Parameter : NotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _value;
        public string Value
        {
            get => _value;
            set => Set(ref _value, value);
        }
    }
}

using ApartmentPanel.Utility;
using ApartmentPanel.Utility.Interfaces;

namespace ApartmentPanel.Core.Models
{
    public class Parameter : NotifyPropertyChanged, IDeepClone<Parameter>
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

        public Parameter Clone() => MemberwiseClone() as Parameter;
    }
}

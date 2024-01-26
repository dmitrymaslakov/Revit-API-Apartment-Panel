using ApartmentPanel.Presentation.ViewModel;

namespace ApartmentPanel.Presentation.Models
{
    public class Parameter : ViewModelBase
    {
        /*public string Name { get; set; }
        public string Value { get; set; }*/
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

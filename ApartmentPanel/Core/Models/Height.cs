using ApartmentPanel.Core.Enums;
using ApartmentPanel.Utility;

namespace ApartmentPanel.Core.Models
{
    public class Height : NotifyPropertyChanged
    {
        public TypeOfHeight TypeOf { get; set; }
        private double _fromFloor;
        public double FromFloor 
        { 
            get => _fromFloor; 
            set => Set(ref _fromFloor, value); 
        }
        private double _fromLevel;
        public double FromLevel 
        { 
            get => _fromLevel;
            set => Set(ref _fromLevel, value);
        }

        private string _responsibleForHeightParameter;
        public string ResponsibleForHeightParameter 
        { 
            get => _responsibleForHeightParameter;
            set => Set(ref _responsibleForHeightParameter, value);
        }

        public Height Clone()
        {
            return new Height
            {
                ResponsibleForHeightParameter = ResponsibleForHeightParameter,
                TypeOf = TypeOf,
                FromFloor = FromFloor,
                FromLevel = FromLevel
            };
        }
    }
}

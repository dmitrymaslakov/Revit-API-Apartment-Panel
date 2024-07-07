using ApartmentPanel.Core.Enums;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Utility;
using ApartmentPanel.Utility.Interfaces;
using System;

namespace ApartmentPanel.Core.Models
{
    public class Height : NotifyPropertyChanged, IDeepClone<Height>, IEntity
    {
        public Height() => Id = Guid.NewGuid();
        public Guid Id { get; }
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

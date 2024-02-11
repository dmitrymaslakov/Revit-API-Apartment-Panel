using ApartmentPanel.Core.Enums;

namespace ApartmentPanel.Core.Models
{
    public class Height
    {
        public TypeOfHeight TypeOf { get; set; }
        public double Value { get; set; }
        public string ResponsibleForHeightParameter { get; set; }

        public Height Clone()
        {
            return new Height
            {
                ResponsibleForHeightParameter = ResponsibleForHeightParameter,
                TypeOf = TypeOf,
                Value = Value
            };
        }
    }
}

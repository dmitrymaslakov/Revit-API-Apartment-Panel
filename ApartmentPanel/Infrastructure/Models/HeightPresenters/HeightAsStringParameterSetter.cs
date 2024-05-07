using ApartmentPanel.Core.Enums;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class HeightAsStringParameterSetter : HeightParameterSetterBase
    {
        private readonly TypeOfHeight _typeOfHeight;

        public HeightAsStringParameterSetter(UIApplication uiapp, TypeOfHeight typeOfHeight) : base(uiapp) 
            => _typeOfHeight = typeOfHeight;

        public override void Set(Parameter parameter, double height) 
        {
            string heightAsString = _typeOfHeight == TypeOfHeight.Center
                            ? $"H={height}"
                            : $"{_typeOfHeight}={height}";
            parameter.Set(heightAsString);
        }
    }
}

using ApartmentPanel.Presentation.Models;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class HeightAsDoubleParameterSetter : HeightParameterSetterBase
    {
        public HeightAsDoubleParameterSetter(UIApplication uiapp) : base(uiapp) { }
        public override void Set(Parameter parameter, double height) 
        {
            double heightAsFeets = UnitUtils.ConvertToInternalUnits(height,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());
            parameter.Set(heightAsFeets);
        }
    }
}

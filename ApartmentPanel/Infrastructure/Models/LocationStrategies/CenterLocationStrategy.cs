using Autodesk.Revit.UI;
using ApartmentPanel.Infrastructure.Enums;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class CenterLocationStrategy : LocationStrategyBase, ILocationStrategy
    {
        public CenterLocationStrategy(UIApplication uiapp) : base(uiapp) { }

        public void SetRequiredLocation(BuiltInstance builtInstance, double height)
        {
            LocationType locationType = LocationType.Center;
            SetRequiredLocationBase(builtInstance, height, locationType);
        }
    }
}

using ApartmentPanel.Infrastructure.Enums;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class BottomLocationStrategy : LocationStrategyBase, ILocationStrategy
    {
        public BottomLocationStrategy(UIApplication uiapp) : base(uiapp) { }

        public void SetRequiredLocation(BuiltInstance builtInstance, double height)
        {
            LocationType locationType = LocationType.Bottom;
            SetRequiredLocationBase(builtInstance, height, locationType);
        }
    }
}

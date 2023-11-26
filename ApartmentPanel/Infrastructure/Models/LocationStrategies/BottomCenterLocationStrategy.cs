using Autodesk.Revit.DB;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class BottomCenterLocationStrategy : ILocationStrategy
    {
        public void SetRequiredLocation(FamilyInstance familyInstance, double height)
        {
            var bb = familyInstance.get_BoundingBox(null);
        }
    }
}

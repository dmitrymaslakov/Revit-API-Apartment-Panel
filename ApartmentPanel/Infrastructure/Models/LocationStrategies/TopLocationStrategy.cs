using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class TopLocationStrategy : RevitInfrastructureBase, ILocationStrategy
    {
        public TopLocationStrategy(UIApplication uiapp) : base(uiapp) { }

        public void SetRequiredLocation(FamilyInstance familyInstance, double height)
        {
            XYZ basePoint = (familyInstance.Location as LocationPoint)?.Point;
            XYZ maxPoint = familyInstance.get_BoundingBox(null).Max;
            XYZ targetPoint = new XYZ(basePoint.X, basePoint.Y, maxPoint.Z);
            XYZ deltaPoints = basePoint.Subtract(targetPoint);

            double heightInFeets = UnitUtils.ConvertToInternalUnits(height,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            XYZ newBasePoint = /*basePoint
                .Subtract(deltaPoints)
                .Add(new XYZ(0, 0, heightInFeets))*/
                new XYZ(deltaPoints.X, deltaPoints.Y, deltaPoints.Z + heightInFeets)
                ;
            familyInstance.Location.Move(newBasePoint);
        }
    }
}

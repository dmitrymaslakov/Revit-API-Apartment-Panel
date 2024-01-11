using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class BottomLocationStrategy : RevitInfrastructureBase, ILocationStrategy
    {
        public BottomLocationStrategy(UIApplication uiapp) : base(uiapp) { }

        public double HorizontalOffset { get; set; }

        public void SetRequiredLocation(FamilyInstance familyInstance, double height)
        {
            XYZ basePoint = (familyInstance.Location as LocationPoint)?.Point;
            XYZ minPoint = familyInstance.get_BoundingBox(null).Min;
            XYZ targetPoint = new XYZ(basePoint.X, basePoint.Y, minPoint.Z);
            XYZ deltaPoints = basePoint.Subtract(targetPoint);

            double heightInFeets = UnitUtils.ConvertToInternalUnits(height,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            XYZ newBasePoint = new XYZ(basePoint.X, basePoint.Y, deltaPoints.Z + heightInFeets);
            familyInstance.Location.Move(newBasePoint - basePoint);
        }

        private double GetFullOffset(FamilyInstance familyInstance)
        {
            XYZ basePoint = (familyInstance.Location as LocationPoint)?.Point;
            XYZ minPoint = familyInstance.get_BoundingBox(null).Min;
            return Math.Abs(basePoint.X - minPoint.X) + HorizontalOffset;
        }
    }
}

using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using ApartmentPanel.Utility;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class TopLocationStrategy : RevitInfrastructureBase, ILocationStrategy
    {        
        public TopLocationStrategy(UIApplication uiapp) : base(uiapp) { }

        public double HorizontalOffset { get; set; }

        public void SetRequiredLocation(FamilyInstance familyInstance, double height)
        {
            var poinCounter = new FamilyInstacePointCounter(_uiapp, familyInstance);
            var (basePoint, maxPoint, minPoint) = (poinCounter.Location, poinCounter.Max, poinCounter.Min);

            XYZ targetPoint = new XYZ(basePoint.X, basePoint.Y, maxPoint.Z);
            XYZ deltaPoints = basePoint.Subtract(targetPoint);
            double heightInFeets = UnitUtils.ConvertToInternalUnits(height,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            double fullOffset = HorizontalOffset == 0 
                ? 0 
                : Math.Abs(basePoint.X - minPoint.X) + HorizontalOffset;
            XYZ newBasePoint = new XYZ(basePoint.X + fullOffset, basePoint.Y, deltaPoints.Z + heightInFeets);
            XYZ translation = newBasePoint - basePoint;
            familyInstance.Location.Move(translation);
        }
    }
}

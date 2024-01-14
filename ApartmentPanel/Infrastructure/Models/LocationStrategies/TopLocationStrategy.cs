using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using ApartmentPanel.Utility;
using System.Net.Http.Headers;
using ApartmentPanel.Utility.Extensions;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class TopLocationStrategy : RevitInfrastructureBase, ILocationStrategy
    {        
        public TopLocationStrategy(UIApplication uiapp) : base(uiapp) { }

        public double HorizontalOffset { get; set; }

        public void SetRequiredLocation(FamilyInstance familyInstance, double height)
        {
            var instancePoints = new FamilyInstacePoints(_uiapp, familyInstance);
            var (basePoint, maxPoint, minPoint) = (instancePoints.Location, instancePoints.Max, instancePoints.Min);

            XYZ targetPoint = new XYZ(basePoint.X, basePoint.Y, maxPoint.Z);
            XYZ deltaPoints = basePoint.Subtract(targetPoint);
            double heightInFeets = UnitUtils.ConvertToInternalUnits(height,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            double fullOffset = HorizontalOffset == 0 
                ? 0 
                : Math.Abs(maxPoint.X - minPoint.X)/2 + HorizontalOffset;
            XYZ newBasePoint = new XYZ(basePoint.X + fullOffset, basePoint.Y, deltaPoints.Z + heightInFeets);
            XYZ translation = newBasePoint - basePoint;
            Transform instanceTransform = familyInstance.GetTransform();
            XYZ localXAxis = instanceTransform.BasisX;
            XYZ globalXAxis = XYZ.BasisX;
            double angle = globalXAxis.AngleOnPlaneTo(localXAxis, XYZ.BasisZ);
            Transform rotation = Transform.CreateRotation(XYZ.BasisZ, angle);
            XYZ rotatedTranslation = rotation.OfVector(translation);

            familyInstance.Location.Move(rotatedTranslation);
        }
    }
}

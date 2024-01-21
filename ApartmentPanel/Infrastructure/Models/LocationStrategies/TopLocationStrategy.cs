using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ApartmentPanel.Utility;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class TopLocationStrategy : RevitInfrastructureBase, ILocationStrategy
    {
        public TopLocationStrategy(UIApplication uiapp) : base(uiapp) { }

        public double HorizontalOffset { get; set; }

        public void SetRequiredLocation(BuiltInstance builtInstance, double height)
        {
            var familyInstance = _document.GetElement(builtInstance.Id) as FamilyInstance;
            var instancePoints = new FamilyInstacePoints(_uiapp, familyInstance);
            var (basePoint, maxPoint) = (instancePoints.Location, instancePoints.Max);

            XYZ targetPoint = new XYZ(basePoint.X, basePoint.Y, maxPoint.Z);
            XYZ deltaPoints = basePoint.Subtract(targetPoint);
            double heightInFeets = UnitUtils.ConvertToInternalUnits(height,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            double angle = GetAngleBetweenGlobalXAxisAndLocalXAxis(familyInstance);

            double fullOffset = HorizontalOffset == 0
                ? 0
                : builtInstance.Width / 2 + HorizontalOffset;

            XYZ newBasePoint = new XYZ(basePoint.X + fullOffset, basePoint.Y, deltaPoints.Z + heightInFeets);
            XYZ translation = newBasePoint - basePoint;
            Transform rotation = Transform.CreateRotation(XYZ.BasisZ, angle);
            XYZ rotatedTranslation = rotation.OfVector(translation);
            using (var tr = new Transaction(_document, "Instance translation"))
            {
                tr.Start();
                familyInstance.Location.Move(rotatedTranslation);
                tr.Commit();
            }
        }

        private double GetAngleBetweenGlobalXAxisAndLocalXAxis(FamilyInstance familyInstance)
        {
            Transform instanceTransform = familyInstance.GetTransform();
            XYZ localXAxis = instanceTransform.BasisX;
            XYZ globalXAxis = XYZ.BasisX;
            return globalXAxis.AngleOnPlaneTo(localXAxis, XYZ.BasisZ);
        }
    }
}

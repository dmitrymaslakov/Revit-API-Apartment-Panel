using ApartmentPanel.Infrastructure.Enums;
using ApartmentPanel.Utility;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public abstract class LocationStrategyBase : RevitInfrastructureBase
    {
        public LocationStrategyBase(UIApplication uiapp) : base(uiapp) { }

        public double HorizontalOffset { get; set; }

        protected void SetRequiredLocationBase(BuiltInstance builtInstance, double height, LocationType locationType)
        {
            var familyInstance = _document.GetElement(builtInstance.Id) as FamilyInstance;
            var instancePoints = new FamilyInstacePoints(_uiapp, familyInstance);
            var (basePoint, maxPoint, minPoint) = (instancePoints.Location, instancePoints.Max, instancePoints.Min);
            XYZ targetPoint = null;
            switch (locationType)
            {
                case LocationType.Top:
                    targetPoint = new XYZ(basePoint.X, basePoint.Y, maxPoint.Z);
                    break;
                case LocationType.Bottom:
                    targetPoint = new XYZ(basePoint.X, basePoint.Y, minPoint.Z);
                    break;
            }

            XYZ deltaPoints = basePoint.Subtract(targetPoint);
            double heightInFeets = UnitUtils.ConvertToInternalUnits(height,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            double angle = GetAngleBetweenGlobalXAxisAndLocalXAxis(familyInstance);

            double fullOffset = HorizontalOffset == 0
                ? 0
                : builtInstance.Width / 2 + HorizontalOffset;

            double levelElevation = GetLevelElevation();
            //XYZ newBasePoint = new XYZ(basePoint.X + fullOffset, basePoint.Y, deltaPoints.Z + heightInFeets);
            XYZ newBasePoint = new XYZ(
                basePoint.X + fullOffset, 
                basePoint.Y,
                levelElevation + deltaPoints.Z + heightInFeets);
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

        protected double GetAngleBetweenGlobalXAxisAndLocalXAxis(FamilyInstance familyInstance)
        {
            Transform instanceTransform = familyInstance.GetTransform();
            XYZ localXAxis = instanceTransform.BasisX;
            XYZ globalXAxis = XYZ.BasisX;
            return globalXAxis.AngleOnPlaneTo(localXAxis, XYZ.BasisZ);
        }
        protected ElementId GetViewLevel(Document doc)
        {
            View active = doc.ActiveView;
            ElementId levelId = null;
            Parameter level = active.LookupParameter("Associated Level");
            if (level == null)
                return null;

            FilteredElementCollector lvlCollector = new FilteredElementCollector(doc);
            ICollection<Element> lvlCollection = lvlCollector.OfClass(typeof(Level)).ToElements();
            foreach (Element l in lvlCollection)
            {
                Level lvl = l as Level;
                if (lvl.Name == level.AsString())
                    levelId = lvl.Id;
            }
            return levelId;
        }

        protected double GetLevelElevation()
        {
            ElementId levelId = GetViewLevel(_document);
            Level level = _document.GetElement(levelId) as Level;
            double elevation = level.Elevation;
            return elevation;
        }
    }
}

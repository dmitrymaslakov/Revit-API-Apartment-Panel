using ApartmentPanel.Utility;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApartmentPanel.Infrastructure.Models
{
    public class BuiltInstance : RevitInfrastructureBase
    {
        public BuiltInstance(UIApplication uiapp, ElementId instanceId) : base(uiapp)
        {
            Id = instanceId;
            FamilyInstance familyInstance = _document.GetElement(Id) as FamilyInstance;
            bool areParallel = AreGlobalXAxisAndLocalXAxisParallel(familyInstance);
            var instancePoints = GetInstancePoints(familyInstance, areParallel);
            Width = GetInstanceWidth(instancePoints);
            MinLocalDelta = GetMinLocalDelta(instancePoints);
            MaxLocalDelta = GetMaxLocalDelta(instancePoints);
        }

        public ElementId Id { get; set; }
        public double Width { get; set; }
        public double MinLocalDelta { get; set; }
        public double MaxLocalDelta { get; set; }

        private (XYZ, XYZ, XYZ) GetInstancePoints(
            FamilyInstance familyInstance, bool areGlobalXAxisAndLocalXAxisParallel)
        {
            if (areGlobalXAxisAndLocalXAxisParallel)
            {
                var instancePoints = new FamilyInstacePoints(_uiapp, familyInstance);
                return (instancePoints.Max, instancePoints.Min, instancePoints.Location);
            }
            else
            {
                Wall tempWall = new RevitUtility(_uiapp).CreateWall();
                FamilySymbol symbol = familyInstance.Symbol;
                FamilyInstance tempInstance = CreateTempFamilyInstance(symbol, tempWall);
                var instancePoints = new FamilyInstacePoints(_uiapp, tempInstance);
                (XYZ, XYZ, XYZ) res = (instancePoints.Max, instancePoints.Min, instancePoints.Location);

                List<ElementId> deletedElementIds = new List<ElementId>
                    {
                        tempWall.Id, tempInstance.Id
                    };
                _document.Delete(deletedElementIds);
                return res;
            }
        }

        private double GetInstanceWidth((XYZ max, XYZ min, XYZ location) instancePoints)
        {
            return Math.Abs(instancePoints.max.X - instancePoints.min.X);
            /*double width;

            if (areGlobalXAxisAndLocalXAxisParallel)
            {
                var instancePoints = new FamilyInstacePoints(_uiapp, familyInstance);
                var (maxPoint, minPoint) = (instancePoints.Max, instancePoints.Min);
                width = Math.Abs(maxPoint.X - minPoint.X);
            }
            else
            {
                Wall tempWall = new RevitUtility(_uiapp).CreateWall();
                FamilySymbol symbol = familyInstance.Symbol;
                FamilyInstance tempInstance = CreateTempFamilyInstance(symbol, tempWall);
                var instancePoints = new FamilyInstacePoints(_uiapp, tempInstance);
                var (maxPoint, minPoint) = (instancePoints.Max, instancePoints.Min);
                width = Math.Abs(maxPoint.X - minPoint.X);

                List<ElementId> deletedElementIds = new List<ElementId>
                    {
                        tempWall.Id, tempInstance.Id
                    };
                _document.Delete(deletedElementIds);
            }
            return width;*/
        }
        private double GetMinLocalDelta((XYZ max, XYZ min, XYZ location) instancePoints) => 
            Math.Abs(instancePoints.min.X - instancePoints.location.X);
        private double GetMaxLocalDelta((XYZ max, XYZ min, XYZ location) instancePoints) =>
            Math.Abs(instancePoints.max.X - instancePoints.location.X);

        private bool AreGlobalXAxisAndLocalXAxisParallel(FamilyInstance familyInstance)
        {
            Transform instanceTransform = familyInstance.GetTransform();
            XYZ localXAxis = instanceTransform.BasisX;
            XYZ globalXAxis = XYZ.BasisX;
            double angle = globalXAxis.AngleOnPlaneTo(localXAxis, XYZ.BasisZ);
            double pi = Math.Round(Math.PI, 3);
            double twoPi = Math.Round(Math.PI * 2, 3);

            if (Equals(Math.Round(angle, 3), pi) || Equals(Math.Round(angle, 3), twoPi))
                return true;
            else
                return false;
        }

        private FamilyInstance CreateTempFamilyInstance(FamilySymbol symbol, Wall wall)
        {
            Face face = null;
            Options geomOptions = new Options { ComputeReferences = true };

            GeometryElement wallGeom = wall.get_Geometry(geomOptions);
            foreach (GeometryObject geomObj in wallGeom)
            {
                Solid geomSolid = geomObj as Solid;
                if (null != geomSolid)
                {
                    foreach (Face geomFace in geomSolid.Faces)
                    {
                        face = geomFace;
                        break;
                    }
                    break;
                }
            }

            // Get the center of the wall 
            BoundingBoxUV bboxUV = face.GetBoundingBox();
            UV center = (bboxUV.Max + bboxUV.Min) / 2.0;
            XYZ location = face.Evaluate(center);
            XYZ normal = face.ComputeNormal(center);
            XYZ refDir = normal.CrossProduct(XYZ.BasisZ);

            FamilyInstance newFamilyInstance = _document
                .Create
                .NewFamilyInstance(face, location, refDir, symbol);
            _document.Regenerate();
            return newFamilyInstance;
        }
    }
}

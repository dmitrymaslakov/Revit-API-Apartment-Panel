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
            Width = GetInstanceWidth(familyInstance, areParallel);
        }

        public ElementId Id { get; set; }
        public double Width { get; set; }

        private double GetInstanceWidth(
            FamilyInstance familyInstance, bool areGlobalXAxisAndLocalXAxisParallel)
        {
            double width;

            if (areGlobalXAxisAndLocalXAxisParallel)
            {
                var instancePoints = new FamilyInstacePoints(_uiapp, familyInstance);
                var (maxPoint, minPoint) = (instancePoints.Max, instancePoints.Min);
                width = Math.Abs(maxPoint.X - minPoint.X);
            }
            else
            {
                Wall tempWall = CreateWall();
                FamilySymbol symbol = familyInstance.Symbol;
                FamilyInstance tempInstance = CreateTempFamilyInstance(symbol, tempWall);
                var instancePoints = new FamilyInstacePoints(_uiapp, tempInstance);
                var (maxPoint, minPoint) = (instancePoints.Max, instancePoints.Min);
                width = Math.Abs(maxPoint.X - minPoint.X);

                using (var tr = new Transaction(_document, "Removing temp elements"))
                {
                    tr.Start();
                    List<ElementId> deletedElementIds = new List<ElementId>
                    {
                        tempWall.Id, tempInstance.Id
                    };
                    _document.Delete(deletedElementIds);
                    tr.Commit();
                }
            }
            return width;
        }
        
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
            XYZ dir = new XYZ(0, 0, 0);

            FamilyInstance newFamilyInstance = null;
            using (var tr = new Transaction(_document, "Creating new FamilyInstance"))
            {
                tr.Start();
                newFamilyInstance = _document
                    .Create
                    .NewFamilyInstance(face, location, refDir, symbol);
                tr.Commit();
            }
            return newFamilyInstance;
        }

        private Wall CreateWall()
        {
            // Create a line to define the wall's location
            Line line = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(10, 0, 0));

            // Find a suitable wall type (you may need to adjust this based on your project)
            FilteredElementCollector collector = new FilteredElementCollector(_document);
            WallType wallType = collector
                .OfClass(typeof(WallType))
                .Cast<WallType>()
                .FirstOrDefault(wt => wt.Kind == WallKind.Basic);
            Wall wallResult = null;
            if (wallType != null)
            {
                // Set the level for the wall (you may need to adjust this based on your project)
                Level level = new FilteredElementCollector(_document)
                    .OfCategory(BuiltInCategory.OST_Levels)
                    .WhereElementIsNotElementType()
                    .Cast<Level>()
                    .FirstOrDefault();

                if (level != null)
                {
                    using (Transaction transaction = new Transaction(_document, "Create Wall"))
                    {
                        transaction.Start();
                        wallResult = Wall.Create(_document, line, wallType.Id, level.Id, 10, 0, false, false);
                        transaction.Commit();
                    }
                }
                else
                {
                    TaskDialog.Show("Error", "No suitable level found.");
                }
            }
            else
            {
                TaskDialog.Show("Error", "No suitable wall type found.");
            }
            return wallResult;
        }

    }
}

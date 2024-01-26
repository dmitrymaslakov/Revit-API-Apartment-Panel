using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using Utility.Extensions;

namespace RevitTest
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AnalizeCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            AnalizeElement(commandData.Application);
            return Result.Succeeded;
        }
        private void AnalizeElement(UIApplication uiapp)
        {
            UIApplication _uiapp = uiapp;
            UIDocument _uiDocument = _uiapp.ActiveUIDocument;
            Document _document = _uiDocument.Document;
            Selection _selection = _uiDocument.Selection;

            try
            {
                var selectedElementId = _selection.GetElementIds().FirstOrDefault();
                var familyInstance = _document.GetElement(selectedElementId) as FamilyInstance;

                Transform instanceTransform = familyInstance.GetTransform();
                XYZ localXAxis = instanceTransform.BasisX;
                XYZ globalXAxis = XYZ.BasisX;
                double angle = globalXAxis.AngleOnPlaneTo(localXAxis, XYZ.BasisZ);
                double angle1 = globalXAxis.AngleOnPlaneTo(globalXAxis, XYZ.BasisZ);
                bool isAnglesEquals = Equals(Math.Round(angle, 3), Math.Round(Math.PI * 2, 3));
                double tolerance = 0.001; // set the tolerance value
                bool areParallel = globalXAxis.IsAlmostEqualTo(localXAxis, tolerance);
                Debug.Write($"angleRad - {angle}");
                Debug.Write($"angleDeg - {ToDegrees(angle)}");
                Debug.Write($"areParallel - {areParallel}");
                /*View3D view3D = new FilteredElementCollector(_document)
                    .OfClass(typeof(View3D))
                    .Cast<View3D>()
                    .FirstOrDefault(v => !v.IsTemplate);
                Options geomOpts = new Options { View = view3D };
                var instanceGeometry = familyInstance
                    .get_Geometry(geomOpts)
                    .OfType<GeometryInstance>()
                    ?.FirstOrDefault()
                    ?.GetInstanceGeometry();
                var symbolGeometry = familyInstance
                    .get_Geometry(geomOpts)
                    .OfType<GeometryInstance>()
                    ?.FirstOrDefault()
                    ?.SymbolGeometry;

                var viewBB = view3D.get_BoundingBox(null);
                foreach (GeometryObject item in symbolGeometry)
                {         
                    Solid solid = item as Solid;
                    //Debug.Write($"viz - {viz}");
                }

                XYZ max = instanceGeometry.GetBoundingBox().Max;
                XYZ min = instanceGeometry.GetBoundingBox().Min;
                XYZ symbolMax = symbolGeometry.GetBoundingBox().Max;
                XYZ symbolMin = symbolGeometry.GetBoundingBox().Min;

                double x = Math.Abs(max.X - min.X);
                double y = Math.Abs(max.Y - min.Y);

                XYZ minMaxXVector = new XYZ(x, y, 0);

                Transform instanceTransform = familyInstance.GetTransform();
                XYZ localXAxis = instanceTransform.BasisX;
                XYZ globalXAxis = XYZ.BasisX;
                double angle = globalXAxis.AngleOnPlaneTo(localXAxis, XYZ.BasisZ);
                double angleInDegrees = ToDegrees(angle);

                Transform rotation = Transform.CreateRotation(XYZ.BasisZ, angle);
                Transform globalTransform = Transform.Identity;
                XYZ rotatedMinMaxXVector = rotation.OfVector(minMaxXVector);*/
                /*var gFS = familySymbol.get_Geometry(geomOpts);
                foreach (var item in gFS)
                {
                    Solid solid = item as Solid;
                    var sBB = solid.GetBoundingBox();
                    var sMax = sBB.Max;
                    var sMin = sBB.Min;
                    Debug.Write($"sMax - {sMax}");
                    Debug.Write($"sMin - {sMin}");
                    Debug.Write($"item - {item}");
                }*/
                /*var instanceGeometry = familySymbol
                    .get_Geometry(geomOpts)
                    .OfType<GeometryInstance>()
                    ?.FirstOrDefault()
                    ?.GetInstanceGeometry();

                XYZ max = instanceGeometry.GetBoundingBox().Max;
                XYZ min = instanceGeometry.GetBoundingBox().Min;*/

                /*Debug.Write($"fsBB.Max - {fsBB.Max}");
                Debug.Write($"fsBB.Min - {fsBB.Min}");
                Debug.Write($"g.Max - {max}");
                Debug.Write($"g.Min - {min}");*/

                /*View view = _document.ActiveView;
                FamilyInstanceCreate(_document, familySymbol, view);*/
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Analize_Exception", ex.Message);
            }
        }
        private ElementId CreateFamilyInstance(Document document, FamilySymbol symbol, Wall wall)
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
            FamilyInstance newFamilyInstance = null;
            using (var tr = new Transaction(document, "Creating new FamilyInstance"))
            {
                tr.Start();
                XYZ dir = new XYZ(0, 0, 0);

                newFamilyInstance = document
                    .Create
                    .NewFamilyInstance(face, location, refDir, symbol);
                tr.Commit();
            }
            return newFamilyInstance.Id;
        }

        private Wall CreateWall(Document doc)
        {
            // Create a line to define the wall's location
            Line line = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(10, 0, 0));

            // Find a suitable wall type (you may need to adjust this based on your project)
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            WallType wallType = collector
                .OfClass(typeof(WallType))
                .Cast<WallType>()
                .FirstOrDefault(wt => wt.Kind == WallKind.Basic);
            Wall wallResult = null;
            if (wallType != null)
            {
                // Set the level for the wall (you may need to adjust this based on your project)
                Level level = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Levels)
                    .WhereElementIsNotElementType()
                    .Cast<Level>()
                    .FirstOrDefault();

                if (level != null)
                {
                    // Create the wall
                    Transaction transaction = new Transaction(doc, "Create Wall");
                    transaction.Start();

                    wallResult = Wall.Create(doc, line, wallType.Id, level.Id, 10, 0, false, false);

                    transaction.Commit();

                    TaskDialog.Show("Success", "Wall created successfully.");
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
        private double ToRadians(double degrees)
            => degrees * Math.PI / 180;
        private double ToDegrees(double radians)
            => radians * 180 / Math.PI;

    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SelectInstanceCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            AnalizeElement(commandData.Application);
            return Result.Succeeded;
        }
        private void AnalizeElement(UIApplication uiapp)
        {
            UIApplication _uiapp = uiapp;
            UIDocument _uiDocument = _uiapp.ActiveUIDocument;
            Document _document = _uiDocument.Document;
            Selection _selection = _uiDocument.Selection;

            try
            {
                ElementCategoryFilter categoryFilter1 = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalFixtures);
                ElementCategoryFilter categoryFilter2 = new ElementCategoryFilter(BuiltInCategory.OST_LightingDevices);
                ElementCategoryFilter categoryFilter3 = new ElementCategoryFilter(BuiltInCategory.OST_LightingFixtures);
                ElementCategoryFilter categoryFilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CommunicationDevices);
                //LogicalOrFilter orFilter = new LogicalOrFilter(categoryFilter1, categoryFilter2);
                LogicalOrFilter orFilter = new LogicalOrFilter(new List<ElementFilter>
                {
                    categoryFilter1, categoryFilter2, categoryFilter3, categoryFilter4
                });
                //var instances = new FilteredElementCollector(_document, _document.ActiveView.Id)
                var instances = new FilteredElementCollector(_document)
                    .WherePasses(orFilter)
                    .OfClass(typeof(FamilyInstance))
                    .ToElements()
                    .Where(fi => TryFindParameterValue(fi))
                    .Select(e => e.Id)
                    ;
                _selection.SetElementIds(new List<ElementId>(instances));
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Analize_Exception", ex.Message);
            }
        }
        private bool TryFindParameterValue(Element instance)
        {
            //string pV = "UK=44"; //48.5
            //string pV = "UK=125";
            //string pV = "UK=144"; //139.5
            //string pV = "UK=230";
            //string pV = "OK=35"; //30.5
            //string pV = "OK=40"; //35.5
            //string pV = "OK=60"; //55.5
            //string pV = "OK=110"; //105.5
            //string pV = "OK=120";//115.5
            //string pV = "OK=121"; //116.5
            string pV = "OK=160"; //155.5
            //string pV = "OK=230";
            //string parameterName = "Elevation from Level";
            string parameterName = "H-UK";
            Parameter parameter = instance.LookupParameter(parameterName);
            //if (parameter == null) throw new ArgumentNullException(parameterName);
            //Debug.Write(string);
            //return Math.Round(parameter.AsDouble(), 2) == 0.0;
            if (parameter is null || string.IsNullOrEmpty(parameter.AsString()))
            {
                return false;
            }
            else
            {
                return parameter.AsString().Contains(pV);
            }
        }
    }
}

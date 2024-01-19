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
                /*var selectedElementId = _selection.GetElementIds().FirstOrDefault();
                var familyInstance = _document.GetElement(selectedElementId) as FamilyInstance;

                View3D view3D = new FilteredElementCollector(_document)
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
                var familySymbol = new FilteredElementCollector(_document)
                    .OfClass(typeof(FamilySymbol))
                    .Where(fs => fs.Name == "Single Socket")
                    .FirstOrDefault() as FamilySymbol;
                var fsBB = familySymbol.get_BoundingBox(null);
                View3D view3D = new FilteredElementCollector(_document)
                    .OfClass(typeof(View3D))
                    .Cast<View3D>()
                    .FirstOrDefault(v => !v.IsTemplate);
                Options geomOpts = new Options
                {
                    ComputeReferences = true,
                    //View = _document.ActiveView,
                    DetailLevel = ViewDetailLevel.Fine
                };
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
        private ElementId FamilyInstanceCreate(Document document, FamilySymbol symbol, View view)
        {
            FamilyInstance newFamilyInstance = null;
            using (var tr = new Transaction(document, "Creating new FamilyInstance"))
            {
                tr.Start();
                XYZ origin = new XYZ(0, 0, 0);
                newFamilyInstance = document.Create
                    .NewFamilyInstance(origin, symbol, view);
                tr.Commit();
            }
            return newFamilyInstance.Id;
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
                var instances = new FilteredElementCollector(_document, _document.ActiveView.Id)
                    .WherePasses(orFilter)
                    .OfClass(typeof(FamilyInstance))
                    .ToElements()
                    .Where(fi => TryFindParameterValue(fi))
                    .Select(e => e.Id);

                _selection.SetElementIds(new List<ElementId>(instances));
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Analize_Exception", ex.Message);
            }
        }
        private bool TryFindParameterValue(Element instance)
        {
            string parameterName = "Elevation from Level";
            Parameter parameter = instance.LookupParameter(parameterName);
            if (parameter == null) throw new ArgumentNullException(parameterName);

            return parameter.AsDouble() == 0;
        }
    }
}

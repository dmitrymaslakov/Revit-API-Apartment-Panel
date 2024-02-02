using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
                ElementId levelId = GetViewLevel(_document);
                Level level = _document.GetElement(levelId) as Level;
                double elevation = level.Elevation;
                Debug.Write($"elevation - {elevation}");
                /*var selectedElementId = _selection.GetElementIds().FirstOrDefault();
                var element = _document.GetElement(selectedElementId);
                var familyInstance = element as FamilyInstance;
                GetLevelInformation(element);*/
                //Getinfo_Level(_document);
                /*Debug.Write($"g.Min - {min}");*/
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
        private ElementId GetViewLevel(Document doc)
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
        private void GetLevelInformation(Element element)
        {
            // Get the level object to which the element is assigned.
            if (element.LevelId.Equals(ElementId.InvalidElementId))
            {
                TaskDialog.Show("Revit", "The element isn't based on a level.");
            }
            else
            {
                Level level = element.Document.GetElement(element.LevelId) as Level;

                // Format the prompt information(Name and elevation)
                String prompt = "The element is based on a level.";
                prompt += "\nThe level name is:  " + level.Name;
                prompt += "\nThe level elevation is:  " + level.Elevation;

                // Show the information to the user.
                TaskDialog.Show("Revit", prompt);
            }
        }
        private void Getinfo_Level(Document document)
        {
            StringBuilder levelInformation = new StringBuilder();
            int levelNumber = 0;
            FilteredElementCollector collector = new FilteredElementCollector(document);
            ICollection<Element> collection = collector.OfClass(typeof(Level)).ToElements();
            foreach (Element e in collection)
            {
                Level level = e as Level;

                if (null != level)
                {
                    // keep track of number of levels
                    levelNumber++;

                    //get the name of the level
                    levelInformation.Append("\nLevel Name: " + level.Name);

                    //get the elevation of the level
                    levelInformation.Append("\n\tElevation: " + level.Elevation);

                    // get the project elevation of the level
                    levelInformation.Append("\n\tProject Elevation: " + level.ProjectElevation);

                    var point = (level.Location as LocationPoint).Point;
                    levelInformation.Append("\n\tpoint: " + point);
                }
            }

            //number of total levels in current document
            levelInformation.Append("\n\n There are " + levelNumber + " levels in the document!");

            //show the level information in the messagebox
            TaskDialog.Show("Revit", levelInformation.ToString());
        }
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
            //string pV = "OK=160"; //155.5
            string pV = "OK=230";
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

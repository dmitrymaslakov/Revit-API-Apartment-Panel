using ApartmentPanel.Utility.SelectionFilters;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentPanel.Utility
{
    public class RevitUtility : RevitInfrastructureBase
    {
        public RevitUtility(UIApplication uiapp) : base(uiapp) { }

        public Wall CreateWall()
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
                    /*using (Transaction transaction = new Transaction(_document, "Create Wall"))
                    {
                        transaction.Start();*/
                        wallResult = Wall.Create(_document, line, wallType.Id, level.Id, 10, 0, false, false);
                    _document.Regenerate();    
                    /*transaction.Commit();
                    }*/
                }
                else
                    TaskDialog.Show("Error", "No suitable level found.");
            }
            else
                TaskDialog.Show("Error", "No suitable wall type found.");
            return wallResult;
        }

        public FamilyInstance CreateFamilyInstance(FamilySymbol symbol, Wall wall)
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
            /*using (var tr = new Transaction(_document, "Creating new FamilyInstance"))
            {
                tr.Start();*/
                if (!symbol.IsActive) ActivateFamilySymbol(symbol);

                newFamilyInstance = _document
                    .Create
                    .NewFamilyInstance(face, location, refDir, symbol);
                /*tr.Commit();
            }*/
            return newFamilyInstance;
        }

        public void ActivateFamilySymbol(FamilySymbol symbol)
        {
            symbol.Activate();
            _document.Regenerate();
        }

        public ISelectionFilter CreateSelectionFilter(ISelectionFilterFactory factory)
        {
            return factory.CreateSelectionFilter();
        }
    }
}

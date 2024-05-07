using ApartmentPanel.Infrastructure.Extensions;
using ApartmentPanel.Utility.Extensions.RevitExtensions;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace ApartmentPanel.Utility
{
    public class FamilyInstacePoints : RevitInfrastructureBase
    {
        public FamilyInstacePoints(UIApplication uiapp, FamilyInstance familyInstance) : base(uiapp)
        {
            Options geomOpts = new Options { DetailLevel = ViewDetailLevel.Fine };
            GeometryElement instanceGeometry = familyInstance
                .get_Geometry(geomOpts)
                .OfType<GeometryInstance>()
                ?.FirstOrDefault()
                ?.GetInstanceGeometry();

            Location = (familyInstance.Location as LocationPoint)?.Point;
            if (instanceGeometry != null)
            {
                var hasCylindricalFace = instanceGeometry
                    .OfType<Solid>()
                    .SelectMany(s => s.Faces.OfType<CylindricalFace>())
                    .Any();
                if (hasCylindricalFace)
                {
                    var solids = instanceGeometry
                        .OfType<Solid>()
                        .Where(s => s.Volume != 0)
                        .Where(s =>
                        {
                            var gStyle = _document.GetElement(s.GraphicsStyleId) as GraphicsStyle;
                            if (s.Volume == 0 || (gStyle != null && gStyle.Name.Contains("Light Source")))
                                return false;
                            else
                                return true;
                        });
                    var unionSolid = solids.Aggregate((x, y) => BooleanOperationsUtils
                        .ExecuteBooleanOperation(x, y, BooleanOperationsType.Union));

                    ElementId directShapeId = null;
                    directShapeId = _document.CreateDirectShape(new List<GeometryObject> { unionSolid }).Id;
                    _document.Regenerate();
                    Element directShape = _document.GetElement(directShapeId);
                    var directShapeBB = directShape.get_BoundingBox(null);
                    Max = directShapeBB.Max;
                    Min = directShapeBB.Min;
                    _document.Delete(directShapeId);
                    _document.Regenerate();
                }
                else
                {
                    Max = instanceGeometry.GetBoundingBox().Max;
                    Min = instanceGeometry.GetBoundingBox().Min;
                }
            }
        }
        public XYZ Location { get; }
        public XYZ Max { get; }
        public XYZ Min { get; }
    }
}

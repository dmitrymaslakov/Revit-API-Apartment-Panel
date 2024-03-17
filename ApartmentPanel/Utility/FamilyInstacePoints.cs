using ApartmentPanel.Infrastructure.Extensions;
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
            /*View3D view3D = new FilteredElementCollector(_document)
                .OfClass(typeof(View3D))
                .Cast<View3D>()
                .FirstOrDefault(v => !v.IsTemplate);
            Options geomOpts = new Options { View = view3D };*/
            Options geomOpts = new Options { DetailLevel = ViewDetailLevel.Fine };
            GeometryElement geometryElement = familyInstance
                .get_Geometry(geomOpts)
                .OfType<GeometryInstance>()
                ?.FirstOrDefault()
                ?.GetInstanceGeometry();

            Location = (familyInstance.Location as LocationPoint)?.Point;
            BoundingBoxXYZ elementBB = null;
            List<BoundingBoxXYZ> boundingBoxes = new List<BoundingBoxXYZ>();
            foreach (GeometryObject geometryObject in geometryElement)
            {
                if (geometryObject is Solid solid)
                {
                    var graphicsStyle = _document.GetElement(solid.GraphicsStyleId) as GraphicsStyle;
                    if (solid.Volume == 0
                        || (graphicsStyle != null 
                        && graphicsStyle.Name.Contains("Light Source")))
                    {
                        continue;
                    }
                    boundingBoxes.Add(solid.GetBoundingBox());
                }
            }
            elementBB = boundingBoxes.Aggregate((acc, elem) => acc.Union(elem));
            if (geometryElement != null)
            {
                /*Max = geometryElement.GetBoundingBox().Max;
                Min = geometryElement.GetBoundingBox().Min;*/
                var max = Location.Add(elementBB.Max);
                var min = Location.Add(elementBB.Min);

                Max = max;
                Min = min;
            }
        }
        public XYZ Location { get; }
        public XYZ Max { get; }
        public XYZ Min { get; }
    }
}

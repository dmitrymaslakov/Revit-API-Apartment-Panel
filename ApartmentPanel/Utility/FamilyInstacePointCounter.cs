using ApartmentPanel.Infrastructure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;

namespace ApartmentPanel.Utility
{
    public class FamilyInstacePointCounter : RevitInfrastructureBase
    {
        public FamilyInstacePointCounter(UIApplication uiapp, FamilyInstance familyInstance) : base(uiapp)
        {
            View3D view3D = new FilteredElementCollector(_document)
                .OfClass(typeof(View3D))
                .Cast<View3D>()
                .FirstOrDefault(v => !v.IsTemplate);
            Options geomOpts = new Options { View = view3D };
            var geometryElement = familyInstance
                .get_Geometry(geomOpts)
                .OfType<GeometryInstance>()
                ?.FirstOrDefault()
                ?.GetInstanceGeometry();
            Location = (familyInstance.Location as LocationPoint)?.Point;
            if (geometryElement != null)
            {
                Max = geometryElement.GetBoundingBox().Max;
                Min = geometryElement.GetBoundingBox().Min;
            }
        }
        public XYZ Location { get; }
        public XYZ Max { get; }
        public XYZ Min { get; }
    }
}

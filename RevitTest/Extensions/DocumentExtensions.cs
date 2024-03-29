using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitTest.Extensions
{
    public static class DocumentExtensions
    {
        public static DirectShape CreateDirectShape(
            this Document doc,
            List<GeometryObject> geometryObjects,
            BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel)
        {
            var directShape = DirectShape.CreateElement(doc, new ElementId(builtInCategory));
            directShape.SetShape(geometryObjects);
            return directShape;
        }
    }
}

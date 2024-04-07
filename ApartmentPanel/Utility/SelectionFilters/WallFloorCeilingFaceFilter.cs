using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ApartmentPanel.Utility.SelectionFilters
{
    internal class WallFloorCeilingFaceFilter : ISelectionFilter
    {
        private readonly Document _document;

        public WallFloorCeilingFaceFilter(Document document) => _document = document;

        public bool AllowElement(Element elem)
        {
            bool result = elem is RevitLinkInstance
                || elem is Wall
                || elem is Floor 
                || elem is Ceiling;
            return result;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            /*GeometryObject geoObject = _document.GetElement(reference).GetGeometryObjectFromReference(reference);
            return geoObject != null && geoObject is Face;*/

            return reference.ElementReferenceType == ElementReferenceType.REFERENCE_TYPE_SURFACE;
        }
    }
}

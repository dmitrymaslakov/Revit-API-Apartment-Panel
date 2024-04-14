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
            /*var ri = _document.GetElement(reference) as RevitLinkInstance;
            Document docLinked = ri.GetLinkDocument();
            Reference linkedref = reference.CreateReferenceInLink();
            Element linkedelement = docLinked.GetElement(linkedref);
            Floor floor = linkedelement as Floor;
            var fi = linkedelement as Wall;
            Element fil = null;
            if (fi != null)
                fil = docLinked.GetElement(fi.LevelId);

            Element ll = null;
            if (floor != null) 
             ll = docLinked.GetElement(floor.LevelId);
            GeometryObject geoObject = linkedelement?.GetGeometryObjectFromReference(reference);
            return geoObject != null && geoObject is Face;*/

            return reference.ElementReferenceType == ElementReferenceType.REFERENCE_TYPE_SURFACE;
            //return linkedelement is Floor;
        }
    }
}

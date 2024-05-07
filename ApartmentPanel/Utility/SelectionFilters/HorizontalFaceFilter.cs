using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ApartmentPanel.Utility.SelectionFilters
{
    internal class HorizontalFaceFilter : ISelectionFilter
    {
        private readonly Document _document;

        public HorizontalFaceFilter(Document document) => _document = document;

        public bool AllowElement(Element elem)
        {
            bool result = elem is RevitLinkInstance
                || elem is Floor 
                || elem is Ceiling;
            return result;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            if (_document.GetElement(reference) is RevitLinkInstance link)
            {
                Document linkDocument = link.GetLinkDocument();
                Reference refInLink = reference.CreateReferenceInLink();
                Element linkedElement = linkDocument.GetElement(refInLink);

                if (linkedElement is Floor || linkedElement is Ceiling)
                {
                    var faceLink = linkedElement.GetGeometryObjectFromReference(refInLink);
                    if (faceLink is PlanarFace planarFace)
                        if (planarFace.FaceNormal.Z != 0) return true;
                }
            }
            else
            {
                Element element = _document.GetElement(reference);

                if (element is Floor || element is Ceiling)
                {
                    var face = element.GetGeometryObjectFromReference(reference);
                    if (face is PlanarFace planarFace)
                        if (planarFace.FaceNormal.Z != 0) return true;
                }
            }
            return false;
        }
    }
}

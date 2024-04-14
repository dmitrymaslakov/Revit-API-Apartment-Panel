using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace ApartmentPanel.Utility.SelectionFilters
{
    public class HorizontalFaceFilterFactory : ISelectionFilterFactory
    {
        private readonly Document _document;

        public HorizontalFaceFilterFactory(Document document) => _document = document;

        public ISelectionFilter CreateSelectionFilter() =>
            new HorizontalFaceFilter(_document);
    }
}

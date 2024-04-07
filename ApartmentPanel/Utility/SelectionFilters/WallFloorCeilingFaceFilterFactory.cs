using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace ApartmentPanel.Utility.SelectionFilters
{
    public class WallFloorCeilingFaceFilterFactory : ISelectionFilterFactory
    {
        private readonly Document _document;

        public WallFloorCeilingFaceFilterFactory(Document document) => _document = document;

        public ISelectionFilter CreateSelectionFilter() =>
            new WallFloorCeilingFaceFilter(_document);
    }
}

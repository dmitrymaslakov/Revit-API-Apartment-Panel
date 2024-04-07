using Autodesk.Revit.UI.Selection;

namespace ApartmentPanel.Utility.SelectionFilters
{
    public interface ISelectionFilterFactory
    {
        ISelectionFilter CreateSelectionFilter();
    }
}
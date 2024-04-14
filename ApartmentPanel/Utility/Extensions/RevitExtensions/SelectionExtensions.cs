using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace ApartmentPanel.Utility.Extensions.RevitExtensions
{
    public static class SelectionExtensions
    {
        public static Reference PickHost(this Selection selection, ISelectionFilter filter) =>
            selection.PickObject(ObjectType.PointOnElement, filter, "Pick a host in the model");
    }
}

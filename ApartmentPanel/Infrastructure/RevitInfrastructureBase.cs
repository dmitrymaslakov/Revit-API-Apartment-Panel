using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure
{
    public abstract class RevitInfrastructureBase
    {
        protected UIApplication _uiapp;
        protected UIDocument _uiDocument;
        protected Document _document;
        protected Selection _selection;

        public RevitInfrastructureBase() { }

        public RevitInfrastructureBase(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _uiDocument = _uiapp.ActiveUIDocument;
            _document = _uiDocument.Document;
            _selection = _uiDocument.Selection;
        }
    }
}

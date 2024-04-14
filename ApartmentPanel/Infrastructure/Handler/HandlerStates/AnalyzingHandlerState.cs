using ApartmentPanel.Utility.Extensions.RevitExtensions;
using ApartmentPanel.Utility.SelectionFilters;
//using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;

namespace ApartmentPanel.Infrastructure.Handler.HandlerStates
{
    internal class AnalyzingHandlerState : HandlerState
    {
        internal override void Handle(UIApplication uiapp)
        {
            SetInfrastructure(uiapp);
            /*using (var tr = new Transaction(_document, "Set Parameters"))
            {
                tr.Start();*/
            ISelectionFilterFactory filterFactory = new HorizontalFaceFilterFactory(_document);
            ISelectionFilter filter = filterFactory.CreateSelectionFilter();
            var reference = _selection.PickHost(filter);
            /*var link = _document.GetElement(reference) as RevitLinkInstance;
            Document linkDocument = link.GetLinkDocument();
            UIDocument uiDoc = new UIDocument(linkDocument);
            Reference refInLink = reference.CreateReferenceInLink();
            Element linkedElement = linkDocument.GetElement(refInLink);
            ElementId selectedElementId = linkDocument.GetElement(reference.LinkedElementId).Id;
            uiDoc.Selection.SetElementIds(new List<ElementId> { selectedElementId });
            _selection.SetElementIds(new List<ElementId> { selectedElementId });*/
            _selection.SetReferences(new List<Reference> { reference });
            /*tr.Commit();
        }*/
        }
    }
}

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialogs.Domain
{
    public class RequestHandler : IExternalEventHandler
    {
        public Request Request { get; } = new Request();

        public void Execute(UIApplication uiapp)
        {
            switch (Request.Take())
            {
                case RequestId.None:
                    {
                        return;  // no request at this time -> we can leave immediately
                    }
                case RequestId.Insert:
                    {
                        InsertTriss(uiapp);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return;
        }

        public string GetName()
        {
            return "Placement triss";
        }

        private static void InsertTriss(UIApplication application)
        {
            if (null == application.ActiveUIDocument.Document)
            {
                return;
            }

            UIDocument uiDocument = application.ActiveUIDocument;
            Document document = uiDocument.Document;
            Selection selection = uiDocument.Selection;
            try
            {
                Family family = new FilteredElementCollector(document)
                    .OfClass(typeof(Family))
                    .ToElements()
                    .FirstOrDefault(el => el.Name.CompareTo("Power Switch") == 0) as Family;

                FamilySymbol symbol = new FilteredElementCollector(document)
                    .WherePasses(new FamilySymbolFilter(family.Id))
                    .ToElements()
                    .FirstOrDefault(el => el.Name.CompareTo("Trissa Switch") == 0) as FamilySymbol
                    ;

                uiDocument.PostRequestForElementTypePlacement(symbol);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.Message);
            }
        }
    }
}

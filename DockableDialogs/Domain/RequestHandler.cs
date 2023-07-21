using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Visual;
using System;
using System.Collections.Generic;
using System.Linq;

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
                case RequestId.Test:
                    {
                        Test(uiapp);
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

        private void Test(UIApplication uiapp)
        {
            UIDocument uiDocument = uiapp.ActiveUIDocument;
            Document document = uiDocument.Document;
            Selection selection = uiDocument.Selection;


            List<ElementFilter> categoryFilters = new List<ElementFilter>
            {
                new ElementCategoryFilter(BuiltInCategory.OST_CommunicationDevices),
                new ElementCategoryFilter(BuiltInCategory.OST_ElectricalFixtures),
            };

            LogicalOrFilter orFilter = new LogicalOrFilter(categoryFilters);

            var collector = new FilteredElementCollector(document);
            collector.WherePasses(orFilter);
            //collector.OfClass(typeof(Family));
            //collector.OfCategory(BuiltInCategory.OST_ElectricalFixtures);
            var elList = collector.ToElements().OfType<FamilySymbol>();
            var fs = new FamilySymbol(); ElementId id = fs.Id;
            /*var f = elList.FirstOrDefault(el => el.Name.CompareTo("Power Switch") == 0) as Family;
            var ids = f.GetFamilySymbolIds();
            foreach (var id in ids)
            {
                var el = document.GetElement(id) as FamilySymbol;
                var c = el.Category.Name;
            }*/
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

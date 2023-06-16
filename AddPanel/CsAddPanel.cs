using System;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using System.Windows.Media;
using System.Linq;
//using Autodesk.Revit.Creation;

namespace AddPanel
{
    /// <remarks>
    /// This application's main class. The class must be Public.
    /// </remarks>
    public class CsAddPanel : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            string tabName = "This Tab Name";
            application.CreateRibbonTab(tabName);

            // Create a push button to trigger a command add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            // Create two push buttons
            var button1 = new PushButtonData("Button1", "My Button #1", thisAssemblyPath, "AddPanel.HelloWorld");
            var button2 = new PushButtonData("Button2", "My Button #2", thisAssemblyPath, "AddPanel.HelloWorld");

            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "NewRibbonPanel");

            // Add the buttons to the panel
            List<RibbonItem> projectButtons = new List<RibbonItem>();
            projectButtons.AddRange(ribbonPanel.AddStackedItems(button1, button2));

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            return Result.Succeeded;
        }

    }
    /// <remarks>
    /// The "HelloWorld" external command. The class must be Public.
    /// </remarks>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class HelloWorld : IExternalCommand
    {
        // The main Execute method (inherited from IExternalCommand) must be public
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //TaskDialog.Show("Revit", "Hello World");
            // Quit if active document is null
            if (null == commandData.Application.ActiveUIDocument.Document)
            {
                message = "Active document is null.";
                return Result.Failed;
            }

            UIApplication application = commandData.Application;
            UIDocument uiDocument = application.ActiveUIDocument;
            Document document = uiDocument.Document;
            Selection selection = uiDocument.Selection;
            try
            {
                /*Transaction transaction = new Transaction(document, "CreateFamilyInstance");
                transaction.Start("Lab");*/
                Family family = new FilteredElementCollector(document)
                    .OfClass(typeof(Family))
                    .ToElements()
                    .FirstOrDefault(e => e.Name.CompareTo("Power Switch") == 0) as Family;

                FamilySymbol symbol = new FilteredElementCollector(document)
                    .WherePasses(new FamilySymbolFilter(family.Id))
                    .ToElements()
                    .FirstOrDefault(e => e.Name.CompareTo("Trissa Switch") == 0) as FamilySymbol
                    ;

                uiDocument.PostRequestForElementTypePlacement(symbol);

                /*var collector = new FilteredElementCollector(document);
                var famCollector = new FilteredElementCollector(document);
                famCollector.OfClass(typeof(Family));
                ICollection<Element> collection = famCollector.ToElements();
                Family family = null;
                foreach (Element element in collection)
                {
                    if (element.Name.CompareTo("Power Switch") == 0)
                    {
                        family = element as Family;
                        break;
                    }
                }
                var fsCollector = new FilteredElementCollector(document);
                ICollection<Element> fsCollection = fsCollector.WherePasses(new FamilySymbolFilter(family.Id)).ToElements();
                foreach (Element element in fsCollection)
                {
                    if (element.Name.CompareTo("Trissa Switch") == 0)
                    {
                        FamilySymbol symbol = element as FamilySymbol;
                        //uiDocument.PromptForFamilyInstancePlacement(symbol);
                        uiDocument.PostRequestForElementTypePlacement(symbol);
                    }
                }*/
                //transaction.Commit();
            }
            catch (Exception e)
            {
                TaskDialog.Show("Revit", e.Message);
            }

            return Result.Succeeded;
        }
    }
}

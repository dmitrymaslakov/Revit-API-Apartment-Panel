using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Security.Cryptography;

namespace DockableDialogs.Domain
{
    public class RequestHandler : IExternalEventHandler
    {
        private const string TRISSA_SWITCH = "Trissa Switch";
        private const string USB = "USB";
        private const string BLOCK1 = "BLOCK1";
        private const string SINGLE_SOCKET = "Single Socket";
        private const string THROUGH_SWITCH = "Through Switch";
        private const string LAMP = "Lamp";

        private const string TELEPHONE_DEVICES = "Telephone Devices";
        private const string COMMUNICATION_DEVICES = "Communication Devices";
        private const string FIRE_ALARM_DEVICES = "Fire Alarm Devices";
        private const string LIGHTING_DEVICES = "Lighting Devices";
        private const string LIGHTING_FIXTURES = "Lighting Fixtures";
        private const string ELECTRICAL_FIXTURES = "Electrical Fixtures";

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

            string circuit = "12";
            string elementName = LAMP;
            string elementCategory = LIGHTING_FIXTURES;
            string lampSuffix = "2";
            string switchSuffixes = "";

            try
            {
                if (elementCategory.Contains("Lighting Devices"))
                {
                    switchSuffixes = GetSuffixesFromLamps(document, selection);

                    if (string.IsNullOrEmpty(switchSuffixes))
                        throw new Exception("No suffixes were found in the lamps.");
                }
                var symbol = new FilteredElementCollector(document)
                    .OfClass(typeof(FamilySymbol))
                    .Where(fs => fs.Name == LAMP)
                    .FirstOrDefault() as FamilySymbol;

                Parameter parameter = symbol.LookupParameter("RBX-CIRCUIT");
                Parameter parameter2 = symbol.LookupParameter("UK-HEIGHT");

                List<string> p = new List<string>();
                foreach (Parameter item in symbol.Parameters)
                {
                    p.Add(item.Definition.Name);
                }

                string str = string.Join(", ", p);

                TaskDialogCreater.ShowNotification(str);

                if (parameter != null && parameter.StorageType == StorageType.String)
                {
                    parameter.Set(circuit + "/" + lampSuffix);
                }
                uiDocument.PostRequestForElementTypePlacement(symbol);
            }
            catch (Exception exception)
            {
                TaskDialogCreater.ShowNotification(exception.Message);
            }
        }

        private string GetSuffixesFromLamps(Document document, Selection selection)
        {
            var collection = selection.GetElementIds()
                .Select(id => document.GetElement(id))
                .OfType<FamilyInstance>()
                .Where(fs => fs.Category.Name.Contains("Lighting Fixtures"))
                ;
            if (collection.Count() < 1)
            {
                string message =
                    "Please select the lamp(s) of Lighting Fixtures category before inserting the switch.";
                throw new Exception(message);
            }

            var circuitParameters = new List<Parameter>();

            string targetCircuitParam = "RBX-CIRCUIT";

            foreach (var lamp in collection)
            {
                var lampParameter = lamp.Parameters
                    .OfType<Parameter>()
                    .Where(p => p.Definition.Name.Contains(targetCircuitParam))
                    .FirstOrDefault();

                if (lampParameter == null)
                {
                    string message =
                        "Some of the Lighting Fixtures do not have RBX-CIRCUIT parameter.";
                    throw new Exception(message);
                }

                circuitParameters.Add(lampParameter);
            }

            var lampCircuits = new List<string>();

            foreach (Parameter parameter in circuitParameters)
                if (parameter.Definition.Name.Contains(targetCircuitParam))
                    if (parameter.StorageType == StorageType.String)
                        lampCircuits.Add(parameter.AsString());

            var suffixes = lampCircuits
                .Select(c => c.Substring(c.IndexOf("/") + 1))
                .ToList();

            return string.Join(",", suffixes);

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

    internal class TaskDialogCreater
    {
        private TaskDialog _taskDialog;

        public TaskDialog Create(string mainInstruction)
        {
            _taskDialog = new TaskDialog("Message")
            {
                MainInstruction = mainInstruction,
                CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel,
                DefaultButton = TaskDialogResult.Ok
            };
            return _taskDialog;
        }

        public TaskDialogResult Show()
        {
            return _taskDialog.Show();
        }

        public static TaskDialogResult ShowNotification(string message)
        {
            return TaskDialog.Show("Notification", message);
        }
    }
}

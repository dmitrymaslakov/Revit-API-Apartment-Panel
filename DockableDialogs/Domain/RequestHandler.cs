using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Security.Cryptography;
using Autodesk.Revit.UI.Events;
using System.Windows.Controls;
//using WindowsInput;
//using Autodesk.Revit.Creation;

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
                    //.Where(fs => fs.Name == LAMP)
                    .Where(fs => fs.Name == SINGLE_SOCKET)
                    .FirstOrDefault() as FamilySymbol;

                ElectricalFixturesCreate(uiDocument, symbol);
                //uiDocument.PromptForFamilyInstancePlacement(symbol);

                /*FamilyInstance newLamp = document.Create
                    .NewFamilyInstance(point, symbol, StructuralType.NonStructural);

                Parameter parameter = newLamp.LookupParameter("RBX-CIRCUIT");
                if (parameter != null && parameter.StorageType == StorageType.String)
                {
                    // Start a transaction to modify the parameter value
                    using (var transaction = new Transaction(document, "Write Parameter Value"))
                    {
                        transaction.Start();

                        // Set the parameter value
                        parameter.Set(circuit);

                        // Commit the transaction
                        transaction.Commit();
                    }
                }*/
                /*uiapp.Idling += OnIdling;

                uiDocument.PostRequestForElementTypePlacement(symbol);
                uiDocument.RefreshActiveView();*/

                /*Parameter parameter = symbol.LookupParameter("RBX-CIRCUIT");
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
                }*/
            }
            catch (Exception exception)
            {
                TaskDialogCreater.ShowNotification(exception.Message);
            }
        }

        private void OnIdling(object sender, IdlingEventArgs e)
        {
            var uiapp = sender as UIApplication;
            UIDocument uiDocument = uiapp.ActiveUIDocument;
            Document document = uiDocument.Document;
            Selection selection = uiDocument.Selection;

            /*ICollection<ElementId> selectedElementIds = selection.GetElementIds();
            string parameterName = "RBX-CIRCUIT";
            if (selectedElementIds.Count == 1)
            {
                ElementId instanceId = selectedElementIds.First();
                Element instanceElement = uiDocument.Document.GetElement(instanceId);

                if (instanceElement is FamilyInstance familyInstance)
                {
                    // Set the parameter value for the specific FamilyInstance
                    Parameter parameter = familyInstance.LookupParameter(parameterName);
                    if (parameter != null && !parameter.IsReadOnly)
                    {
                        using (Transaction transaction = new Transaction(uiDocument.Document, "Modify Parameter"))
                        {
                            if (transaction.Start() == TransactionStatus.Started)
                            {
                                parameter.Set("parameterValue");
                                transaction.Commit();
                            }
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Parameter Not Found or Read-Only", $"Parameter '{parameterName}' not found or is read-only.");
                    }
                }
            }*/
            uiapp.Idling -= OnIdling;
        }

        private void ElectricalFixturesCreate(UIDocument uiDocument, FamilySymbol symbol)
        {
            Document document = uiDocument.Document;
            Reference reference = uiDocument.Selection
                .PickObject(ObjectType.PointOnElement, "Pick a host in the model");
            var levelId = ViewLevel(uiDocument);

            using (var tr = new Transaction(document, "New ElectricalFixture"))
            {
                tr.Start();
                XYZ dir = new XYZ(0, 0, 0);
                FamilyInstance newElectricalFixture = uiDocument
                    .Document.Create
                    .NewFamilyInstance(reference, reference.GlobalPoint, dir, symbol);
                newElectricalFixture
                    .get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM)
                    .Set(levelId);
                document.GetUnits().GetFormatOptions(SpecTypeId.Length);

                double newElevationFeets = UnitUtils.ConvertToInternalUnits(40.0, 
                    document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

                double newElevationMeters = UnitUtils.ConvertFromInternalUnits(40.0,
                    document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

                newElectricalFixture
                    .get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM)
                    .Set(newElevationFeets);

                newElectricalFixture.LookupParameter("RBX-CIRCUIT").Set("circuit");
                tr.Commit();
            }
        }

        private ElementId ViewLevel(UIDocument uiDocument)
        {
            Document doc = uiDocument.Document;
            var active = doc.ActiveView;
            ElementId levelId = null;
            Parameter level = active.LookupParameter("Associated Level");
            FilteredElementCollector lvlCollector = new FilteredElementCollector(doc);
            ICollection<Element> lvlCollection = lvlCollector.OfClass(typeof(Level)).ToElements();
            foreach (Element l in lvlCollection)
            {
                Level lvl = l as Level;
                if (lvl.Name == level.AsString())
                {
                    levelId = lvl.Id;
                }
            }
            return levelId;
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

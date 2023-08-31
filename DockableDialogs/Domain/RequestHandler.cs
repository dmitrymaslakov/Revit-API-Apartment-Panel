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
using DockableDialogs.Utility;
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
        public object Props { get; set; }

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
                        InsertElement(uiapp);
                        break;
                    }
                case RequestId.Test:
                    {
                        //Test(uiapp);
                        InsertElement(uiapp);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            return;
        }

        private void InsertElement(UIApplication uiapp)
        {
            
            UIDocument uiDocument = uiapp.ActiveUIDocument;
            Document document = uiDocument.Document;
            Selection selection = uiDocument.Selection;
            try
            {
                var elementData = Props as Dictionary<string, string>;
                string elementName = elementData[nameof(elementName)];
                string elementCategory = elementData[nameof(elementCategory)];
                string circuit = elementData[nameof(circuit)];
                string switchHeight = elementData[nameof(switchHeight)];
                string socketHeight = elementData[nameof(socketHeight)];
                string lampSuffix = elementData[nameof(lampSuffix)];

                switch (elementCategory)
                {
                    case StaticData.LIGHTING_FIXTURES:
                        new FamilyInstanceBuilder(uiapp)
                            .WithCircuit(circuit)
                            .WithCurrentLevel()
                            .WithLampSuffix(lampSuffix)
                            .Build(elementName);
                        break;
                    case StaticData.LIGHTING_DEVICES:
                        new FamilyInstanceBuilder(uiapp)
                            .WithElevationFromLevel(switchHeight)
                            .WithCircuit(circuit)
                            .WithSwitchSuffixes()
                            .WithCurrentLevel()
                            .Build(elementName);
                        break;
                    case StaticData.ELECTRICAL_FIXTURES:
                    case StaticData.TELEPHONE_DEVICES:
                    case StaticData.FIRE_ALARM_DEVICES:
                    case StaticData.COMMUNICATION_DEVICES:
                        new FamilyInstanceBuilder(uiapp)
                            .WithElevationFromLevel(socketHeight)
                            .WithCircuit(circuit)
                            .WithCurrentLevel()
                            .Build(elementName);
                        break;
                    default:
                        break;
                }
                /*string elementName = elementData[nameof(elementName)];
                string elementCategory = elementData[nameof(elementCategory)];
                string switchSuffixes = "";

                if (elementCategory.Contains(LIGHTING_DEVICES))
                {
                    switchSuffixes = GetSuffixesFromLamps(document, selection);

                    if (string.IsNullOrEmpty(switchSuffixes))
                        throw new Exception("No suffixes were found in the lamps.");

                    elementData.Add(nameof(switchSuffixes), switchSuffixes);
                }

                var familySymbol = new FilteredElementCollector(document)
                    .OfClass(typeof(FamilySymbol))
                    .Where(fs => fs.Name == elementName)
                    .FirstOrDefault() as FamilySymbol;

                ElementId newFamilyInstanceId = FamilyInstanceCreate(uiDocument, familySymbol);
                FamilyInstanceConfigure(uiDocument, newFamilyInstanceId, elementData);*/

                Props = null;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
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
                    //.Where(fs => fs.Name == SINGLE_SOCKET)
                    .Where(fs => fs.Name == THROUGH_SWITCH)
                    .FirstOrDefault() as FamilySymbol;
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


        private void FamilyInstanceConfigure(UIDocument uiDocument,
            ElementId familyInstanceId, Dictionary<string, string> elementData)
        {
            Document document = uiDocument.Document;
            FamilyInstance familyInstance = document.GetElement(familyInstanceId) as FamilyInstance;
            var levelId = GetViewLevel(uiDocument);

            string circuit = elementData[nameof(circuit)];
            //string elementName = elementData[nameof(elementName)];
            //string elementCategory = elementData[nameof(elementCategory)];
            string lampSuffix = elementData[nameof(lampSuffix)];
            string switchSuffixes = "";
            bool isElevationParsed = double.TryParse(elementData["elevationFromLevel"],
                out double elevationFromLevel);
            if (familyInstance.Category.Name.Contains(LIGHTING_DEVICES))
                switchSuffixes = elementData[nameof(switchSuffixes)];

            using (var tr = new Transaction(document, "Config FamilyInstance"))
            {
                tr.Start();
                XYZ dir = new XYZ(0, 0, 0);
                var category = familyInstance.Category.Name;
                if (levelId != null)
                {
                    familyInstance
                        .get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM)
                        .Set(levelId);
                    if (!category.Contains(LIGHTING_FIXTURES))
                    {
                        double newElevationFeets = UnitUtils.ConvertToInternalUnits(elevationFromLevel,
                            document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

                        familyInstance
                            .get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM)
                            .Set(newElevationFeets);
                    }
                }
                Parameter circuitParam = familyInstance.LookupParameter("RBX-CIRCUIT");
                switch (category)
                {
                    case LIGHTING_FIXTURES:
                        circuitParam.Set(circuit + "/" + lampSuffix);
                        break;
                    case LIGHTING_DEVICES:
                        circuitParam.Set(circuit + "/" + switchSuffixes);
                        break;
                    case ELECTRICAL_FIXTURES:
                    case TELEPHONE_DEVICES:
                    case FIRE_ALARM_DEVICES:
                    case COMMUNICATION_DEVICES:
                        circuitParam.Set(circuit);
                        break;
                }
                tr.Commit();
            }
        }

        private void LightingFixturesCreate(UIDocument uiDocument, FamilySymbol symbol)
        {
            Document document = uiDocument.Document;
            Reference reference = uiDocument.Selection
                .PickObject(ObjectType.PointOnElement, "Pick a host in the model");
            var levelId = GetViewLevel(uiDocument);

            using (var tr = new Transaction(document, "New LightingFixture"))
            {
                tr.Start();
                XYZ dir = new XYZ(0, 0, 0);
                FamilyInstance newLightingFixtures = uiDocument
                    .Document.Create
                    .NewFamilyInstance(reference, reference.GlobalPoint, dir, symbol);
                if (levelId != null)
                {
                    newLightingFixtures
                        .get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM)
                        .Set(levelId);
                }
                newLightingFixtures.LookupParameter("RBX-CIRCUIT").Set("circuit");
                tr.Commit();
            }
        }

        private ElementId GetViewLevel(UIDocument uiDocument)
        {
            Document doc = uiDocument.Document;

            var active = doc.ActiveView;
            ElementId levelId = null;
            Parameter level = active.LookupParameter("Associated Level");
            if (level == null)
                return null;

            FilteredElementCollector lvlCollector = new FilteredElementCollector(doc);
            ICollection<Element> lvlCollection = lvlCollector.OfClass(typeof(Level)).ToElements();
            foreach (Element l in lvlCollection)
            {
                Level lvl = l as Level;
                if (lvl.Name == level.AsString())
                    levelId = lvl.Id;
            }
            return levelId;
        }

        private string GetSuffixesFromLamps(Document document, Selection selection)
        {
            //try
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
            /*catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
                return null;
            }*/
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

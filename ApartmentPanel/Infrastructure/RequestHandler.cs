using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using ApartmentPanel.Utility;
using ApartmentPanel.Presentation.View.Components;
using System.Linq;
using ApartmentPanel.Core.Models;
using System.Collections.ObjectModel;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;

namespace ApartmentPanel.Infrastructure
{
    public class RequestHandler : IExternalEventHandler
    {
        public Request Request { get; } = new Request();
        public object Props { get; set; }

        public UIApplication Uiapp { get; set; }
        public UIDocument UiDocument { get; set; }
        public Document Document { get; set; }
        public Selection Selection { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                Uiapp = uiapp;
                UiDocument = Uiapp.ActiveUIDocument;
                Document = UiDocument.Document;
                Selection = UiDocument.Selection;

                switch (Request.Take())
                {
                    case RequestId.None:
                        {
                            return;
                        }
                    case RequestId.Configure:
                        {
                            ShowConfigPanel();
                            break;
                        }
                    case RequestId.Insert:
                        {
                            InsertElement();
                            break;
                        }
                    case RequestId.AddElement:
                        {
                            AddElement();
                            break;
                        }
                    case RequestId.EditElement:
                        {
                            break;
                        }
                    case RequestId.RemoveElement:
                        {
                            break;
                        }
                    case RequestId.AddCircuit:
                        {
                            break;
                        }
                    case RequestId.EditCircuit:
                        {
                            break;
                        }
                    case RequestId.RemoveCircuit:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show($"Exception", ex.Message);
            }
        }

        public string GetName() => "Placement Apartment elements";

        private void ShowConfigPanel()
        {
            var editPanelVM = Props as ConfigPanelVM;
            new ConfigPanel(editPanelVM).Show();
            Props = null;
        }

        private void AddElement()
        {
            var addElement = Props as Action<ApartmentElement>;
            IEnumerable<CategorizedFamilySymbols> categorizedElements = CategorizeElements();
            new ListElements(addElement, categorizedElements).ShowDialog();
            Props = null;
        }

        private IEnumerable<CategorizedFamilySymbols> CategorizeElements()
        {
            // Create a new FilteredElementCollector and filter by FamilySymbol class
            var collector = new FilteredElementCollector(Document).OfClass(typeof(FamilySymbol));

            // Create a list of BuiltInCategory enums for the categories you want to filter by
            List<BuiltInCategory> categories = new List<BuiltInCategory>
            {
                BuiltInCategory.OST_TelephoneDevices,
                BuiltInCategory.OST_CommunicationDevices,
                BuiltInCategory.OST_FireAlarmDevices,
                BuiltInCategory.OST_LightingDevices,
                BuiltInCategory.OST_LightingFixtures,
                BuiltInCategory.OST_ElectricalFixtures
            };

            // Create a list of CategoryFilters, one for each category
            List<ElementFilter> categoryFilters = categories
                .Select(c => new ElementCategoryFilter(c))
                .Cast<ElementFilter>()
                .ToList();

            // Combine the category filters using a LogicalOrFilter
            LogicalOrFilter orFilter = new LogicalOrFilter(categoryFilters);
            // Apply the filter to the collector
            collector.WherePasses(orFilter);

            // Get the filtered FamilySymbols
            List<FamilySymbol> familySymbols = collector
                .ToElements()
                .Cast<FamilySymbol>()
                .ToList();

            return GetCategorizedElements(familySymbols);
        }

        private IEnumerable<CategorizedFamilySymbols> GetCategorizedElements(List<FamilySymbol> allElements)
        {
            return allElements
            .GroupBy(fs => fs.Category.Name)
            .Select(gfs => new CategorizedFamilySymbols
            {
                Category = gfs.Key,
                CategorizedElements =
                new ObservableCollection<ApartmentElement>(gfs.Select(fs => new ApartmentElement { Name = fs.Name, Category = fs.Category.Name }))
            }).ToList();
        }

        private void InsertElement()
        {
            var elementData = Props as Dictionary<string, string>;
            string elementName = elementData[nameof(elementName)];
            string elementCategory = elementData[nameof(elementCategory)];
            string circuit = elementData[nameof(circuit)];
            string height = elementData[nameof(height)];
            string lampSuffix = elementData[nameof(lampSuffix)];
            string insertingMode = elementData[nameof(insertingMode)];
            bool isMultiple = true;
            List<FamilyInstance> lamps = null;

            if (elementCategory.Contains(StaticData.LIGHTING_DEVICES))
                lamps = PickLamp();

            while (isMultiple)
            {
                try
                {
                    switch (elementCategory)
                    {
                        case StaticData.LIGHTING_FIXTURES:
                            new FamilyInstanceBuilder(Uiapp)
                                .WithCircuit(circuit)
                                .WithCurrentLevel()
                                .WithLampSuffix(lampSuffix)
                                .Build(elementName);
                            break;
                        case StaticData.LIGHTING_DEVICES:
                            new FamilyInstanceBuilder(Uiapp)
                                .WithSwitchSuffixes(lamps)
                                .WithElevationFromLevel(height)
                                .WithCircuit(circuit)
                                .WithCurrentLevel()
                                .Build(elementName);
                            break;
                        case StaticData.ELECTRICAL_FIXTURES:
                        case StaticData.TELEPHONE_DEVICES:
                        case StaticData.FIRE_ALARM_DEVICES:
                        case StaticData.COMMUNICATION_DEVICES:
                            new FamilyInstanceBuilder(Uiapp)
                                .WithElevationFromLevel(height)
                                .WithCircuit(circuit)
                                .WithCurrentLevel()
                                .Build(elementName);
                            break;
                        default:
                            break;
                    }
                    if (!insertingMode.Contains("multiple")) isMultiple = false;
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    isMultiple = false;
                }
            }
            Props = null;
        }

        private List<FamilyInstance> PickLamp()
        {
            var collection = Selection.GetElementIds()
                            .Select(id => Document.GetElement(id))
                            .OfType<FamilyInstance>()
                            .Where(fs => fs.Category.Name.Contains("Lighting Fixtures"))
                            .ToList()
                            ;
            if (collection.Count() < 1)
            {
                string message =
                    "Please select the lamp(s) of Lighting Fixtures category before inserting the switch.";
                throw new Exception(message);
            }
            return collection;
        }
    }

    class LightingDeviceSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            // Only allow elements in the Lighting Devices category
            return elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_LightingDevices;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            // Not used in this example
            return false;
        }
    }
}

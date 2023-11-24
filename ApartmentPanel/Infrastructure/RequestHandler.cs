using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using ApartmentPanel.Utility;
using System.Linq;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;

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
                            AnalizeElement();
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
                    case RequestId.GetProperties:
                        {
                            //GetProperties();
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
            /*var configPanelVM = Props as ConfigPanelViewModel;
            new ConfigPanel(configPanelVM).Show();
            Props = null;*/
        }

        private BuiltInCategory StringToBuiltInCategory(string category)
        {
            // Remove spaces and capitalize each word to match the format of the BuiltInCategory enum
            string formattedCategory = string.Concat(
                category.Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));

            // Add the "OST_" prefix
            string ostCategory = "OST_" + formattedCategory;

            // Try to parse the string as a BuiltInCategory
            if (Enum.TryParse(ostCategory, out BuiltInCategory result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Invalid category");
            }
        }

        private void AddElement()
        {
            var showListElementsPanel = Props as Action<List<(string name, string category)>>;
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

            List<(string name, string category)> familyProps = familySymbols                
                .Select(fs => (fs.Name, fs.Category.Name))
                .ToList();

            showListElementsPanel(familyProps);
            Props = null;
        }

        /*private void AddElement()
        {
            var addElement = Props as Action<IApartmentElement>;
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
                new ObservableCollection<IApartmentElement>(gfs.Select(fs => 
                    new ApartmentElement { Name = fs.Name, Category = fs.Category.Name }))
            }).ToList();
        }*/

        private void InsertElement()
        {
            var elementData = Props as InsertElementDTO;
            /*string elementName = elementData[nameof(elementName)];
            string elementCategory = elementData[nameof(elementCategory)];
            string circuit = elementData[nameof(circuit)];
            string height = elementData[nameof(height)];
            string lampSuffix = elementData[nameof(lampSuffix)];
            string insertingMode = elementData[nameof(insertingMode)];*/
            bool isMultiple = true;
            List<FamilyInstance> lamps = null;

            if (elementData.Category.Contains(StaticData.LIGHTING_DEVICES))
                lamps = PickLamp();

            while (isMultiple)
            {
                try
                {
                    switch (elementData.Category)
                    {
                        case StaticData.LIGHTING_FIXTURES:
                            new FamilyInstanceBuilder(Uiapp)
                                .WithCircuit(elementData.Circuit)
                                .WithCurrentLevel()
                                .WithLampSuffix(elementData.CurrentSuffix)
                                .Build(elementData.Name);
                            break;
                        case StaticData.LIGHTING_DEVICES:
                            new FamilyInstanceBuilder(Uiapp)
                                .WithSwitchSuffixes(lamps)
                                .WithHeight(elementData.Height, elementData.TypeOfHeight)
                                .WithCircuit(elementData.Circuit)
                                .WithCurrentLevel()
                                .Build(elementData.Name);
                            break;
                        case StaticData.ELECTRICAL_FIXTURES:
                        case StaticData.TELEPHONE_DEVICES:
                        case StaticData.FIRE_ALARM_DEVICES:
                        case StaticData.COMMUNICATION_DEVICES:
                            new FamilyInstanceBuilder(Uiapp)
                                .WithHeight(elementData.Height, elementData.TypeOfHeight)
                                .WithCircuit(elementData.Circuit)
                                .WithCurrentLevel()
                                .Build(elementData.Name);
                            break;
                        default:
                            break;
                    }
                    if (!elementData.InsertingMode.Contains("multiple")) isMultiple = false;
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

        private void AnalizeElement()
        {
            var selectedElementId = Selection.GetElementIds().FirstOrDefault();
            if (selectedElementId == null)
            {
                // No element is selected
                return;
            }

            Element selectedElement = Document.GetElement(selectedElementId);
            var familyInstance = selectedElement as FamilyInstance;
            var lp = familyInstance.Location as LocationPoint;
            var pointFeets = lp.Point;
            var zCm = UnitUtils.ConvertFromInternalUnits(pointFeets.Z,
                Document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());
            var bb = familyInstance.get_BoundingBox(null);
            var pointMax = bb.Max;
            var pointMin = bb.Min;
            var zMaxCm = UnitUtils.ConvertFromInternalUnits(pointMax.Z,
                Document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());
            var zMinCm = UnitUtils.ConvertFromInternalUnits(pointMin.Z,
                Document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());
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

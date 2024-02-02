using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using ApartmentPanel.Utility;
using System.Linq;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Infrastructure.Models;
using ApartmentPanel.Utility.Exceptions;
using System.Text;
using System.Diagnostics;

namespace ApartmentPanel.Infrastructure
{
    public class RequestHandler : RevitInfrastructureBase, IExternalEventHandler
    {
        public Request Request { get; } = new Request();
        public object Props { get; set; }

        /*public UIApplication _uiapp { get; set; }
        public UIDocument _uiDocument { get; set; }
        public Document _document { get; set; }
        public Selection _selection { get; set; }*/

        public void Execute(UIApplication uiapp)
        {
            try
            {
                _uiapp = uiapp;
                _uiDocument = _uiapp.ActiveUIDocument;
                _document = _uiDocument.Document;
                _selection = _uiDocument.Selection;

                switch (Request.Take())
                {
                    case RequestId.None:
                        {
                            AnalizeElement(uiapp);
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
                    case RequestId.BatchInsert:
                        {
                            InsertBatch();
                            break;
                        }
                    case RequestId.AddElement:
                        {
                            AddElement();
                            break;
                        }
                    case RequestId.SettingParameters:
                        {
                            SetParameters();
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

        private void SetParameters()
        {
            var setParamsDTO = Props as SetParamsDTO;

            bool isInstanceExist = 
                TryGetInstance(_document, setParamsDTO.ElementName, out FamilyInstance element);

            if (!isInstanceExist) return;
            var element2 = new FilteredElementCollector(_document)
                .OfClass(typeof(FamilySymbol))
                .Where(fs => fs.Name == setParamsDTO.ElementName)
                .FirstOrDefault() as FamilyInstance;

            List<string> parameters = new List<string>();
            ParameterSet parameterSet = element.Parameters;
            ParameterMap parameterMap = element.ParametersMap;
            foreach (Parameter param in parameterSet)
            {
                if (param == null) continue;
                parameters.Add(param.Definition.Name);
            }
            setParamsDTO.SetInstanceParameters(parameters);
            Props = null;
        }

        private bool TryGetInstance(Document document, string elementName, out FamilyInstance element)
        {
            element = new FilteredElementCollector(document)
                .OfClass(typeof(FamilyInstance))                
                .ToElements()
                .Select(e => e as FamilyInstance)
                .FirstOrDefault(fi => string.Equals(fi.Name, elementName));

            return !(element is null);
        }

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
            var collector = new FilteredElementCollector(_document).OfClass(typeof(FamilySymbol));
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

        private void InsertBatch()
        {
            var batchData = Props as InsertBatchDTO;
            new BatchInstaller(_uiapp, batchData).Install();
        }

        private void InsertElement()
        {
            var elementData = Props as InsertElementDTO;
            bool isMultiple = true;

            if (elementData.Category.Contains(StaticData.LIGHTING_DEVICES))
            {
                List<FamilyInstance> lamps = PickLamp();
                elementData.SwitchNumbers = lamps.Count == 0
                    ? ""
                    : GetNumbersFromLamps(lamps);
            }

            while (isMultiple)
            {
                try
                {
                    FamilyInstanceBuilder instanceBuilder = null;
                    BuiltInstance builtInstance = null;
                    switch (elementData.Category)
                    {
                        case StaticData.LIGHTING_FIXTURES:
                            instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                            builtInstance = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                                .InstallLightingFixtures();
                            break;
                        case StaticData.LIGHTING_DEVICES:
                            instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                            builtInstance = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                                .InstallLightingDevices();
                            break;
                        case StaticData.ELECTRICAL_FIXTURES:
                            instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                            builtInstance = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                                .InstallElectricalFixtures();
                            break;
                        case StaticData.TELEPHONE_DEVICES:
                        case StaticData.FIRE_ALARM_DEVICES:
                        case StaticData.COMMUNICATION_DEVICES:
                            instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                            builtInstance = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                                .InstallCommunicationDevices();
                            break;
                        default:
                            break;
                    }
                    if (builtInstance != null) _selection.SetElementIds(new List<ElementId> { builtInstance.Id });
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
            var collection = _selection.GetElementIds()
                            .Select(id => _document.GetElement(id))
                            .OfType<FamilyInstance>()
                            .Where(fs => fs.Category.Name.Contains("Lighting Fixtures"))
                            .ToList()
                            ;
            /*if (collection.Count() < 1)
            {
                string message =
                    "Please select the lamp(s) of Lighting Fixtures category before inserting the switch.";
                throw new Exception(message);
            }*/
            return collection;
        }

        private string GetNumbersFromLamps(List<FamilyInstance> lamps)
        {
            var circuitParameters = new List<Parameter>();

            string targetCircuitParam = StaticData.ELEMENT_CIRCUIT_PARAM_NAME;

            foreach (var lamp in lamps)
            {
                var lampParameter = lamp.Parameters
                    .OfType<Parameter>()
                    .Where(p => p.Definition.Name.Contains(targetCircuitParam))
                    .FirstOrDefault();

                if (lampParameter == null)
                    throw new CustomParameterException(targetCircuitParam, lamp.Name);

                circuitParameters.Add(lampParameter);
            }

            var lampCircuits = new List<string>();

            foreach (Parameter parameter in circuitParameters)
                if (parameter.Definition.Name.Contains(targetCircuitParam))
                    if (parameter.StorageType == StorageType.String)
                        lampCircuits.Add(parameter.AsString());

            var suffixes = lampCircuits
                .Select(c => c.Substring(c.IndexOf("/") + 1))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return string.Join(",", suffixes);
        }

        private void AnalizeElement(UIApplication uiapp)
        {
            UIApplication _uiapp = uiapp;
            UIDocument _uiDocument = _uiapp.ActiveUIDocument;
            Document _document = _uiDocument.Document;
            Selection _selection = _uiDocument.Selection;

            try
            {
                ElementCategoryFilter categoryFilter1 = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalFixtures);
                ElementCategoryFilter categoryFilter2 = new ElementCategoryFilter(BuiltInCategory.OST_LightingDevices);
                ElementCategoryFilter categoryFilter3 = new ElementCategoryFilter(BuiltInCategory.OST_LightingFixtures);
                ElementCategoryFilter categoryFilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CommunicationDevices);
                //LogicalOrFilter orFilter = new LogicalOrFilter(categoryFilter1, categoryFilter2);
                LogicalOrFilter orFilter = new LogicalOrFilter(new List<ElementFilter>
                {
                    categoryFilter1, categoryFilter2, categoryFilter3, categoryFilter4
                });
                var instances = new FilteredElementCollector(_document, _document.ActiveView.Id)
                //var instances = new FilteredElementCollector(_document)
                    .WherePasses(orFilter)
                    .OfClass(typeof(FamilyInstance))
                    .ToElements()
                    .Where(fi => TryFindParameterValue(fi))
                    .Select(e => e.Id)
                    ;
                Debug.Write($"instances - {instances.Count()}");
                _selection.SetElementIds(new List<ElementId>(instances));
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Analize_Exception", ex.Message);
            }
        }
        private bool TryFindParameterValue(Element instance)
        {
            //string parameterName = "Elevation from Level";
            string parameterName = "H-UK";
            Parameter parameter = instance.LookupParameter(parameterName);
            if (parameter == null) throw new ArgumentNullException(parameterName);
            //Debug.Write(parameter.AsString().Contains("OK=40"));
            //return Math.Round(parameter.AsDouble(), 2) == 0.0;
            return parameter.AsString().Contains("OK=40");
        }

        private XYZ FeetToCm(XYZ feetPoint)
        {
            double x = UnitUtils.ConvertFromInternalUnits(feetPoint.X,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());
            double y = UnitUtils.ConvertFromInternalUnits(feetPoint.Y,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());
            double z = UnitUtils.ConvertFromInternalUnits(feetPoint.Z,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            return new XYZ(x, y, z);
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

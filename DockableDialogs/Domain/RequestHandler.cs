using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using DockableDialogs.Utility;
using DockableDialogs.ViewModel.ComponentsVM;
using DockableDialogs.View.Components;
using System.Linq;
using System.Windows.Controls;

namespace DockableDialogs.Domain
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
                        ShowEditPanel();
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

            return;
        }

        public string GetName()
        {
            return "Placement Apartment elements";
        }
        
        private void ShowEditPanel()
        {
            var editPanelVM = Props as EditPanelVM;
            new EditPanel(editPanelVM).Show();
            Props = null;
        }

        private void AddElement()
        {
            var addElement = Props as Action<FamilySymbol>;
            List<FamilySymbol> categorizedElements = CategorizeElements();
            new ListElements(addElement, categorizedElements).ShowDialog();
            Props = null;
        }

        private List<FamilySymbol> CategorizeElements()
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

            return familySymbols;
        }

        private void InsertElement()
        {            
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
                        new FamilyInstanceBuilder(Uiapp)
                            .WithCircuit(circuit)
                            .WithCurrentLevel()
                            .WithLampSuffix(lampSuffix)
                            .Build(elementName);
                        break;
                    case StaticData.LIGHTING_DEVICES:
                        new FamilyInstanceBuilder(Uiapp)
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
                        new FamilyInstanceBuilder(Uiapp)
                            .WithElevationFromLevel(socketHeight)
                            .WithCircuit(circuit)
                            .WithCurrentLevel()
                            .Build(elementName);
                        break;
                    default:
                        break;
                }
                Props = null;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }
    }
}

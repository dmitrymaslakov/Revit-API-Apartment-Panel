using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApartmentPanel.Infrastructure.Handler.HandlerStates
{
    internal class AddElementHandlerState : HandlerState
    {
        internal override void Handle(UIApplication uiapp)
        {
            SetInfrastructure(uiapp);
            AddElement();
        }
        private void AddElement()
        {
            var showListElementsPanel = _handler.Props as Action<List<(string name, string category)>>;
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
            _handler.Props = null;
        }
    }
}

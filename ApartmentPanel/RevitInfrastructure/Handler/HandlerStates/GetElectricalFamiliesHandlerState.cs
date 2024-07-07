using Autodesk.Revit.UI;
using System;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using ApartmentPanel.RevitInfrastructure.Handler.HandlerStates;
using ApartmentPanel.UseCases.ElectricalElements.Dto;
using ApartmentPanel.UseCases.ApartmentElements.Dto;

namespace ApartmentPanel.RevitInfrastructure.Handler
{
    internal class GetElectricalFamiliesHandlerState 
        : HandlerState<object, ICollection<ElectricalElementDto>>
    {
        public GetElectricalFamiliesHandlerState(UIApplication uiapp) : base(uiapp) { }

        internal override ICollection<ElectricalElementDto> Handle(object parameter)
        {
            try
            {
                var collector = new FilteredElementCollector(_document).OfClass(typeof(FamilySymbol));
                // Create a list of BuiltInCategory enums for the categories you want to filter by
                List<BuiltInCategory> categories = new List<BuiltInCategory>
                {
                    BuiltInCategory.OST_TelephoneDevices,
                    BuiltInCategory.OST_CommunicationDevices,
                    BuiltInCategory.OST_FireAlarmDevices,
                    BuiltInCategory.OST_LightingDevices,
                    BuiltInCategory.OST_LightingFixtures,
                    BuiltInCategory.OST_ElectricalFixtures,
                    BuiltInCategory.OST_ElectricalEquipment
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

                ICollection<ElectricalElementDto> familyProps = familySymbols
                    .Select(fs => new ElectricalElementDto
                    {
                        Name = fs.Name,
                        Category = fs.Category.Name,
                        Family = fs.FamilyName
                    })
                    .ToList();
                return familyProps;
            }
            catch (Exception ex)
            {
                TaskDialog.Show($"Exception", ex.Message);
                return null;
            }
        }
    }
}

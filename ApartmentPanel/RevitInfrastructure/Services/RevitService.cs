using ApartmentPanel.Infrastructure.Interfaces.Services;
using ApartmentPanel.RevitInfrastructure.Handler;
using ApartmentPanel.UseCases.ApartmentElements.Dto;
using ApartmentPanel.UseCases.ElectricalElements.Dto;
using ApartmentPanel.Utility;
using Revit.Async;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApartmentPanel.RevitInfrastructure.Services
{
    internal class RevitService : RevitInfrastructureBase, ICadServices
    {
        public async Task<ICollection<ElectricalElementDto>> GetElectricalElementsAsync()
        {
            return await RevitTask.RunAsync(app =>
            {
                int? e = null;
                var handler = new RevitHandler<object, ICollection<ElectricalElementDto>>();
                handler.SetState(new GetElectricalFamiliesHandlerState(app));
                return handler.Execute(null);
            });
           /*return await RevitTask.RunAsync(app =>
            {
                SetInfrastructure(app);
                var _document = app.ActiveUIDocument;
                var collector = new FilteredElementCollector(_document.Document).OfClass(typeof(FamilySymbol));
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

                ICollection<ElectricalFamilyDto> familyProps = familySymbols
                    .Select(fs => new ElectricalFamilyDto
                    {
                        Name = fs.Name,
                        Category = fs.Category.Name,
                        Family = fs.FamilyName
                    })
                    .ToList();
                return familyProps;
            });*/
        }
    }
}

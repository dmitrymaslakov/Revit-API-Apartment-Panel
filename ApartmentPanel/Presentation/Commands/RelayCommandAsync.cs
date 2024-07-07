using Autodesk.Revit.UI;
using System;
using System.Threading.Tasks;

namespace ApartmentPanel.Presentation.Commands
{
    public class RelayCommandAsync : BaseCommand
    {
        private readonly Func<object, Task> _execute;

        public RelayCommandAsync(Func<object, Task> execute) =>
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

        public override async void Execute(object parameter)
        {
            try
            {
                await _execute(parameter);
                /*var r = await RevitTask.RunAsync(app =>
                {
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

                    List<(string name, string category, string family)> familyProps = familySymbols
                        .Select(fs => (fs.Name, fs.Category.Name, fs.FamilyName))
                        .ToList();
                    return familyProps;
                });*/
            }
            catch (Exception ex)
            {
                TaskDialog.Show("RelayCommand_Exception", ex.Message);
            }
        }
    }
}

using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Utility;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentPanel.Infrastructure.Repositories
{
    public class ApartmentElementRepository : IApartmentElementRepository
    {
        public void InsertElement(UIApplication Uiapp, object Props)
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
                lamps = PickLamp(Uiapp.ActiveUIDocument.Selection, Uiapp.ActiveUIDocument.Document);

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
        private List<FamilyInstance> PickLamp(Selection Selection, Document Document)
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
}

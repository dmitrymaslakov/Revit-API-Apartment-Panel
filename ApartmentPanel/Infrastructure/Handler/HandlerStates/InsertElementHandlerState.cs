using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Infrastructure.Handler.HandlerStates;
using ApartmentPanel.Infrastructure.Models;
using ApartmentPanel.Utility;
using ApartmentPanel.Utility.Exceptions;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace ApartmentPanel.Infrastructure.Handler.HandlerStates
{
    internal class InsertElementHandlerState : HandlerState
    {
        internal override void Handle(UIApplication uiapp)
        {
            SetInfrastructure(uiapp);
            InsertElement();
        }
        private void InsertElement()
        {
            var elementData = _handler.Props as InsertElementDTO;
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
                    if (builtInstance != null)
                        _selection.SetElementIds(new List<ElementId> { builtInstance.Id });
                    else
                    {
                        TaskDialog.Show("Insert error", $"{elementData.Name} doesn't exist in the model");
                        return;
                    }

                    if (!elementData.InsertingMode.Contains("multiple")) isMultiple = false;
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    isMultiple = false;
                }
            }
            _handler.Props = null;
        }
        private List<FamilyInstance> PickLamp()
        {
            var collection = _selection.GetElementIds()
                            .Select(id => _document.GetElement(id))
                            .OfType<FamilyInstance>()
                            .Where(fs => fs.Category.Name.Contains("Lighting Fixtures"))
                            .ToList()
                            ;
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
    }
}

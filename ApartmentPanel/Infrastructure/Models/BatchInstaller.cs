using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Utility;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace ApartmentPanel.Infrastructure.Models
{
    public class BatchInstaller : RevitInfrastructureBase
    {
        private readonly InsertBatchDTO _batch;

        public BatchInstaller(UIApplication uiapp, InsertBatchDTO batch)
            : base(uiapp) => _batch = batch;

        public void Install()
        {
            var batchedInstances = new BatchedInstanceRow(_uiapp);
            Reference host = null;
            ElementId id = null;
            FamilyInstanceBuilder instanceBuilder = null;

            foreach (var elementData in _batch.BatchedElements)
            {
                if (_batch.BatchedElements.IndexOf(elementData) > 0)
                    elementData.Offset = batchedInstances.GetOffset();

                switch (elementData.Category)
                {
                    case StaticData.LIGHTING_FIXTURES:
                        instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                        id = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                            .InstallLightingFixtures(host);
                        break;
                    case StaticData.LIGHTING_DEVICES:
                        instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                        id = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                            .InstallLightingDevices(host);
                        break;
                    case StaticData.ELECTRICAL_FIXTURES:
                        instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                        id = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                            .InstallElectricalFixtures(host);
                        break;
                    case StaticData.TELEPHONE_DEVICES:
                    case StaticData.FIRE_ALARM_DEVICES:
                    case StaticData.COMMUNICATION_DEVICES:
                        instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                        id = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                            .InstallCommunicationDevices(host);
                        break;
                    default:
                        break;
                }
                if (id != null)
                {
                    var instance = _document.GetElement(id) as FamilyInstance;
                    batchedInstances.Add(new BatchedInstance()
                    {
                        Instance = instance,
                        Margin = elementData.Margin,
                    });

                    if (_batch.BatchedElements.IndexOf(elementData) == 0 && instanceBuilder != null)
                        host = instanceBuilder.Host;
                }
            }
        }

        private double GetValueFromLocationToInstanceEdge(FamilyInstance familyInstance)
        {
            XYZ basePoint = (familyInstance.Location as LocationPoint)?.Point;
            XYZ maxPoint = familyInstance.get_BoundingBox(null).Max;
            return Math.Abs(basePoint.X - maxPoint.X);
        }
    }
}

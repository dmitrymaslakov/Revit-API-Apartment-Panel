﻿using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
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
            BuiltInstance builtInstance = null;
            FamilyInstanceBuilder instanceBuilder = null;
            List<ElementId> instanceIds = new List<ElementId>();
            foreach (var elementData in _batch.BatchedElements)
            {
                //if (_batch.BatchedElements.IndexOf(elementData) > 0)
                if (!IsElementStartNewRow(elementData))
                    elementData.Offset = batchedInstances.GetOffset();

                switch (elementData.Category)
                {
                    case StaticData.LIGHTING_FIXTURES:
                        instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                        builtInstance = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                            .InstallLightingFixtures(host);
                        break;
                    case StaticData.LIGHTING_DEVICES:
                        instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                        builtInstance = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                            .InstallLightingDevices(host);
                        break;
                    case StaticData.ELECTRICAL_FIXTURES:
                    case StaticData.ELECTRICAL_EQUIPMENT:
                        instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                        builtInstance = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                            .InstallElectricalFixtures(host);
                        break;
                    case StaticData.TELEPHONE_DEVICES:
                    case StaticData.FIRE_ALARM_DEVICES:
                    case StaticData.COMMUNICATION_DEVICES:
                        instanceBuilder = new FamilyInstanceBuilder(_uiapp);
                        builtInstance = new ElementInstaller(_uiapp, elementData, instanceBuilder)
                            .InstallCommunicationDevices(host);
                        break;
                    default:
                        break;
                }
                if (builtInstance != null)
                {
                    //var instance = _document.GetElement(builtInstance.Id) as FamilyInstance;
                    instanceIds.Add(builtInstance.Id);
                    if (batchedInstances.Count != 0 && IsElementStartNewRow(elementData)) batchedInstances.Clear();

                    batchedInstances.Add(new BatchedInstance()
                    {
                        Instance = builtInstance,
                        Margin = elementData.Margin,
                    });

                    if (_batch.BatchedElements.IndexOf(elementData) == 0 && instanceBuilder != null)
                        host = instanceBuilder.Host;
                }
                else
                {
                    TaskDialog.Show("Insert error", $"{elementData.Name} doesn't exist in the model");
                    return;
                }
            }
            _selection.SetElementIds(instanceIds);
        }

        private bool IsElementStartNewRow(InsertElementDTO element)
        {
            return element.Location.Y == 0;
        }
        private double GetValueFromLocationToInstanceEdge(FamilyInstance familyInstance)
        {
            XYZ basePoint = (familyInstance.Location as LocationPoint)?.Point;
            XYZ maxPoint = familyInstance.get_BoundingBox(null).Max;
            return Math.Abs(basePoint.X - maxPoint.X);
        }
    }
}

﻿using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Infrastructure.Models.LocationStrategies;
using ApartmentPanel.Core.Enums;
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace ApartmentPanel.Infrastructure.Models
{
    public class ElementInstaller
    {
        private readonly InsertElementDTO _elementData;
        private readonly FamilyInstanceBuilder _familyInstanceBuilder;
        private readonly ILocationStrategy _locationStrategy;
        private readonly Func<double, string> _heightFormat;
        private readonly UIApplication _uiapp;

        public ElementInstaller(UIApplication uiapp, InsertElementDTO elementData,
            FamilyInstanceBuilder familyInstanceBuilder)
        {
            _uiapp = uiapp;
            _elementData = elementData;
            _familyInstanceBuilder = familyInstanceBuilder;
            _locationStrategy = GetLocationStrategy(_elementData.Height.TypeOf);
            _heightFormat = height =>
            {
                return height.ToString();
                /*return _elementData.Height.TypeOf == TypeOfHeight.Center 
                ? $"H={height}"
                : $"{_elementData.Height.TypeOf}={height}";*/
            };
        }

        public BuiltInstance InstallLightingFixtures(Reference host = null)
        {
            return _familyInstanceBuilder
                .SetLocationStrategy(_locationStrategy)
                .WithTypeOfHeight(_elementData.Height.TypeOf)
                .WithHeight(_elementData.Height.FromFloor, _elementData.Height.FromLevel)
                .WithResponsibleForHeight(_elementData.Height.ResponsibleForHeightParameter)
                .WithFamilyName(_elementData.Family)
                //.RenderHeightAs(_heightFormat)
                .WithCircuit(_elementData.Circuit.Number)
                .WithResponsibleForCircuit(_elementData.Circuit.ResponsibleForCircuitParameter)
                .WithCurrentLevel()
                .WithCircuitSuffix(_elementData.CurrentSuffix)
                .WithHost(host)
                .WithParameters(_elementData.Parameters)
                .WithDirection(_elementData.Direction)
                .Build(_elementData.Name);
        }

        public BuiltInstance InstallLightingDevices(Reference host = null)
        {
            return _familyInstanceBuilder
                .WithSwitchNumbers(_elementData.SwitchKeys)
                .SetLocationStrategy(_locationStrategy)
                .WithTypeOfHeight(_elementData.Height.TypeOf)
                .WithHeight(_elementData.Height.FromFloor, _elementData.Height.FromLevel)
                .WithResponsibleForHeight(_elementData.Height.ResponsibleForHeightParameter)
                .WithFamilyName(_elementData.Family)
                //.RenderHeightAs(_heightFormat)
                .WithCircuit(_elementData.Circuit.Number)
                .WithCircuitSuffix(_elementData.CurrentSuffix)
                .WithResponsibleForCircuit(_elementData.Circuit.ResponsibleForCircuitParameter)
                .WithCurrentLevel()
                .WithHost(host)
                .WithParameters(_elementData.Parameters)
                .WithDirection(_elementData.Direction)
                .Build(_elementData.Name);
        }

        public BuiltInstance InstallElectricalFixtures(Reference host = null)
        {
            return _familyInstanceBuilder
                .WithSwitchNumbers(_elementData.SwitchKeys)
                .SetLocationStrategy(_locationStrategy)
                .WithTypeOfHeight(_elementData.Height.TypeOf)
                .WithHeight(_elementData.Height.FromFloor, _elementData.Height.FromLevel)
                .WithResponsibleForHeight(_elementData.Height.ResponsibleForHeightParameter)
                .WithFamilyName(_elementData.Family)
                //.RenderHeightAs(_heightFormat)
                .WithCircuit(_elementData.Circuit.Number)
                .WithCircuitSuffix(_elementData.CurrentSuffix)
                .WithResponsibleForCircuit(_elementData.Circuit.ResponsibleForCircuitParameter)
                .WithCurrentLevel()
                .WithHost(host)
                .WithParameters(_elementData.Parameters)
                .WithDirection(_elementData.Direction)
                .Build(_elementData.Name);
        }

        public BuiltInstance InstallCommunicationDevices(Reference host = null)
        {
            return _familyInstanceBuilder
                .SetLocationStrategy(_locationStrategy)
                .WithTypeOfHeight(_elementData.Height.TypeOf)
                .WithHeight(_elementData.Height.FromFloor, _elementData.Height.FromLevel)
                .WithResponsibleForHeight(_elementData.Height.ResponsibleForHeightParameter)
                .WithFamilyName(_elementData.Family)
                //.RenderHeightAs(_heightFormat)
                .WithCurrentLevel()
                .WithHost(host)
                .WithParameters(_elementData.Parameters)
                .WithDirection(_elementData.Direction)
                .Build(_elementData.Name);
        }

        private ILocationStrategy GetLocationStrategy(TypeOfHeight typeOfHeight)
        {
            switch (typeOfHeight)
            {
                case TypeOfHeight.UK:
                    return new BottomLocationStrategy(_uiapp) { HorizontalOffset = _elementData.Offset };
                case TypeOfHeight.OK:
                    return new TopLocationStrategy(_uiapp) { HorizontalOffset = _elementData.Offset };
                case TypeOfHeight.Center:
                    return new CenterLocationStrategy(_uiapp) { HorizontalOffset = _elementData.Offset };
            }
            return null;
        }
    }
}

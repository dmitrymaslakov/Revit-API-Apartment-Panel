using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
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
            _heightFormat = height => $"{_elementData.Height.TypeOf}={height}";
        }

        public BuiltInstance InstallLightingFixtures(Reference host = null)
        {
            return _familyInstanceBuilder
                .WithCircuit(_elementData.Circuit)
                .WithCurrentLevel()
                .WithLampSuffix(_elementData.CurrentSuffix)
                .WithHost(host)
                .WithParameters(_elementData.Parameters)
                //.WithOffset(_elementData.Offset)
                .Build(_elementData.Name);
        }

        public BuiltInstance InstallLightingDevices(Reference host = null)
        {
            return _familyInstanceBuilder
                .WithSwitchNumbers(_elementData.SwitchNumbers)
                .SetLocationStrategy(_locationStrategy)
                .WithHeight(_elementData.Height.Value)
                .RenderHeightAs(_heightFormat)
                .WithCircuit(_elementData.Circuit)
                .WithCurrentLevel()
                .WithHost(host)
                .WithParameters(_elementData.Parameters)
                //.WithOffset(_elementData.Offset)
                .Build(_elementData.Name);
        }

        public BuiltInstance InstallElectricalFixtures(Reference host = null)
        {
            return _familyInstanceBuilder
                .SetLocationStrategy(_locationStrategy)
                .WithHeight(_elementData.Height.Value)
                .RenderHeightAs(_heightFormat)
                .WithCircuit(_elementData.Circuit)
                .WithCurrentLevel()
                .WithHost(host)
                .WithParameters(_elementData.Parameters)
                //.WithOffset(_elementData.Offset)
                .Build(_elementData.Name);
        }

        public BuiltInstance InstallCommunicationDevices(Reference host = null)
        {
            return _familyInstanceBuilder
                .SetLocationStrategy(_locationStrategy)
                .WithHeight(_elementData.Height.Value)
                .RenderHeightAs(_heightFormat)
                .WithCurrentLevel()
                .WithHost(host)
                .WithParameters(_elementData.Parameters)
                //.WithOffset(_elementData.Offset)
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
                    break;
            }
            return null;
        }
    }
}
